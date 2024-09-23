using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

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

        protected string m_MyCallSign = "None";
        public string MyCallSign
        {
            get
            {
                return m_MyCallSign;
            }
            set
            {
                if (m_MyCallSign != value)
                {
                    m_MyCallSign = value;
                    m_MyCallSign = m_MyCallSign.Replace(" (Spotter)", "");
                    m_MyCallSign = m_MyCallSign.Replace(" (Gunner)", "");
                    OnPropertyChanged();
                }
            }
        }

        #region OverlaySettings

        protected double m_OverlayTransparency = 0.5;
        public double OverlayOpacity
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

        protected Brush m_OverlayBackgroundColor = Brushes.DarkGray;
        [XmlIgnore]
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

        protected Brush m_OverlayValuesFontColor = Brushes.Red;
        [XmlIgnore]
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

        protected Brush m_OverlayLabelsFontColor = Brushes.Bisque;
        [XmlIgnore]
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

        protected int m_OverlayFontSize = 12;

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
        [XmlIgnore]
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

            BindingDictionary.Add(SEND_ARTY_MSG_DICT_KEY, new KeyCombo(Key.RightCtrl, Key.NumPad0));
        }

        #endregion

        #region Logger
        protected string m_LoggerFilePath = "Log.txt";
        public string LoggerFilePath
        {
            get
            {
                return m_LoggerFilePath;
            }
            set
            {
                m_LoggerFilePath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Language
        protected string m_Language = "English";
        public string Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                m_Language = value;
                OnPropertyChanged();
            }
        }

        protected bool m_SaveSelectedLanguage = false;
        public bool SaveSelectedLanguage
        {
            get
            {
                return m_SaveSelectedLanguage;
            }
            set
            {
                m_SaveSelectedLanguage = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
