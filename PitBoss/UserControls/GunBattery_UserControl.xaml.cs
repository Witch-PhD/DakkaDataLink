using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
    /// Interaction logic for GunBattery_UserControl.xaml
    /// </summary>
    public partial class GunBattery_UserControl : UserControl
    {
        public GunBattery_UserControl()
        {
            dataManager = DataManager.Instance;
            InitializeComponent();
            
            DataContext = dataManager;
            
        }
        DataManager dataManager;

        

        
    }
}
