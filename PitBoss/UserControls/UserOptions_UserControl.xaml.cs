using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
        private double opacity = 0.1;

        public UserOptions_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            DataContext = dataManager;
            initComboBoxes();
            updateKeyBindingStrings();
            //dataManager.newCoordsReceived += OnNewCoordsReceived;
        }
        DataManager dataManager;

        private void initComboBoxes()
        {
            BrushConverter converter = new BrushConverter();

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
            ArtyProfile_ComboBox.SelectedItem = ArtyProfile_ComboBox.Items[0];
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
                LabelColor_ComboBox.IsEnabled = true;
                ValueColor_ComboBox.IsEnabled = true;
                FontSize_TextBox.IsEnabled = true;
                OverlayOpacity_Slider.DataContext = overlayWindow.BackGroundBrush;
                OverlayOpacity_TextBox.DataContext = overlayWindow.BackGroundBrush;
                overlayWindow.Closing += (o, ev) =>
                {
                    overlayWindow = null;
                };

                overlayWindow.Show();
                overlayWindow.BackGroundBrush.Opacity = opacity;
            }
            else
            {
                opacity = OverlayOpacity_Slider.Value;
                OverlayOpacity_Slider.DataContext = null;
                OverlayOpacity_TextBox.DataContext = null;
                OverlayOpacity_TextBox.Text = "N/A";
                OverlayOpacity_Slider.IsEnabled = false;
                LabelColor_ComboBox.IsEnabled = false;
                ValueColor_ComboBox.IsEnabled = false;
                FontSize_TextBox.IsEnabled = false;
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

        


        private void disableSetBindingButtons()
        {
            ChangeBindingFor_AzPlus1_Button.IsEnabled = false;
            ChangeBindingFor_AzMinus1_Button.IsEnabled = false;
            ChangeBindingFor_AzPlus10_Button.IsEnabled = false;
            ChangeBindingFor_AzMinus10_Button.IsEnabled = false;
            ChangeBindingFor_DistPlusOne_Button.IsEnabled = false;
            ChangeBindingFor_DistMinusOne_Button.IsEnabled = false;
            ChangeBindingFor_DistPlusMulti_Button.IsEnabled = false;
            ChangeBindingFor_DistMinusMulti_Button.IsEnabled = false;
            ChangeBindingFor_SendCoords_Button.IsEnabled = false;
        }

        private void enableSetBindingButtons()
        {
            ChangeBindingFor_AzPlus1_Button.IsEnabled = true;
            ChangeBindingFor_AzMinus1_Button.IsEnabled = true;
            ChangeBindingFor_AzPlus10_Button.IsEnabled = true;
            ChangeBindingFor_AzMinus10_Button.IsEnabled = true;
            ChangeBindingFor_DistPlusOne_Button.IsEnabled = true;
            ChangeBindingFor_DistMinusOne_Button.IsEnabled = true;
            ChangeBindingFor_DistPlusMulti_Button.IsEnabled = true;
            ChangeBindingFor_DistMinusMulti_Button.IsEnabled = true;
            ChangeBindingFor_SendCoords_Button.IsEnabled = true;
        }

        private void updateKeyBindingStrings()
        {
            Dictionary<string, KeyCombo> combosList = dataManager.userOptions.BindingDictionary;
            AzPlusOne_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.AZ_UP_ONE_DEG_DICT_KEY].ToString();
            AzMinusOne_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.AZ_DOWN_ONE_DEG_DICT_KEY].ToString();
            AzPlusTen_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.AZ_UP_MULTI_DEG_DICT_KEY].ToString();
            AzMinusTen_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.AZ_DOWN_MULTI_DEG_DICT_KEY].ToString();
            DistPlusOne_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.DIST_UP_ONE_TICK_DICT_KEY].ToString();
            DistMinusOne_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.DIST_DOWN_ONE_TICK_DICT_KEY].ToString();
            DistPlusMulti_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.DIST_UP_MULTI_TICK_DICT_KEY].ToString();
            DistMinusMulti_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.DIST_DOWN_MULTI_TICK_DICT_KEY].ToString();
            SendCoords_KeyBinding_TextBox.Text = dataManager.userOptions.BindingDictionary[UserOptions.SEND_ARTY_MSG_DICT_KEY].ToString();
        }

        private bool setBindingsActivated = false;
        private void ChangeBindingFor_AzPlus1_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_AzPlus1_Button.IsEnabled = true;
                ChangeBindingFor_AzPlus1_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.AZ_UP_ONE_DEG_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_AzPlus1_Button.IsEnabled = true;
                ChangeBindingFor_AzPlus1_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_AzMinus1_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_AzMinus1_Button.IsEnabled = true;
                ChangeBindingFor_AzMinus1_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.AZ_DOWN_ONE_DEG_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_AzMinus1_Button.IsEnabled = true;
                ChangeBindingFor_AzMinus1_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_AzPlus10_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_AzPlus10_Button.IsEnabled = true;
                ChangeBindingFor_AzPlus10_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.AZ_UP_MULTI_DEG_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_AzPlus10_Button.IsEnabled = true;
                ChangeBindingFor_AzPlus10_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_AzMinus10_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_AzMinus10_Button.IsEnabled = true;
                ChangeBindingFor_AzMinus10_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.AZ_DOWN_MULTI_DEG_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_AzMinus10_Button.IsEnabled = true;
                ChangeBindingFor_AzMinus10_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_DistPlusOne_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_DistPlusOne_Button.IsEnabled = true;
                ChangeBindingFor_DistPlusOne_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.DIST_UP_ONE_TICK_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_DistPlusOne_Button.IsEnabled = true;
                ChangeBindingFor_DistPlusOne_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_DistMinusOne_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_DistMinusOne_Button.IsEnabled = true;
                ChangeBindingFor_DistMinusOne_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.DIST_DOWN_ONE_TICK_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_DistMinusOne_Button.IsEnabled = true;
                ChangeBindingFor_DistMinusOne_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_DistPlusMulti_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_DistPlusMulti_Button.IsEnabled = true;
                ChangeBindingFor_DistPlusMulti_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.DIST_UP_MULTI_TICK_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_DistPlusMulti_Button.IsEnabled = true;
                ChangeBindingFor_DistPlusMulti_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_DistMinusMulti_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_DistMinusMulti_Button.IsEnabled = true;
                ChangeBindingFor_DistMinusMulti_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.DIST_DOWN_MULTI_TICK_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_DistMinusMulti_Button.IsEnabled = true;
                ChangeBindingFor_DistMinusMulti_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void ChangeBindingFor_SendCoords_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!setBindingsActivated) // Changing Binding.
            {
                setBindingsActivated = !setBindingsActivated;
                disableSetBindingButtons();
                ChangeBindingFor_SendCoords_Button.IsEnabled = true;
                ChangeBindingFor_SendCoords_Button.Content = "Finish";
                SpotterKeystrokeHandler.Instance.BeginSetKeyBinding(UserOptions.SEND_ARTY_MSG_DICT_KEY);
            }
            else // Done changing binding.
            {
                setBindingsActivated = !setBindingsActivated;
                enableSetBindingButtons();
                ChangeBindingFor_SendCoords_Button.IsEnabled = true;
                ChangeBindingFor_SendCoords_Button.Content = "Change Key(s)";
                SpotterKeystrokeHandler.Instance.EndSetKeyBinding();
                updateKeyBindingStrings();
            }
        }

        private void Shortcuts_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ShorcutsMenu.Visibility == Visibility.Visible)
            {
                ShorcutsMenu.Visibility = Visibility.Hidden;
                Shortcuts_Button.Content = "Show";
            }
            else
            {
                ShorcutsMenu.Visibility = Visibility.Visible;
                Shortcuts_Button.Content = "Hide";
            }
        }
    }
}
