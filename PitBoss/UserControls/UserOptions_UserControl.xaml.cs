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
    /// Interaction logic for UserOptions_UserControl.xaml
    /// </summary>
    public partial class UserOptions_UserControl : UserControl
    {
        public UserOptions_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();

            initComboBoxes();
            DataContext = dataManager;

        }
        DataManager dataManager;

        private void initComboBoxes()
        {
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Red));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Orange));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Yellow));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Green));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Blue));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Purple));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Magenta));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Pink));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Black));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.White));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.Gray));
            LabelColor_ComboBox.Items.Add(nameof(Brushes.DarkGray));

            ValueColor_ComboBox.Items.Add(nameof(Brushes.Red));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Orange));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Yellow));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Green));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Blue));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Purple));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Magenta));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Pink));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Black));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.White));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.Gray));
            ValueColor_ComboBox.Items.Add(nameof(Brushes.DarkGray));

        }

        public void CloseAllWindows()
        {
            overlayWindow?.Close();
        }

        Overlay_Window? overlayWindow;
        private void ToggleOverlay_Button_Click(object sender, RoutedEventArgs e)
        {
            if (overlayWindow == null)
            {

                overlayWindow = new Overlay_Window();
                overlayWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                overlayWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
                OverlayOpacity_Slider.IsEnabled = true;
                OverlayOpacity_Slider.DataContext = overlayWindow.BackGroundBrush;
                OverlayOpacity_TextBox.DataContext = overlayWindow.BackGroundBrush;
                overlayWindow.Closing += (o, ev) =>
                {
                    overlayWindow = null;
                };

                overlayWindow.Show();
                overlayWindow.BackGroundBrush.Opacity = 0.1;
            }
            else
            {
                OverlayOpacity_Slider.DataContext = null;
                OverlayOpacity_TextBox.DataContext = null;
                OverlayOpacity_TextBox.Text = "N/A";
                OverlayOpacity_Slider.IsEnabled = false;
                overlayWindow.Close();
            }
        }

        private void TextBox_UpdateOnEnter(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (textBox.GetBindingExpression(TextBox.TextProperty).ValidateWithoutUpdate())
                {
                    textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }
    }
}
