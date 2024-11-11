using DakkaDataLink;

namespace DDL_DedicatedServer_Linux
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            DataManager dataManager = DataManager.Instance;
            GlobalLogger logger = GlobalLogger.Instance;
            GlobalLogger.Log($"Dakka Data Link v{DdlConstants.VERSION_STRING} Dedicated Server starting.");
            dataManager.StartUdpServer();
            Console.WriteLine("Type Exit to quit.");
            string? KillString = "";
            while (KillString != "Exit")
            {
                KillString = Console.ReadLine();
            }
            dataManager.StopUdp();
            GlobalLogger.Shutdown();
        }
    }
}
