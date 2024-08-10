using Comms.Services;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace Comms
{
    public class GunBattery
    {
        private List<Arty.ArtyClient> spottersList = new List<Arty.ArtyClient>();

        private static GunBattery? m_Instance;
        public static GunBattery Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GunBattery();
                }
                return m_Instance;
            }
            private set { }
        }

        private GunBattery()
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
                    options.Listen(IPAddress.Any, 5001, listenOptions =>
                    {

                        listenOptions.Protocols = HttpProtocols.Http1;
                    });
                });











                //WebApplicationBuilder builder = WebApplication.CreateBuilder();

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

        public void TrySubscribeToSpotter(string spotterEndPoint)
        {
            GrpcChannel channel = GrpcChannel.ForAddress(spotterEndPoint);
            
            var client = new Arty.ArtyClient(channel);

            var response = client.notifySubscribe(new subscribeNotification { Callsign = "GunBattery1" });
        }

        internal void receiveNewCoords(double az, double dist)
        {
            double test = az;
        }

    }
}
