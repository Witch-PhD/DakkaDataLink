using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace PitBoss.UserControls
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

            GetExternalIp(); // TODO: Probably want to move this to the server start up function.
            MyCallsign_Textbox.DataContext = dataManager;
            ActiveUsers_DataGrid.ItemsSource = dataManager.ConnectedUsersCallsigns;

#if DEBUG
            serverIp_TextBox.Text = "127.0.0.1";
#endif

        }
        private DataManager dataManager;

        private void StartStopUdpClient_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.UdpHandlerActive) // Stopping
            {
                DataManager.Instance.StopUdp();
                StartStopUdpClient_Button.Content = "Connect to Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;
                StartStopUdpServer_Button.IsEnabled = true;
                MyCallsign_Textbox.IsEnabled = true;
                userIp_stackPanel.Visibility = Visibility.Hidden;
            }
            else // Starting
            {
                setOperatingModes();
                serverIp_TextBox.Text = serverIp_TextBox.Text.Trim();
                bool validIP = IPAddress.TryParse(serverIp_TextBox.Text, out _);
                if (validIP)
                {
                    DataManager.Instance.StartUdpClient(serverIp_TextBox.Text);
                    StartStopUdpClient_Button.Content = "Disconnect";

                    StartStopUdpServer_Button.IsEnabled = false;
                    serverIp_TextBox.IsEnabled = false;
                    MyCallsign_Textbox.IsEnabled = false;
                    userIp_stackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    //Console.WriteLine("*** Invalid server IP address. Check your values.");
                }
                
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
                userIp_stackPanel.Visibility = Visibility.Hidden;
            }
            else // Starting
            {
                setOperatingModes();
                DataManager.Instance.StartUdpServer();
                StartStopUdpServer_Button.Content = "Disconnect";

                userIp_stackPanel.Visibility = Visibility.Visible;
                StartStopUdpClient_Button.IsEnabled = false;
                serverIp_TextBox.IsEnabled = false;
                MyCallsign_Textbox.IsEnabled = false;
                
            }
            
        }

        private void ConnectToGrpcServer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.GrpcClientHandlerActive)
            {
                DataManager.Instance.StopGrpcClient();
                ConnectToGrpcServer_Button.Content = "Connect to Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                StartStopGrpcServer_Button.IsEnabled = true;
                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;
                MyCallsign_Textbox.IsEnabled = true;
            }
            else
            {
                serverIp_TextBox.Text = serverIp_TextBox.Text.Trim();
                bool validIP = IPEndPoint.TryParse(serverIp_TextBox.Text + $":{PitBossConstants.SERVER_PORT}", out _);
                if (validIP)
                {
                    DataManager.Instance.StartGrpcClient(serverIp_TextBox.Text + $":{PitBossConstants.SERVER_PORT}");
                    ConnectToGrpcServer_Button.Content = "Disconnect";

                    StartStopGrpcServer_Button.IsEnabled = false;
                    serverIp_TextBox.IsEnabled = false;
                    MyCallsign_Textbox.IsEnabled = false;

                    setOperatingModes();
                }
                else
                {
                    //Console.WriteLine("*** Invalid server IP address. Check your values.");
                }
            }
        }

        private void StartStopGrpcServer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.GrpcServerHandlerActive)
            {
                DataManager.Instance.StopGrpcServer();

                StartStopGrpcServer_Button.Content = "Start as Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                ConnectToGrpcServer_Button.IsEnabled = true;
                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;
                MyCallsign_Textbox.IsEnabled = true;

                userIp_stackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                DataManager.Instance.StartGrpcServer();

                StartStopGrpcServer_Button.Content = "Stop Server";

                ConnectToGrpcServer_Button.IsEnabled = false;
                serverIp_TextBox.IsEnabled = false;
                MyCallsign_Textbox.IsEnabled = false;

                userIp_stackPanel.Visibility = Visibility.Visible;

                setOperatingModes();
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

        private void GetExternalIp()
        {
            userIp_textBox.Text = new HttpClient().GetStringAsync("https://checkip.amazonaws.com/").GetAwaiter().GetResult();
        }

        private void CopyIp_Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(userIp_textBox.Text);
        }
    }
}
