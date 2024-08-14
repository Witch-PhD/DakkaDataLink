using Comms_Core;
using Comms_Core.Services;
using Grpc.Core;
using System;
using System.Collections.Generic;
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
            outgoingStreams = new List<IServerStreamWriter<Coords>>();
            dataManager = DataManager.Instance;
        }
        public ServerCallContext CallContext
        {
            get; set;
        }

        public static List<IServerStreamWriter<Coords>> outgoingStreams;

        public string ListeningIp = "0.0.0.0";
        public int ListeningPort = 5005;
        private Server? theServer;
        public void StartServer()
        {
            dataManager.ServerHandlerActive = true;
            theServer = new Server
            {
                Services = { Arty.BindService(this) },
                Ports = { new ServerPort(ListeningIp, ListeningPort, ServerCredentials.Insecure) }

            };
            theServer.Start();
            Console.WriteLine($"Server listening on {ListeningIp}:{ListeningPort}");
        }

        public async void StopServer()
        {
            Task serverShutdown = theServer.KillAsync();
            await serverShutdown;
            dataManager.ServerHandlerActive = false;
            theServer = null;
        }


        /// <summary>
        /// Called remotely by a connecting gun.
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task openStream(IAsyncStreamReader<Coords> requestStream, IServerStreamWriter<Coords> responseStream, ServerCallContext context)
        {
           
            outgoingStreams.Add(responseStream);
            dataManager.ConnectedClients++;
            Console.WriteLine($"openStream(): {context.Peer}. Connected. {dataManager.ConnectedClients} connections now active.");
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                //Coords newCoords = requestStream.Current;
                _ = requestStream.Current;
            }

            outgoingStreams.Remove(responseStream);
            dataManager.ConnectedClients--;
            Console.WriteLine($"openStream(): {context.Peer}. Disconnected. {dataManager.ConnectedClients} connections now active.");
        }

        public async void sendNewCoords(double _az, double _dist)
        {
            //Console.WriteLine($"sendNewCoords entered");
            Console.WriteLine($"sendNewCoords az: {_az}, dist: {_dist} to {dataManager.ConnectedClients} guns.");
            foreach (IServerStreamWriter<Coords> gun in outgoingStreams)
            {
                
                await gun.WriteAsync(new Coords { Az = _az, Dist = _dist });
            }
        }
    }
}
