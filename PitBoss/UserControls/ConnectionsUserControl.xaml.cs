﻿using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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

            GetExternalIp();

#if DEBUG
            serverIp_TextBox.Text = "127.0.0.1";
#endif

        }
        private DataManager dataManager;

        private void ConnectToServer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.ClientHandlerActive)
            {
                DataManager.Instance.StopClient();
                ConnectToServer_Button.Content = "Connect to Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                SpotterKeystrokeHandler.Instance.Deactivate();
                StartServer_Button.IsEnabled = true;
                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;
            }
            else
            {
                serverIp_TextBox.Text = serverIp_TextBox.Text.Trim();
                bool validIP = IPEndPoint.TryParse(serverIp_TextBox.Text + $":{PitBossConstants.SERVER_PORT}", out _);
                if (validIP)
                {
                    DataManager.Instance.StartClient(serverIp_TextBox.Text + $":{PitBossConstants.SERVER_PORT}");
                    ConnectToServer_Button.Content = "Disconnect";

                    StartServer_Button.IsEnabled = false;
                    serverIp_TextBox.IsEnabled = false;

                    setOperatingModes();
                }
                else
                {
                    Console.WriteLine("*** Invalid server IP address. Check your values.");
                }
            }
        }

        private void StartServer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.ServerHandlerActive)
            {
                DataManager.Instance.StopServer();

                StartServer_Button.Content = "Start as Server";

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                mainWindow.SetOperatingMode(DataManager.ProgramOperatingMode.eIdle);

                SpotterKeystrokeHandler.Instance.Deactivate();
                ConnectToServer_Button.IsEnabled = true;
                serverIp_TextBox.IsEnabled = true;
                gunnerMode_RadioButton.IsEnabled = true;
                spotterMode_RadioButton.IsEnabled = true;

                userIp_stackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                DataManager.Instance.StartServer();

                StartServer_Button.Content = "Stop Server";

                ConnectToServer_Button.IsEnabled = false;
                serverIp_TextBox.IsEnabled = false;

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
                SpotterKeystrokeHandler.Instance.Activate();
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