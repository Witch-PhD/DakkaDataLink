using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class UserOptions_UserControl : UserControl, INotifyPropertyChanged
    {
        public UserOptions_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();

            initComboBoxes();
            DataContext = dataManager;
            //dataManager.newCoordsReceived += OnNewCoordsReceived;
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

            foreach (string artyName in ArtilleryProfiles.Instance.ArtyProfilesDict.Keys)
            {
                ArtyProfile_ComboBox.Items.Add(artyName);
            }
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

        private bool m_FlashOnNewCoords = true;
        public bool FlashOnNewCoords
        {
            get { return m_FlashOnNewCoords; }
            set { m_FlashOnNewCoords = value; OnPropertyChanged(); }
        }
        private void OnNewCoordsReceived(object sender, bool args)
        {
            if (m_FlashOnNewCoords)
            {
                overlayWindow.FlashOverlay();
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

        private void ArtyProfile_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ArtilleryProfiles.Instance.CurrentProfile = ArtilleryProfiles.Instance.ArtyProfilesDict[ArtyProfile_ComboBox.SelectedValue as string];
            dataManager.LatestDist = ArtilleryProfiles.Instance.CurrentProfile.MinDist;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
