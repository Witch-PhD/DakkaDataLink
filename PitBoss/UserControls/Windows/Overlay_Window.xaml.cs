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
    /// Interaction logic for Overlay_Window.xaml
    /// </summary>
    public partial class Overlay_Window : Window
    {
        DataManager dataManager;
        public Overlay_Window()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            //DataContext = DataManager.Instance;
            AzLabel_TextBlock.DataContext = dataManager.userOptions;
            DistLabel_TextBlock.DataContext = dataManager.userOptions;

            AzValue_TextBlock.DataContext = dataManager;
            DistValue_TextBlock.DataContext = dataManager;

            Topmost = true;
            
        }

        internal void FlashOverlay()
        {
            Task flashTask = Task.Run(() =>
            {
                SolidColorBrush originalColor = BackGroundBrush;
                BackGroundBrush = Brushes.DarkRed;
                Thread.Sleep(500);
                BackGroundBrush = originalColor;
            });
        }

        private void MainBorder_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
