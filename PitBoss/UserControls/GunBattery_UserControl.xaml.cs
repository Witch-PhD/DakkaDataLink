using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
    /// Interaction logic for GunBattery_UserControl.xaml
    /// </summary>
    public partial class GunBattery_UserControl : UserControl
    {
        public GunBattery_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            
            DataContext = dataManager;
            
        }
        DataManager dataManager;

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.ClientHandlerActive)
            {
                DataManager.Instance.StopClient();
                Connect_Button.Content = "Connect";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(MainWindow.ProgramOperatingMode.eIdle);
            }
            else
            {
                bool validIP = IPEndPoint.TryParse(spotterIp_TextBox.Text + $":{Constants.SERVER_PORT}", out _);
                if (validIP)
                {
                    DataManager.Instance.StartClient(spotterIp_TextBox.Text + $":{Constants.SERVER_PORT}");
                    Connect_Button.Content = "Disconnect";

                    MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                    mainWindow.SetOperatingMode(MainWindow.ProgramOperatingMode.eGunner);
                }
                else
                {
                    Console.WriteLine("*** Invalid spotter IP address. Check your values.");
                }
            }
        }

        
    }
}
