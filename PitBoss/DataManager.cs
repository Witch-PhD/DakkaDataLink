using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PitBoss
{
    internal class DataManager
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
        private DataManager() { }

        private Comms.Spotter? m_spotter;
        public Comms.Spotter Spotter
        {
            get
            {
                if (m_spotter == null)
                {
                    m_spotter = Comms.Spotter.Instance;
                }
                return (m_spotter);
            }
            private set
            {
                
            }
        }

        private Comms.GunBattery? m_gunBattery;
        public Comms.GunBattery GunBattery
        {
            get
            {
                if (m_gunBattery == null)
                {
                    m_gunBattery = Comms.GunBattery.Instance;
                }
                return (m_gunBattery);
            }
            private set
            {

            }
        }
    }
}
