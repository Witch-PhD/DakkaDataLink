using PitBoss.UserControls;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PitBoss.Constants;

namespace PitBoss
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//#if DEBUG
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32", SetLastError = true)]
        public static extern void FreeConsole();
//#endif
        public MainWindow()
        {
//#if INCLUDE_DEBUG_CONSOLE
            AllocConsole();
//#endif
            InitializeComponent();
            this.Title = "The Pit Boss";
            this.SizeToContent = SizeToContent.WidthAndHeight;
            dataManager = DataManager.Instance;
            StatusBar_GunsConnectedValue_TextBlock.DataContext = dataManager;
        }

        DataManager dataManager;

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            theUserOptionsUserControl.CloseAllWindows();
            FreeConsole();
            //theSpotterUserControl.CloseAllWindows();
            //theGunnerUserControl.CloseAllWindows();
        }

        public void SetOperatingMode(DataManager.ProgramOperatingMode mode)
        {
            switch (mode)
            {
                case DataManager.ProgramOperatingMode.eIdle:
                    //Gunner_TabItem.IsEnabled = true;
                    //Spotter_TabItem.IsEnabled = true;
                    Gunner_TabItem.Visibility = Visibility.Collapsed;
                    Spotter_TabItem.Visibility = Visibility.Collapsed;
                    break;

                case DataManager.ProgramOperatingMode.eSpotter:
                    //Spotter_TabItem.IsSelected = true;
                    //Gunner_TabItem.IsEnabled = false;
                    Spotter_TabItem.Visibility = Visibility.Visible;
                    Gunner_TabItem.Visibility = Visibility.Collapsed;
                    break;

                case DataManager.ProgramOperatingMode.eGunner:
                    //Gunner_TabItem.IsSelected = true;
                    //Spotter_TabItem.IsEnabled = false;
                    Gunner_TabItem.Visibility= Visibility.Visible;
                    Spotter_TabItem.Visibility = Visibility.Collapsed;
                    break;

                default:
                    Gunner_TabItem.IsEnabled = true;
                    Spotter_TabItem.IsEnabled = true;
                    //Gunner_TabItem.Visibility = Visibility.Visible;
                    //Spotter_TabItem.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}