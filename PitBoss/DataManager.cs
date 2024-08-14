using Comms_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PitBoss
{
    public class DataManager : INotifyPropertyChanged
    {
        private static DataManager? m_Instance;
        public static DataManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DataManager();
                    m_Instance.serverHandler = gRpcServerHandler.Instance;
                    m_Instance.clientHandler = gRpcClientHandler.Instance;

                }
                return m_Instance;
            }
            private set { }
        }

        
        private DataManager()
        {
            m_userOptions = new UserOptions();
        }

        private gRpcServerHandler serverHandler;
        private gRpcClientHandler clientHandler;
        private UserOptions m_userOptions;
        public UserOptions userOptions
        {
            get
            {
                return m_userOptions;
            }
            set
            {
                if (m_userOptions != value)
                {
                    m_userOptions = value;
                    OnPropertyChanged();
                }
            }
        }

        private double m_LatestAz = 0.0;
        public double LatestAz
        {
            get
            {
                return m_LatestAz;
            }
            set
            {
                m_LatestAz = value;
                if (m_LatestAz >= 360.0)
                {
                    m_LatestAz -= 360.0;
                }
                else if (m_LatestAz < 0.0)
                {
                    m_LatestAz += 360.0;
                }
                OnPropertyChanged();
            }
        }


        private double m_LatestDist = 0.0;
        public double LatestDist
        {
            get
            {
                return m_LatestDist;
            }
            set
            {
                m_LatestDist = value;
                OnPropertyChanged();
            }
        }


        private bool m_ClientHandlerActive = false;
        public bool ClientHandlerActive
        {
            get
            {
                return m_ClientHandlerActive;
            }
            set
            {
                if (value != m_ClientHandlerActive)
                {
                    m_ClientHandlerActive = value;

                    OnPropertyChanged();
                }
            }
        }

        private bool m_ServerHandlerActive = false;
        public bool ServerHandlerActive
        {
            get
            {
                return m_ServerHandlerActive;
            }
            set
            {
                if (value != m_ServerHandlerActive)
                {
                    m_ServerHandlerActive = value;

                    OnPropertyChanged();
                }
            }
        }

        private int m_ServerActiveListeningPort = 50082;
        public int ServerActiveListeningPort
        {
            get
            {
                return m_ServerActiveListeningPort;
            }
            set
            {
                if (value != m_ServerActiveListeningPort)
                {
                    m_ServerActiveListeningPort = value;

                    OnPropertyChanged();
                }
            }
        }

        private int m_ConnectedClients = 0;
        public int ConnectedClients
        {
            get
            {
                return m_ConnectedClients;
            }
            set
            {
                if (m_ConnectedClients != value)
                {
                    m_ConnectedClients = value;
                    OnPropertyChanged();
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void StartServer()
        {
            serverHandler.StartServer();
        }

        public void StopServer()
        {
            serverHandler.StopServer();
        }

        public void StartClient(string targetIpAndPort)
        {
            clientHandler.connectToServer(targetIpAndPort);
        }

        public void StopClient()
        {
            clientHandler.disconnectFromServer();
        }

        public void SendCoords()
        {
            //Coords newCoords = new Coords{ Az = _az, Dist = _dist };
            serverHandler.sendNewCoords(LatestAz, LatestDist);
        }

        
        public void NewCoordsReceived(Coords coords)
        {
            LatestAz = coords.Az;
            LatestDist = coords.Dist;
            Console.WriteLine($"DataManager.NewCoordsReceived(), Az: {coords.Az}, Dist: {coords.Dist}");
        }


        
    }
}
