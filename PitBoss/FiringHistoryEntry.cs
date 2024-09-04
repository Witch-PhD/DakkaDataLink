using Comms_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PitBoss
{
    public class FiringHistoryEntry : INotifyPropertyChanged
    {
        public FiringHistoryEntry()
        {
        
        }

        public FiringHistoryEntry(ArtyMsg artyMsg)
        {
            Dist = artyMsg.Dist;
            Az = artyMsg.Az;
        }

        private double m_Az = 0.0;
        public double Az
        {
            get
            {
                return m_Az;
            }
            set
            {
                m_Az = value;
                OnPropertyChanged();
            }
        }

        private double m_Dist = 0.0;
        public double Dist
        {
            get
            {
                return m_Dist;
            }
            set
            {
                m_Dist = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
