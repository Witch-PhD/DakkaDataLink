using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DakkaDataLink.UserControls
{
    /// <summary>
    /// Interaction logic for UserOptions_UserControl.xaml
    /// </summary>
    public partial class UserOptions_UserControl : UserControl, INotifyPropertyChanged
    {
        private double opacity = 0.1;
        private Uri checkedDictionaryUri;

        public UserOptions_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            DataContext = dataManager;
            initComboBoxes();
            updateKeyBindingStrings();
            dataManager.newCoordsReceived += OnNewCoordsReceived;
        }
        DataManager dataManager;

        private void initComboBoxes()
        {
            
            foreach (string artyName in ArtilleryProfiles.Instance.ArtyProfilesDict.Keys)
            {
                ArtyProfile_ComboBox.Items.Add(artyName);
            }
            ArtyProfile_ComboBox.SelectedItem = ArtyProfile_ComboBox.Items[0];
            LabelColor_ComboBox.SelectedValue = nameof(dataManager.userOptions.OverlayLabelsFontColor);
        }

        private void initColors()
        {
            string[] colors ={
                nameof(Brushes.Red), nameof(Brushes.Orange), nameof(Brushes.Yellow), nameof(Brushes.Green),
                nameof(Brushes.Blue), nameof(Brushes.Purple), nameof(Brushes.Magenta), nameof(Brushes.Pink),
                nameof(Brushes.Black), nameof(Brushes.White), nameof(Brushes.Gray), nameof(Brushes.DarkGray)
                };

            LabelColor_ComboBox.Items.Clear();
            ValueColor_ComboBox.Items.Clear();
            OverlayBackgroundColor_ComboBox.Items.Clear();
            OverlayAlertColor_ComboBox.Items.Clear();
            foreach (string color in colors)
            {
                var labelColor = new ComboBoxItem
                {   
                    Content = Application.Current.Resources["comboboxitem_color_" + color.ToLower()] as string,
                    Tag = color
                };
                LabelColor_ComboBox.Items.Add(labelColor);
                var valueColor = new ComboBoxItem
                {
                    Content = Application.Current.Resources["comboboxitem_color_" + color.ToLower()] as string,
                    Tag = color
                };
                ValueColor_ComboBox.Items.Add(valueColor);
                var bgColor = new ComboBoxItem
                {
                    Content = Application.Current.Resources["comboboxitem_color_" + color.ToLower()] as string,
                    Tag = color
                };
                OverlayBackgroundColor_ComboBox.Items.Add(bgColor);
                var alertColor = new ComboBoxItem
                {
                    Content = Application.Current.Resources["comboboxitem_color_" + color.ToLower()] as string,
                    Tag = color
                };
                OverlayAlertColor_ComboBox.Items.Add(alertColor);
            }
        }

        public void CloseAllWindows()
        {
            if (overlayWindow != null)
            {
                dataManager.userOptions.OverlayOpacity = OverlayOpacity_Slider.Value;
            }
            overlayWindow?.Close();
        }

        Overlay_Window? overlayWindow;
        private void ToggleOverlay_Button_Click(object sender, RoutedEventArgs e)
        {
            if (overlayWindow == null) // Opening Window
            {
                initColors();

                overlayWindow = new Overlay_Window();
                overlayWindow.BackGroundBrush.Color = dataManager.userOptions.OverlayBackgroundColor;
                overlayWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                overlayWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
                OverlayOpacity_Slider.IsEnabled = true;
                LabelColor_ComboBox.IsEnabled = true;
                ValueColor_ComboBox.IsEnabled = true;
                OverlayBackgroundColor_ComboBox.IsEnabled = true;
                OverlayAlertColor_ComboBox.IsEnabled = true;
                FontSize_TextBox.IsEnabled = true;
                OverlayOpacity_Slider.DataContext = overlayWindow.BackGroundBrush;
                OverlayOpacity_TextBox.DataContext = overlayWindow.BackGroundBrush;
                UseFlashAlert_CheckBox.IsEnabled = true;
                UseFlashAlert_CheckBox.DataContext = overlayWindow;
                overlayWindow.Closing += (o, ev) =>
                {
                    overlayWindow = null;
                };

                overlayWindow.Show();
                overlayWindow.BackGroundBrush.Opacity = dataManager.userOptions.OverlayOpacity;
            }
            else // Closing Window
            {
                dataManager.userOptions.OverlayOpacity = OverlayOpacity_Slider.Value;
                //opacity = OverlayOpacity_Slider.Value;
                OverlayOpacity_Slider.DataContext = null;
                OverlayOpacity_TextBox.DataContext = null;
                OverlayOpacity_TextBox.Text = "N/A";
                OverlayOpacity_Slider.IsEnabled = false;
                LabelColor_ComboBox.IsEnabled = false;
                ValueColor_ComboBox.IsEnabled = false;
                OverlayBackgroundColor_ComboBox.IsEnabled = false;
                OverlayAlertColor_ComboBox.IsEnabled = false;
                FontSize_TextBox.IsEnabled = false;
                UseFlashAlert_CheckBox.IsEnabled = false;
                UseFlashAlert_CheckBox.DataContext = null;
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
                overlayWindow?.FlashOverlay();
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

        public void updateKeyBindingStrings()
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
                Shortcuts_Button.Content = Application.Current.Resources["button_show"] as string;
            }
            else
            {
                ShorcutsMenu.Visibility = Visibility.Visible;
                Shortcuts_Button.Content = Application.Current.Resources["button_hide"] as string;
            }
        }

        private bool DidLanugageChanged()
        {
            var dictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault();

            if (dictionary != null && dictionary.Source != null)
            {
                if (checkedDictionaryUri != null && checkedDictionaryUri.Equals(dictionary.Source))
                {
                    //Console.WriteLine("Check");
                    return false;
                }

                checkedDictionaryUri = dictionary.Source;
                return true;
            }

            checkedDictionaryUri = null;
            return true;
        }

        private void OverlayBackgroundColor_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (overlayWindow != null)
            {
                ComboBoxItem thingy = OverlayBackgroundColor_ComboBox.SelectedItem as ComboBoxItem;
                overlayWindow.BackGroundBrush.Color = (Color)ColorConverter.ConvertFromString(thingy.Content as string);
            }
        }

        private void OverlayAlertColor_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            overlayWindow?.FlashOverlay();
        }
    }
}
