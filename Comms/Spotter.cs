using Comms.Services;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace Comms
{
    public class Spotter
    {
        private Dictionary<string, Arty.ArtyClient> batteriesList = new Dictionary<string, Arty.ArtyClient>();

        private static Spotter? m_Instance;
        public static Spotter Instance
        { 
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Spotter();
                }
                return m_Instance;
            }
            private set
            {

            }
        }
        private Spotter()
        {

        }

        private WebApplication? webServer;
        private Task? webServerTask;
        
        public void BeginAcceptingConnections()
        {
            if ((webServer == null) && (webServerTask == null))
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder();




               
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 5005, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1;
                    });
                });








                // Add services to the container.
                builder.Services.AddGrpc();

                webServer = builder.Build();

                // Configure the HTTP request pipeline.
                //app.MapGrpcService<GreeterService>();
                //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

                webServer.MapGrpcService<ArtyService>();
                webServer.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

                webServerTask = webServer.RunAsync();
            }
        }

        public async void StopConnections()
        {
            //foreach (Arty.ArtyClient client in batteriesList)
            //{
            //    client.
            //}
            if (webServer != null)
            {
                await webServer.StopAsync();
                webServer = null;
            }
            if (webServerTask != null)
            {
                webServerTask.Dispose();
                webServerTask = null;
            }
        }

        internal void AddNewSubscriber(string incomingPeer)
        {
            if (!batteriesList.ContainsKey(incomingPeer))
            {
                GrpcChannel subscriberChannel = GrpcChannel.ForAddress(incomingPeer);
                batteriesList[incomingPeer] = new Arty.ArtyClient(subscriberChannel);
            }
        }

        public void SendNewCoords(double _Az, double _Dist)
        {
            foreach (Arty.ArtyClient client in batteriesList.Values)
            {
                MsgStatus msgStatus = client.receiveNewCoords(new Coords { Az = _Az, Dist = _Dist });

            }
        }
    }
}
