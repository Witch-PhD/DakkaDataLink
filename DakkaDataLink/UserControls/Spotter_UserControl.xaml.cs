using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DakkaDataLink.UserControls
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
            DataContext = DataManager.Instance;
            //Az_TextBox.DataContext = DataManager.Instance;
            //Dist_TextBox.DataContext = DataManager.Instance;
            PreviousCoords_DataGrid.ItemsSource = dataManager.PreviousCoords;
            SavedCoords_DataGrid.ItemsSource = dataManager.SavedCoords;
        }
        DataManager dataManager;

        

        private void SendCoords_Button_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Instance.SendCoords();
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

        private void SaveCoords_ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FiringHistoryEntry? theEntry = PreviousCoords_DataGrid.SelectedItem as FiringHistoryEntry;
            if (theEntry != null)
            {
                dataManager.SavedCoords.Add(theEntry);
            }
        }

        private void SetCoords_ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FiringHistoryEntry? theEntry = SavedCoords_DataGrid.SelectedItem as FiringHistoryEntry;
            if (theEntry != null)
            {
                dataManager.LatestAz = theEntry.Az;
                dataManager.LatestDist = theEntry.Dist;
            }
        }

        private void DeleteSavedCoords_ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FiringHistoryEntry? theEntry = SavedCoords_DataGrid.SelectedItem as FiringHistoryEntry;
            if (theEntry != null)
            {
                dataManager.SavedCoords.Remove(theEntry);
            }
        }
    }
}
