using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PitBoss
{
    public class UserOptions : INotifyPropertyChanged
    {
        public UserOptions()
        {
            BindingDictionary = new Dictionary<string, KeyCombo>();
            LoadDefaultKeyBindings();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region OverlaySettings

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
        #endregion

        #region KeyBindings
        public Dictionary<string, KeyCombo> BindingDictionary;

        public const string AZ_UP_ONE_DEG_DICT_KEY = "Az_Up_One_Deg_Binding";
        public const string AZ_UP_MULTI_DEG_DICT_KEY = "Az_Up_Multi_Deg_Binding";
        public const string AZ_DOWN_ONE_DEG_DICT_KEY = "Az_Down_One_Deg_Binding";
        public const string AZ_DOWN_MULTI_DEG_DICT_KEY = "Az_Down_Multi_Deg_Binding";

        public const string DIST_UP_ONE_TICK_DICT_KEY = "Dist_Up_One_Tick_Binding";
        public const string DIST_UP_MULTI_TICK_DICT_KEY = "Dist_Up_Multi_Tick_Binding";
        public const string DIST_DOWN_ONE_TICK_DICT_KEY = "Dist_Down_One_Tick_Binding";
        public const string DIST_DOWN_MULTI_TICK_DICT_KEY = "Dist_Down_Multi_Tick_Binding";

        public const string SEND_ARTY_MSG_DICT_KEY = "Send_Arty_Msg_Binding";

        public void LoadDefaultKeyBindings()
        {
            BindingDictionary.Add(AZ_UP_ONE_DEG_DICT_KEY, new KeyCombo(Key.RightCtrl, Key.Right));
            BindingDictionary.Add(AZ_UP_MULTI_DEG_DICT_KEY, new KeyCombo(Key.RightShift, Key.Right));
            BindingDictionary.Add(AZ_DOWN_ONE_DEG_DICT_KEY, new KeyCombo(Key.RightCtrl, Key.Left));
            BindingDictionary.Add(AZ_DOWN_MULTI_DEG_DICT_KEY, new KeyCombo(Key.RightShift, Key.Left));

            BindingDictionary.Add(DIST_UP_ONE_TICK_DICT_KEY, new KeyCombo(Key.RightCtrl, Key.Up));
            BindingDictionary.Add(DIST_UP_MULTI_TICK_DICT_KEY, new KeyCombo(Key.RightShift, Key.Up));
            BindingDictionary.Add(DIST_DOWN_ONE_TICK_DICT_KEY, new KeyCombo(Key.RightCtrl, Key.Down));
            BindingDictionary.Add(DIST_DOWN_MULTI_TICK_DICT_KEY, new KeyCombo(Key.RightShift, Key.Down));

            BindingDictionary.Add(SEND_ARTY_MSG_DICT_KEY, new KeyCombo(Key.RightCtrl, Key.Up));
        }

        #endregion
    }
}
