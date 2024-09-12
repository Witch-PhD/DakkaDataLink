using System.Windows;

namespace PitBoss
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Version_TextBlock.Text = PitBossConstants.VERSION_STRING;
        }

    }
}
