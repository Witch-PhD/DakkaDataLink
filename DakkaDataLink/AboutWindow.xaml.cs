using System.Windows;

namespace DakkaDataLink
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Version_TextBlock.Text = DdlConstants.VERSION_STRING;
        }

    }
}
