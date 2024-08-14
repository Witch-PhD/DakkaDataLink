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
            outgoingStreams = new List<IServerStreamWriter<Coords>>();
            dataManager = DataManager.Instance;
        }
        public ServerCallContext CallContext
        {
            get; set;
        }



        public static List<IServerStreamWriter<Coords>> outgoingStreams;

        public string ListeningIp = "0.0.0.0"; // TODO: Likely need to find the available IP addresses on the local machine.
        private int defaultListeningPort = 50082; // TODO: Set this to 0 to get it auto-assigned?
        private Server? theServer;
        public void StartServer()
        {
            dataManager.ServerHandlerActive = true;
            theServer = new Server
            {
                Services = { Arty.BindService(this) },
                Ports = { new ServerPort(ListeningIp, defaultListeningPort, ServerCredentials.Insecure) }

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
        public override async Task openStream(IAsyncStreamReader<Coords> requestStream, IServerStreamWriter<Coords> responseStream, ServerCallContext context)
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
                    //Coords newCoords = requestStream.Current;
                    _ = requestStream.Current;
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

        public async void sendNewCoords(double _az, double _dist)
        {
            //Console.WriteLine($"sendNewCoords entered");
            Console.WriteLine($"sendNewCoords az: {_az}, dist: {_dist} to {outgoingStreams.Count} guns.");
            foreach (IServerStreamWriter<Coords> gun in outgoingStreams)
            {
                
                await gun.WriteAsync(new Coords { Az = _az, Dist = _dist });
            }
        }
    }
}
