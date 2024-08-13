using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PitBoss.UserControls
{
    /// <summary>
    /// Interaction logic for Spotter_UserControl.xaml
    /// </summary>
    public partial class Spotter_UserControl : UserControl
    {
        List<Key> keysToMonitor = new List<Key>();
        string keyMsg;
        public Spotter_UserControl()
        {
            InitializeComponent();
            Az_TextBox.DataContext = DataManager.Instance;
            Dist_TextBox.DataContext = DataManager.Instance;

        }


        private void OpenConnection_Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataManager.Instance.ConnectionStatus == Constants.ConnectionStatus.Connected_As_Spotter)
            {
                DataManager.Instance.StopServer();
                OpenConnection_Button.Content = "Open Connections";
            }
            else
            {
                DataManager.Instance.StartServer();
                SpotterKeystrokeHandler.Instance.Activate();
                OpenConnection_Button.Content = "Stop Connection";
            }
            
        }

        private void SendCoords_Button_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Instance.SendNewCoords(DataManager.Instance.LatestAz, DataManager.Instance.LatestDist);
        }
    }
}
