using System;
using System.Collections.Generic;
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
            InitializeComponent();
            DataContext = DataManager.Instance;
        }

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataManager.Instance.ConnectionStatus == Constants.ConnectionStatus.Connected_As_Gunner)
            {
                DataManager.Instance.StopClient();
                Connect_Button.Content = "Connect";
            }
            else
            {
                bool validIP = IPEndPoint.TryParse(spotterIp_TextBox.Text, out _);
                if (validIP)
                {
                    DataManager.Instance.StartClient(spotterIp_TextBox.Text);
                    Connect_Button.Content = "Disconnect";
                }
                else
                {
                    Console.WriteLine("*** Invalid spotter IP address. Check your values.");
                }
            }
        }



        GunnerOverlay_Window? overlayWindow;
        private void ToggleOverlay_Button_Click(object sender, RoutedEventArgs e)
        {
            if (overlayWindow == null)
            {

                overlayWindow = new GunnerOverlay_Window();
                overlayWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                overlayWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
                OverlayTransparency_Slider.IsEnabled = true;
                OverlayTransparency_Slider.DataContext = overlayWindow.BackGroundBrush;
                OverlayTransparency_TextBox.DataContext = overlayWindow.BackGroundBrush;
                overlayWindow.Closing += (o, ev) =>
                {
                    overlayWindow = null;
                };

                overlayWindow.Show();
                overlayWindow.BackGroundBrush.Opacity = 0.1;
            }
            else
            {
                OverlayTransparency_Slider.DataContext = null;
                OverlayTransparency_TextBox.DataContext = null;
                OverlayTransparency_TextBox.Text = "N/A";
                OverlayTransparency_Slider.IsEnabled = false;
                overlayWindow.Close();
            }
        }
    }
}
