using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Comms_Core;
using Grpc.Core;

namespace PitBoss
{
    class UdpHandler
    {

        private static UdpHandler? m_Instance;

        public static UdpHandler Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new UdpHandler();
                    m_Instance.sendStatusTimer.Elapsed += m_Instance.sendStatusTimerElapsed;
                    m_Instance.sendStatusTimer.Interval = 1500;
                }
                return m_Instance;
            }
            protected set { }
        }
        private UdpHandler()
        { 
        
        }
        private DataManager dataManager = DataManager.Instance;
        public UdpClient udpClient = new UdpClient(PitBossConstants.SERVER_PORT);
        private Thread? m_udpReceiveThread;

        private List<IPEndPoint> m_RemoteEndPoints = new List<IPEndPoint>();
        private IPEndPoint? m_serverIpEndPoint;
        private bool m_RunningAsServer = false;

        private CancellationTokenSource? receiveTaskCancelSource;
        private System.Timers.Timer sendStatusTimer = new System.Timers.Timer(); 
        /// <summary>
        /// Begin UDP operations.
        /// </summary>
        /// <param name="serverIpAddress">Leave this as null if running as the server, otherwise pass in the target server address.</param>
        public void Start(IPAddress? serverIpAddress = null)
        {
            if (m_udpReceiveThread == null)
            {
                if (serverIpAddress != null) // Starting as client
                {
                    m_serverIpEndPoint = new IPEndPoint(serverIpAddress, PitBossConstants.SERVER_PORT);
                    m_RemoteEndPoints.Add(m_serverIpEndPoint);
                    m_RunningAsServer = false;
                    Console.WriteLine($"UdpHandler starting as client...");
                    GlobalLogger.Log($"UdpHandler starting as client...");
                }
                else // Starting as server
                {
                    m_RunningAsServer = true;
                    Console.WriteLine($"UdpHandler starting as server...");
                    GlobalLogger.Log($"UdpHandler starting as server...");
                }
                receiveTaskCancelSource = new CancellationTokenSource();
                dataManager.UdpHandlerActive = true;
                m_udpReceiveThread = new Thread(receivingTask);
                m_udpReceiveThread.Name = "UdpReceiveThread";
                m_udpReceiveThread.IsBackground = true;
                m_RunReceivingTask = true;
                m_udpReceiveThread.Start();
                sendStatusTimer.Start();
            }
            else
            {
                // TODO: Report warning.
            }
        }

        public void Stop()
        {
            Console.WriteLine($"UdpHandler stopping...");
            GlobalLogger.Log($"UdpHandler stopping...");
            sendStatusTimer.Stop();
            m_RunReceivingTask = false;
            receiveTaskCancelSource?.Cancel();
            if (m_udpReceiveThread != null)
            {
                bool stoppedSuccessfully = m_udpReceiveThread.Join(3000);
                if (stoppedSuccessfully)
                {
                    // TODO: Report in log.
                }
                else
                {
                    // TODO: Report error. Retry or kill.
                }
            }
            dataManager.UdpHandlerActive = false; // TODO: Move this somewhere else.
            m_udpReceiveThread = null; // TODO: move this up a block or two?
            m_serverIpEndPoint = null; // TODO: maybe move this to the receive task?
            m_RemoteEndPoints.Clear();
            receiveTaskCancelSource = null;
        }

        

        public void SendCoordsToAll(ArtyMsg msg)
        {
            if (msg.Coords == null)
            {
                // TODO: Report error.
                return;
            }
            Console.WriteLine($"UdpHandler sending new ArtyMsg: CallSign: {msg.Callsign} Az: {msg.Coords.Az}, Dist: {msg.Coords.Dist}");
            GlobalLogger.Log($"UdpHandler sending new ArtyMsg: CallSign: {msg.Callsign} Az: {msg.Coords.Az}, Dist: {msg.Coords.Dist}");
            try
            {
                byte[] rawData = msg.ToByteArray();
                int dataLength = rawData.Length;
                foreach (IPEndPoint endPoint in m_RemoteEndPoints)
                {
                    udpClient.SendAsync(rawData, dataLength, endPoint);
                }
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"*** UdpHandler.SendToAll RpcException: {ex.Message}");
                GlobalLogger.Log($"*** UdpHandler.SendToAll RpcException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*** UdpHandler.SendToAll Other Exception: {ex.Message}");
                GlobalLogger.Log($"*** UdpHandler.SendToAll Other Exception: {ex.Message}");
            }
        }

        private void sendStatusTimerElapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (m_serverIpEndPoint != null) // Running as client
            {
                SendClientReport();
            }
            else // Running as server
            {

            }
        }

        private void SendClientReport()
        {
            ArtyMsg msg = new ArtyMsg();
            msg.Callsign = dataManager.MyCallsign;
            msg.ClientReport = new ClientReport();
            msg.ClientReport.ClientType = (int)dataManager.OperatingMode;
            msg.ClientReport.SpotterPassword = "";

            try
            {
                byte[] rawData = msg.ToByteArray();
                int dataLength = rawData.Length;
                foreach (IPEndPoint endPoint in m_RemoteEndPoints) // This should only have 1 entry (the server) if running as client.
                {
                    udpClient.SendAsync(rawData, dataLength, endPoint);
                }
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"*** UdpHandler.SendClientReport RpcException: {ex.Message}");
                GlobalLogger.Log($"*** UdpHandler.SendClientReport RpcException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*** UdpHandler.SendClientReport Other Exception: {ex.Message}");
                GlobalLogger.Log($"*** UdpHandler.SendClientReport Other Exception: {ex.Message}");
            }
        }

        private bool m_RunReceivingTask = false;
        private async void receivingTask()
        {
            Console.WriteLine($"UdpHandler receive task starting.");
            GlobalLogger.Log($"UdpHandler receive task starting.");

            CancellationToken cancelToken = receiveTaskCancelSource.Token;

            IPEndPoint remoteEndpoint;
            while (m_RunReceivingTask)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync(cancelToken);
                    remoteEndpoint = result.RemoteEndPoint;
                    ArtyMsg theMsg = ArtyMsg.Parser.ParseFrom(result.Buffer);
                    if (!m_RemoteEndPoints.Contains(remoteEndpoint))
                    {
                        m_RemoteEndPoints.Add(remoteEndpoint); // TODO: Need to handle timed out end points.
                    }
                    // TODO: Update endpoint's timeout timer.
                    dataManager.NewArtyMsgReceived(theMsg);
                }
                catch (InvalidProtocolBufferException ex)
                {
                    // TODO: Report malformed data.
                }
                catch (OperationCanceledException ex)
                {
                    // Do nothing. This is supposed to happen when the cancelToken is cancelled during Stop().
                }
            }

            Console.WriteLine($"UdpHandler receive task stopped.");
            GlobalLogger.Log($"UdpHandler receive task stopped.");
        }

    }
}
