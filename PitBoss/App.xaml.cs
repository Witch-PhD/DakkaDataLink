using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace PitBoss
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindow mainWindow;

        public static KeyboardListener KListener = new KeyboardListener();
        private static SpotterKeystrokeHandler keystrokeHandler = SpotterKeystrokeHandler.Instance;
        DataManager dataManager = DataManager.Instance;
        private static RawKeyEventHandler keyDownHandler;
        private static RawKeyEventHandler keyUpHandler;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            keyDownHandler = new RawKeyEventHandler(KListener_KeyDown);
            keyUpHandler = new RawKeyEventHandler(KListener_KeyUp);

            KListener.KeyDown += keyDownHandler;
            KListener.KeyUp += keyUpHandler;
            
            mainWindow = new MainWindow();
            mainWindow.Show();
        }


        static void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            SpotterKeystrokeHandler.Instance.handleKeyDown(args);
            
            // I tried writing the data in file here also, to make sure the problem is not in Console.WriteLine
        }

        static void KListener_KeyUp(object sender, RawKeyEventArgs args)
        {
            //if (!args.IsSysKey) // We only care about ctrl or shift.
            //{
            //    return;
            //}
            SpotterKeystrokeHandler.Instance.handleKeyUp(args);
            //Console.WriteLine(args.Key.ToString());
            // I tried writing the data in file here also, to make sure the problem is not in Console.WriteLine
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            KListener.Dispose();
        }

    }

}
