using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace DakkaDataLink.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectionsUserControl.xaml
    /// </summary>
    public partial class ConnectionsUserControl : UserControl
    {
        public ConnectionsUserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            dataManager.userOptions.PropertyChanged += UserOptions_PropertyChanged;
            MyCallsign_Textbox.DataContext = dataManager;
            ActiveUsers_DataGrid.ItemsSource = dataManager.ConnectedUsersCallsigns;

#if DEBUG
            serverIp_TextBox.Text = "127.0.0.1";
#endif

        }

        private void UserOptions_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            serverIp_TextBox.Text = dataManager.userOptions.LastServerIp;
        }

        private DataManager dataManager;

        private void StartStopUdpClient_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.UdpHandlerActive) // Stopping
            {
                dataManager.StopUdp();
                StartStopUdpClient_Button.Content = "Connect to Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;
                StartStopUdpServer_Button.IsEnabled = true;
                MyCallsign_Textbox.IsEnabled = true;
                //userIp_stackPanel.Visibility = Visibility.Hidden;
            }
            else // Starting
            {
                
                serverIp_TextBox.Text = serverIp_TextBox.Text.Trim();
                string targetIpString;
                bool validUrl = false;
                
                
                bool validIP = IPAddress.TryParse(serverIp_TextBox.Text, out _);
                if (validIP)
                {
                    targetIpString = serverIp_TextBox.Text;
                }
                else
                {
                    IPAddress[] addresses;
                    try
                    {
                        addresses = Dns.GetHostAddresses(serverIp_TextBox.Text);
                        validUrl = true;
                        targetIpString = addresses[0].ToString();
                    }
                    catch (SocketException ex)
                    {
                        MessageBox.Show(Window.GetWindow(this), "Failed to start: Invalid or unknown URL.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show(Window.GetWindow(this), "Failed to start: Invalid or unknown URL.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }

                setOperatingModes();
                dataManager.userOptions.LastServerIp = serverIp_TextBox.Text;
                dataManager.StartUdpClient(targetIpString);
                StartStopUdpClient_Button.Content = "Disconnect from Server";

                StartStopUdpServer_Button.IsEnabled = false;
                serverIp_TextBox.IsEnabled = false;
                MyCallsign_Textbox.IsEnabled = false;
                //userIp_stackPanel.Visibility = Visibility.Visible;
            }
        }

        private void StartStopUdpServer_Button_Click(Object sender, RoutedEventArgs e)
        {
            if (dataManager.UdpHandlerActive) // Stopping
            {
                DataManager.Instance.StopUdp();
                StartStopUdpServer_Button.Content = "Start as Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;
                StartStopUdpClient_Button.IsEnabled = true;
                MyCallsign_Textbox.IsEnabled = true;
                userIp_stackPanel.Visibility = Visibility.Collapsed;
            }
            else // Starting
            {
                setOperatingModes();
                bool gotIp = ShowExternalIp();
                DataManager.Instance.StartUdpServer();
                StartStopUdpServer_Button.Content = "Stop Server";
                if (gotIp)
                {
                    userIp_stackPanel.Visibility = Visibility.Visible;
                }
                StartStopUdpClient_Button.IsEnabled = false;
                serverIp_TextBox.IsEnabled = false;
                MyCallsign_Textbox.IsEnabled = false;
                
            }
            
        }

        private void setOperatingModes()
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            DataManager.ProgramOperatingMode newMode;

            if ((bool)gunnerMode_RadioButton.IsChecked)
            {
                newMode = DataManager.ProgramOperatingMode.eGunner;
            }
            else
            {
                newMode = DataManager.ProgramOperatingMode.eSpotter;
            }

            gunnerMode_RadioButton.IsEnabled = false;
            spotterMode_RadioButton.IsEnabled = false;

            dataManager.OperatingMode = newMode;
            mainWindow.SetOperatingMode(newMode);

        }

        /// <summary>
        /// Show the external IP address used by this PC.
        /// </summary>
        /// <returns>True if success, False if failed.</returns>
        private bool ShowExternalIp()
        {
            bool success = true;
            try
            {
                userIp_textBox.Text = new HttpClient().GetStringAsync("https://checkip.amazonaws.com/").GetAwaiter().GetResult();
                //userIp_textBox.Text = new HttpClient().GetStringAsync("https://www.hgeay45yhshrtynfn.com/").GetAwaiter().GetResult();
            }
            catch (HttpRequestException ex)
            {
                userIp_textBox.Text = "Could not resolve.";
                success = false;
            }
            return success;
        }

        private void CopyIp_Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(userIp_textBox.Text);
        }
    }
}
