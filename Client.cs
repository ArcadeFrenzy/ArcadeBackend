using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeServer
{
    public class Client
    {
        public int playerId;
        public long lastKeepAliveTime;

        public Player Player
        {
            get
            {
                return ArcadeServer.players[playerId];
            }
        }

        private readonly TcpClient client;
        private readonly BinaryWriter writer;
        private readonly BinaryReader reader;

        public Client(TcpClient client)
        {
            this.client = client;
            this.writer = new BinaryWriter(client.GetStream());
            this.reader = new BinaryReader(client.GetStream());
            this.lastKeepAliveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void SendCommand(Command command)
        {
            try
            {
                command.EncodeCommand(this.writer);
                this.writer.Flush();

                Console.WriteLine($"Sent server command {command.GetType().Name}.");
            }
            catch(IOException)
            {
                // Error on sending command, do nothing as player may have disconnected.
            }
        }

        public void ProcessLoop()
        {
            while(this.client.Connected)
            {
                Command.DecodeCommand(this, this.reader);
            }
        }
    }
}
