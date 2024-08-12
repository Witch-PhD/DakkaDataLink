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
using System.Windows.Shapes;

namespace PitBoss.UserControls
{
    /// <summary>
    /// Interaction logic for GunnerOverlay_Window.xaml
    /// </summary>
    public partial class GunnerOverlay_Window : Window
    {
        public GunnerOverlay_Window()
        {
            InitializeComponent();
            DataContext = DataManager.Instance;
            
            Topmost = true;
        }

  

        private void MainBorder_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
