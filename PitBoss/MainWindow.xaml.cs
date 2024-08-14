using PitBoss.UserControls;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PitBoss.Constants;

namespace PitBoss
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
#if DEBUG
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32", SetLastError = true)]
        public static extern void FreeConsole();
#endif
        public MainWindow()
        {
#if DEBUG
            AllocConsole();
#endif
            InitializeComponent();
            this.Title = "The Pit Boss";
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            theSpotterUserControl.CloseAllWindows();
            theGunnerUserControl.CloseAllWindows();
        }
    }
}