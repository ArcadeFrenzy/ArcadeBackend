using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ArcadeServer
{
    public class ArcadeServer
    {
        private static TcpListener listener;

        private static object clientLock = new object();
        private static List<Client> clients = new List<Client>();
        public static Dictionary<int, Player> players = new Dictionary<int, Player>();

        static void Main(string[] args)
        {
            Commands.RegisterAllCommands();
            Console.WriteLine("Arcade Frenzy Server starting...");
            
            listener = new TcpListener(IPAddress.Any, 8000);

            listener.Start();

            Console.WriteLine("Server started.");

            new Thread(KeepAliveThread).Start();

            while(true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();

                Client client = new Client(tcpClient);
                
                lock (clientLock)
                {
                    clients.Add(client);
                }

                new Thread(() =>
                {
                    client.ProcessLoop();
                }).Start();
            }
        }

        private static void KeepAliveThread()
        {
            while (true)
            {
                Thread.Sleep(1000);
                List<int> inactiveClients = new List<int>();

                lock (clientLock)
                {
                    foreach (var client in clients)
                    {
                        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - client.lastKeepAliveTime >= 30000)
                        {
                            inactiveClients.Add(client.playerId);
                            Console.WriteLine($"{client.playerId} is inactive.");
                        }
                    }

                    foreach (var client in clients)
                    {
                        foreach (var inactiveClient in inactiveClients)
                        {
                            if (client.playerId == inactiveClient)
                            {
                                players.Remove(client.playerId);
                                break;
                            }

                            client.SendCommand(new PlayerLeftCommand(inactiveClient));
                        }
                    }

                    clients.RemoveAll(client => inactiveClients.Contains(client.playerId));
                }
            }
        }
    }
}