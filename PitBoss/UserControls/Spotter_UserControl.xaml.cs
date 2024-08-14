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
            DataContext = DataManager.Instance;
            //Az_TextBox.DataContext = DataManager.Instance;
            //Dist_TextBox.DataContext = DataManager.Instance;

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
