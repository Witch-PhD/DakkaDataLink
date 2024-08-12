using System.Configuration;
using System.Data;
using System.Windows;

namespace PitBoss
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow mainWindow;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            
        }

    }

}
