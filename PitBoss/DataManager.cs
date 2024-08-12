using Comms_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PitBoss
{
    public class DataManager
    {
        private static DataManager? m_Instance;
        public static DataManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DataManager();
                }
                return m_Instance;
            }
            private set { }
        }

        gRpcServerHandler serverHandler = gRpcServerHandler.Instance;
        gRpcClientHandler clientHandler = gRpcClientHandler.Instance;

        public void StartServer()
        {
            serverHandler.StartServer();
        }

        public void StartClient()
        {
            clientHandler.connectToServer(serverHandler.ListeningIp + ":" + serverHandler.ListeningPort);
        }

        public void SendNewCoords(double _az, double _dist)
        {
            //Coords newCoords = new Coords{ Az = _az, Dist = _dist };
            serverHandler.sendNewCoords(_az, _dist);
        }

        public void NewCoordsReceived(Coords coords)
        {
            Console.WriteLine($"DataManager.NewCoordsReceived(), Az: {coords.Az}, Dist: {coords.Dist}");
        }

    }
}
