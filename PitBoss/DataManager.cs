using Comms_Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                    m_Instance.m_userOptions = new UserOptions();
                    m_Instance.m_ArtyProfiles = ArtilleryProfiles.Instance;
                    //m_Instance.ConnectedGunsCallsigns = new ObservableCollection<string>();
                }
                return m_Instance;
            }
            private set { }
        }

        
        private DataManager()
        {
            //m_userOptions = new UserOptions();
            
        }

        private gRpcServerHandler serverHandler;
        private gRpcClientHandler clientHandler;
        private UserOptions m_userOptions;
        private ArtilleryProfiles m_ArtyProfiles;

        //public ObservableCollection<string> ConnectedGunsCallsigns { get; set; }
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

        

        public enum ProgramOperatingMode
        {
            eIdle,
            eSpotter,
            eGunner
        }
        private ProgramOperatingMode m_operatingMode;
        public ProgramOperatingMode OperatingMode
        {
            get
            { return m_operatingMode; }
            set
            {
                if (value != m_operatingMode)
                {
                    m_operatingMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string m_MyCallsign = "";
        public string MyCallsign
        {
            get
            {
                return m_MyCallsign;
            }
            set
            {
                if (m_MyCallsign != value)
                {
                    m_MyCallsign = value;
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


        private double m_LatestDist = 100.0;
        public double LatestDist
        {
            get
            {
                return m_LatestDist;
            }
            set
            {
                if (OperatingMode == ProgramOperatingMode.eSpotter)
                {
                    // Order of these checks is important.
                    if (value < m_ArtyProfiles.CurrentProfile.MinDist)
                    {
                        m_LatestDist = m_ArtyProfiles.CurrentProfile.MinDist;
                    }
                    else if (value <= m_ArtyProfiles.CurrentProfile.MaxDist)
                    {
                        m_LatestDist = value;
                    }
                    else
                    {
                        m_LatestDist = m_ArtyProfiles.CurrentProfile.MaxDist;
                    }
                }
                else
                {
                    m_LatestDist = value;
                }
                
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

        public ArtyMsg getAssembledMsg()
        {
            ArtyMsg artyMsg = new ArtyMsg();
            artyMsg.Az = LatestAz;
            artyMsg.Dist = LatestDist;
            artyMsg.Callsign = MyCallsign;
            if (ServerHandlerActive)
            {
                artyMsg.ConnectedGuns = ConnectedClients;
            }

            return artyMsg;
        }

        public void unpackIncomingArtyMsg(ArtyMsg msg)
        {
            LatestAz = msg.Az;
            LatestDist = msg.Dist;
            if (!(ServerHandlerActive))
            {
                ConnectedClients = msg.ConnectedGuns;
            }
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
            ArtyMsg artyMsg = getAssembledMsg();

            if (ServerHandlerActive)
            {
                serverHandler.sendNewCoords(artyMsg);
            }
            else if (ClientHandlerActive)
            {
                clientHandler.sendNewCoords(artyMsg);
            }
            else
            {
                // wut
            }
        }

        public event EventHandler<bool> newCoordsReceived;
        public void NewArtyMsgReceived(ArtyMsg theMsg)
        {
            unpackIncomingArtyMsg(theMsg);
            
            newCoordsReceived?.Invoke(this, true);
            Console.WriteLine($"DataManager.NewArtyMsgReceived(), CallSign: {theMsg.Callsign} Az: {theMsg.Az}, Dist: {theMsg.Dist}, Connected Guns: {theMsg.ConnectedGuns}");
        }


        
    }
}
