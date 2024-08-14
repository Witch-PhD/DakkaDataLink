﻿using Comms_Core;
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
        private gRpcClientHandler()
        {
            dataManager = DataManager.Instance;
        }
        private DataManager dataManager;
        AsyncDuplexStreamingCall<Coords, Coords> duplexStream;
        public IAsyncStreamReader<Comms_Core.Coords> incomingStream;
        public IClientStreamWriter<Comms_Core.Coords> outgoingStream;
        CancellationTokenSource clientShutdownTokenSource;
        Task receiveTask;

        Arty.ArtyClient artyConnection;
        Channel channel;


        private string m_channelTarget = "";
        //public bool Connected = false;
        public void connectToServer(string channelTarget)
        {
            if (dataManager.ClientHandlerActive)
            {
                Console.WriteLine("gRpcClientHandler already connected. Aborting new connection attempt.");
                return;
            }
            
            try
            {
                m_channelTarget = channelTarget;
                Console.WriteLine($"gRpcClientHandler connecting to {m_channelTarget}");
                openGrpcChannel();
                clientShutdownTokenSource = new CancellationTokenSource();
                receiveTask = new Task(receivingTask);
                dataManager.ClientHandlerActive = true;
                receiveTask.Start();
                
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"RpcException: {ex.Message}");
                dataManager.ClientHandlerActive = false;
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

        public void disconnectFromServer()
        {
            duplexStream.Dispose();
            dataManager.ClientHandlerActive = false;
        }

        public async void receivingTask()
        {
            Console.WriteLine("gRpcClientHandler.receivingTask() started.");
            while (dataManager.ClientHandlerActive)
            {
                try
                {
                    if (await incomingStream.MoveNext(clientShutdownTokenSource.Token))
                    {
                        Coords newCoords = incomingStream.Current;
                        dataManager.NewCoordsReceived(newCoords);
                    }
                }
                catch (RpcException ex)
                {
                    Console.WriteLine($"gRpcClientHandler.receivingTask() RpcException: {ex.Message}");
                    duplexStream.Dispose();
                    openGrpcChannel();
                    Thread.Sleep(1000);
                }
            }
        }
    }
}