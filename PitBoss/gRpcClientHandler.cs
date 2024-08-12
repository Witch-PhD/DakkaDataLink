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


        public IAsyncStreamReader<Comms_Core.Coords> incomingStream;
        public IClientStreamWriter<Comms_Core.Coords> outgoingStream;
        CancellationTokenSource clientShutdownTokenSource;
        Task receiveTask;

        Arty.ArtyClient artyConnection;
        Channel channel;
        public void connectToServer(string channelTarget)
        {
            Console.WriteLine($"gRpcClientHandler connecting to {channelTarget}");
            channel = new Grpc.Core.Channel(channelTarget, ChannelCredentials.Insecure);
            artyConnection = new Arty.ArtyClient(channel);
            clientShutdownTokenSource = new CancellationTokenSource();

            AsyncDuplexStreamingCall<Coords, Coords> duplexStream = artyConnection.openStream();
            incomingStream = duplexStream.ResponseStream;
            outgoingStream = duplexStream.RequestStream;

            receiveTask = new Task(receivingTask);
            receiveTask.Start();
        }

        public async void receivingTask()
        {
            Console.WriteLine("gRpcClientHandler.receivingTask() started.");

            while (await incomingStream.MoveNext(clientShutdownTokenSource.Token))
            {
                Coords newCoords = incomingStream.Current;
                DataManager.Instance.NewCoordsReceived(newCoords);
            }
        }
    }
}
