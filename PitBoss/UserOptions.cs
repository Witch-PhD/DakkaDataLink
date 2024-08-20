using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PitBoss
{
    public class UserOptions : INotifyPropertyChanged
    {
        public UserOptions()
        {
            
        }

        private double m_OverlayTransparency = 0.5;
        public double OverlayTransparency
        {
            get
            {
                return m_OverlayTransparency;
            }
            set
            {
                if (m_OverlayTransparency != value)
                {
                    m_OverlayTransparency = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush m_OverlayBackgroundColor = Brushes.DarkGray;
        public Brush OverlayBackgroundColor
        {
            get
            {
                return m_OverlayBackgroundColor;
            }
            set
            {
                if (m_OverlayBackgroundColor != value)
                {
                    m_OverlayBackgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush m_OverlayValuesFontColor = Brushes.Red;
        public Brush OverlayValuesFontColor
        {
            get
            {
                return m_OverlayValuesFontColor;
            }
            set
            {
                if (m_OverlayValuesFontColor != value)
                {
                    m_OverlayValuesFontColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush m_OverlayLabelsFontColor = Brushes.Bisque;
        public Brush OverlayLabelsFontColor
        {
            get
            {
                return m_OverlayLabelsFontColor;
            }
            set
            {
                if (m_OverlayLabelsFontColor != value)
                {
                    m_OverlayLabelsFontColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private int m_OverlayFontSize = 12;
        public int OverlayFontSize
        {
            get
            {
                return m_OverlayFontSize;
            }
            set
            {
                if (m_OverlayFontSize != value)
                {
                    m_OverlayFontSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
