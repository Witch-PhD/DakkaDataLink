using DakkaDataLink;

namespace DDL_DedicatedServer_Linux
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
            DataManager dataManager = DataManager.Instance;
            GlobalLogger logger = GlobalLogger.Instance;
            dataManager.StartUdpServer();
            Console.WriteLine("Type Exit to quit.");
            string? KillString = "";
            while (KillString != "Exit")
            {
                KillString = Console.ReadLine();
            }
            dataManager.StopUdp();
        }
    }
}
