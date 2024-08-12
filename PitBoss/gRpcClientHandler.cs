using Comms_Core;
using Comms_Core.Services;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PitBoss.Constants;

namespace PitBoss
{
    public class gRpcClientHandler
    {

        private static gRpcClientHandler? m_Instance;
        public static gRpcClientHandler Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new gRpcClientHandler();
                }
                return m_Instance;
            }
            private set { }
        }
        private gRpcClientHandler() { }

        AsyncDuplexStreamingCall<Coords, Coords> duplexStream;
        public IAsyncStreamReader<Comms_Core.Coords> incomingStream;
        public IClientStreamWriter<Comms_Core.Coords> outgoingStream;
        CancellationTokenSource clientShutdownTokenSource;
        Task receiveTask;

        Arty.ArtyClient artyConnection;
        Channel channel;

        internal Constants.ConnectionStatus connectionStatus = Constants.ConnectionStatus.Disconnected;

        //public bool Connected = false;
        public void connectToServer(string channelTarget)
        {
            if (connectionStatus == ConnectionStatus.Connected_As_Gunner)
            {
                Console.WriteLine("gRpcClientHandler already connected. Aborting new connection attempt.");
                return;
            }
            try
            {
                Console.WriteLine($"gRpcClientHandler connecting to {channelTarget}");
                channel = new Grpc.Core.Channel(channelTarget, ChannelCredentials.Insecure);
                artyConnection = new Arty.ArtyClient(channel);
                clientShutdownTokenSource = new CancellationTokenSource();

                duplexStream = artyConnection.openStream();
                incomingStream = duplexStream.ResponseStream;
                outgoingStream = duplexStream.RequestStream;

                receiveTask = new Task(receivingTask);
                connectionStatus = ConnectionStatus.Connected_As_Gunner;
                receiveTask.Start();
                
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"RpcException: {ex.Message}");
                connectionStatus = ConnectionStatus.Error;
            }
        }

        public void disconnectFromServer()
        {
            duplexStream.Dispose();
            connectionStatus = Constants.ConnectionStatus.Disconnected;
        }

        public async void receivingTask()
        {
            Console.WriteLine("gRpcClientHandler.receivingTask() started.");
            while (connectionStatus == Constants.ConnectionStatus.Connected_As_Gunner)
            {
                try
                {
                    if (await incomingStream.MoveNext(clientShutdownTokenSource.Token))
                    {
                        Coords newCoords = incomingStream.Current;
                        DataManager.Instance.NewCoordsReceived(newCoords);
                    }
                }
                catch (RpcException ex)
                {
                    Console.WriteLine($"gRpcClientHandler.receivingTask() RpcException: {ex.Message}");
                    disconnectFromServer();
                }
            }
        }
    }
}
