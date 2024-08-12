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
            InitializeComponent();
        }

        private void OpenConnection_Button_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Instance.StartServer();
        }

        private void SendCoords_Button_Click(object sender, RoutedEventArgs e)
        {
            DataManager.Instance.SendNewCoords(45.0, 155.0);
        }
    }
}
