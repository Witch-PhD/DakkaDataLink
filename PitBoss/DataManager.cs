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
                }
                return m_Instance;
            }
            private set { }
        }

        
        private DataManager()
        {
            //App.KListener.KeyDown += KListener_KeyDown;
            
        }

        ~DataManager()
        {
            //App.KListener.KeyDown -= KListener_KeyDown;
        }

        gRpcServerHandler serverHandler = gRpcServerHandler.Instance;
        gRpcClientHandler clientHandler = gRpcClientHandler.Instance;

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


        private Constants.ConnectionStatus m_ConnectionStatus = Constants.ConnectionStatus.Disconnected;
        public Constants.ConnectionStatus ConnectionStatus
        {
            get
            {
                return m_ConnectionStatus;
            }
            set
            {
                if (value != m_ConnectionStatus)
                {
                    m_ConnectionStatus = value;

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

        public void SendNewCoords(double _az, double _dist)
        {
            //Coords newCoords = new Coords{ Az = _az, Dist = _dist };
            serverHandler.sendNewCoords(_az, _dist);
        }

        
        public void NewCoordsReceived(Coords coords)
        {
            LatestAz = coords.Az;
            LatestDist = coords.Dist;
            Console.WriteLine($"DataManager.NewCoordsReceived(), Az: {coords.Az}, Dist: {coords.Dist}");
        }


        
    }
}
