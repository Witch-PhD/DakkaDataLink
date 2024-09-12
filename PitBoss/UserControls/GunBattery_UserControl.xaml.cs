using System.Windows.Controls;

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
