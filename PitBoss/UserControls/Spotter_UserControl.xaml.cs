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
        
        public Spotter_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            Az_TextBox.DataContext = DataManager.Instance;
            Dist_TextBox.DataContext = DataManager.Instance;

        }
        DataManager dataManager;

        private void OpenConnection_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataManager.ServerHandlerActive)
            {
                DataManager.Instance.StopServer();
                SpotterKeystrokeHandler.Instance.Deactivate();
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
            DataManager.Instance.SendCoords();
        }

        public void CloseAllWindows()
        {
            overlayWindow?.Close();
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
