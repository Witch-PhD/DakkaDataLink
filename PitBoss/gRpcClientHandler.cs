using Comms_Core;
using Comms_Core.Services;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PitBoss.PitBossConstants;

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
        private gRpcClientHandler()
        {
            dataManager = DataManager.Instance;
        }
        private DataManager dataManager;
        AsyncDuplexStreamingCall<ArtyMsg, ArtyMsg> duplexStream;
        public IAsyncStreamReader<Comms_Core.ArtyMsg> incomingStream;
        public IClientStreamWriter<Comms_Core.ArtyMsg> outgoingStream;
        CancellationTokenSource clientShutdownTokenSource;
        Task receiveTask;

        Arty.ArtyClient artyConnection;
        Channel channel;


        private string m_channelTarget = "";
        //public bool Connected = false;
        public void connectToServer(string channelTarget)
        {
            if (dataManager.GrpcClientHandlerActive)
            {
                //Console.WriteLine("gRpcClientHandler already connected. Aborting new connection attempt.");
                GlobalLogger.Log("gRPC Client already connected to server. Aborting new connection attempt.");
                return;
            }
            
            try
            {
                m_channelTarget = channelTarget;
                //Console.WriteLine($"gRPC Client connecting to {m_channelTarget}");
                GlobalLogger.Log($"gRPC Client connecting to {m_channelTarget}");
                openGrpcChannel();
                clientShutdownTokenSource = new CancellationTokenSource();
                clientShutdownTokenSource.Token.ThrowIfCancellationRequested();
                receiveTask = new Task(receivingTask);
                dataManager.GrpcClientHandlerActive = true;
                receiveTask.Start();
                
            }
            catch (RpcException ex)
            {
                //Console.WriteLine($"gRPC Client RpcException: {ex.Message}");
                GlobalLogger.Log($"gRPC Client RpcException: {ex.Message}");
                dataManager.GrpcClientHandlerActive = false;
            }
        }

        private void openGrpcChannel()
        {
            channel = new Grpc.Core.Channel(m_channelTarget, ChannelCredentials.Insecure);
            artyConnection = new Arty.ArtyClient(channel);

            duplexStream = artyConnection.openStream();
            incomingStream = duplexStream.ResponseStream;
            outgoingStream = duplexStream.RequestStream;
        }

        public async void disconnectFromServer()
        {
            dataManager.GrpcClientHandlerActive = false;
            clientShutdownTokenSource?.Cancel();
            Thread.Sleep(100);
            duplexStream?.Dispose();
            
            await receiveTask;
        }

        public async void receivingTask()
        {
            //Console.WriteLine("gRpc Client receivingTask started.");
            GlobalLogger.Log("gRpc Client receivingTask started.");
            while (dataManager.GrpcClientHandlerActive)
            {
                try
                {
                    if (await incomingStream.MoveNext(clientShutdownTokenSource.Token))
                    {
                        ArtyMsg newMsg = incomingStream.Current;
                        dataManager.NewArtyMsgReceived(newMsg);
                    }
                }
                catch (RpcException ex)
                {
                    if (ex.StatusCode == StatusCode.Cancelled)
                    {
                        break;
                    }
                    else
                    {
                        //Console.WriteLine($"gRpcClientHandler.receivingTask RpcException: {ex.Message}");
                        GlobalLogger.Log($"gRpc Client receivingTask RpcException: {ex.Message}");
                        duplexStream.Dispose();
                        openGrpcChannel();
                        Thread.Sleep(1000);
                    }
                }
            }
            //Console.WriteLine("gRpcClientHandler receivingTask ending.");
            GlobalLogger.Log("gRpcClientHandler receivingTask ending.");
        }

        public async void sendArtyMsg(ArtyMsg artyMsg)
        {
            ////Console.WriteLine($"gRpc Client sending new ArtyMsg: CallSign: {artyMsg.Callsign} Az: {artyMsg.Az}, Dist: {artyMsg.Dist}, Connected Guns: {artyMsg.ConnectedGuns}");
            //GlobalLogger.Log($"gRpc Client sending new ArtyMsg: CallSign: {artyMsg.Callsign} Az: {artyMsg.Az}, Dist: {artyMsg.Dist}, Connected Guns: {artyMsg.ConnectedGuns}");
            try
            {
                await outgoingStream.WriteAsync(artyMsg);
            }
            catch (RpcException ex)
            {
                //Console.WriteLine($"*** gRpc Client.sendArtyMsg RpcException: {ex.Message}");
                GlobalLogger.Log($"*** gRpc Client.sendArtyMsg RpcException: {ex.Message}");
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"*** gRpc Client.sendArtyMsg Other Exception: {ex.Message}");
                GlobalLogger.Log($"*** gRpc Client.sendArtyMsg Other Exception: {ex.Message}");
            }
        }
    }
}
