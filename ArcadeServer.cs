using System.Net;
using System.Net.Sockets;

namespace ArcadeServer
{
    public class ArcadeServer
    {
        private static TcpListener listener;
        public static Dictionary<int, Player> players = new Dictionary<int, Player>();

        static void Main(string[] args)
        {
            Commands.RegisterAllCommands();
            Console.WriteLine("Arcade Frenzy Server starting...");
            
            listener = new TcpListener(IPAddress.Any, 8000);

            listener.Start();

            Console.WriteLine("Server started.");

            while(true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();

                Client client = new Client(tcpClient);

                new Thread(() =>
                {
                    client.ProcessLoop();
                }).Start();
            }
        }
    }
}