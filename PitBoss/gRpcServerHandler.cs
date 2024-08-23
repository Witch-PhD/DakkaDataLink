using Comms_Core;
using Comms_Core.Services;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PitBoss
{
    public class gRpcServerHandler : ArtyService
    {
        private static gRpcServerHandler? m_Instance;
        private DataManager dataManager;
        public static gRpcServerHandler Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new gRpcServerHandler();
                }
                return m_Instance;
            }
            protected set
            {

            }
        }
        private gRpcServerHandler()
        {
            outgoingStreams = new List<IServerStreamWriter<ArtyMsg>>();
            dataManager = DataManager.Instance;
        }
        public ServerCallContext CallContext
        {
            get; set;
        }



        public static List<IServerStreamWriter<ArtyMsg>> outgoingStreams;

        public string ListeningIp = "0.0.0.0"; // TODO: Likely need to find the available IP addresses on the local machine.
        private int m_listeningPort = Constants.SERVER_PORT;
        private Server? theServer;
        public void StartServer()
        {
            dataManager.ServerHandlerActive = true;
            theServer = new Server
            {
                Services = { Arty.BindService(this) },
                Ports = { new ServerPort(ListeningIp, m_listeningPort, ServerCredentials.Insecure) }

            };
            theServer.Start();
            Thread.Sleep(100);
            IEnumerator<ServerPort> portsEnumerator = theServer.Ports.GetEnumerator();
            //portsEnumerator.Reset();
            foreach (ServerPort port in theServer.Ports)
            {
                //Console.WriteLine($"Server listening on {ListeningIp}:{port.BoundPort}");
                dataManager.ServerActiveListeningPort = port.BoundPort;
            }
            //dataManager.ServerActiveListeningPort = theServer.Ports[portsEnumerator];
            Console.WriteLine($"Server listening on {ListeningIp}:{dataManager.ServerActiveListeningPort}");
        }

        public async void StopServer()
        {
            Task serverShutdown = theServer.KillAsync();
            await serverShutdown;
            dataManager.ServerHandlerActive = false;
            theServer = null;
            Console.WriteLine($"Server stopped.");
        }


        /// <summary>
        /// Called remotely by a connecting gun.
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task openStream(IAsyncStreamReader<ArtyMsg> requestStream, IServerStreamWriter<ArtyMsg> responseStream, ServerCallContext context)
        {
            string currentPeer = context.Peer;
            try
            {
                outgoingStreams.Add(responseStream);
                dataManager.ConnectedClients = outgoingStreams.Count;
                Console.WriteLine($"openStream(): {context.Peer}. Connected. {outgoingStreams.Count} connections now active.");
                while (await requestStream.MoveNext(context.CancellationToken))
                {
                    if (context.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    ArtyMsg newMsg = requestStream.Current;
                    if (dataManager.OperatingMode == DataManager.ProgramOperatingMode.eGunner)
                    {
                        newMsg.ConnectedGuns = dataManager.ConnectedClients;
                        dataManager.NewArtyMsgReceived(newMsg);
                        sendNewCoords(newMsg);
                    }
                    else
                    {
                        //if (!dataManager.ConnectedGunsCallsigns.Contains(newMsg.Callsign))
                        //{
                        //    dataManager.ConnectedGunsCallsigns.Add(newMsg.Callsign);
                        //}
                    }
                    
                    //_ = requestStream.Current;
                }

            }
            finally
            {
                outgoingStreams.Remove(responseStream);
                dataManager.ConnectedClients = outgoingStreams.Count;
                //dataManager.ConnectedClients--;
                Console.WriteLine($"openStream(): {currentPeer}. Disconnected. {outgoingStreams.Count} connections now active.");
            }
        }

        public async void sendNewCoords(ArtyMsg artyMsg)
        {
            //Console.WriteLine($"sendNewCoords entered");
            Console.WriteLine($"gRpcServerHandler.sendNewCoords(): CallSign: {artyMsg.Callsign} Az: {artyMsg.Az}, Dist: {artyMsg.Dist}, Connected Guns: {artyMsg.ConnectedGuns}");
            foreach (IServerStreamWriter<ArtyMsg> gun in outgoingStreams)
            {
                
                await gun.WriteAsync(artyMsg);
            }
        }
    }
}
