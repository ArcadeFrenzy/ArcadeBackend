namespace ArcadeServer
{
    public sealed class ClientHelloCommand : Command
    {
        private static int NEXT_PLAYER_ID = 0;
        private string name;

        private ClientHelloCommand() : base(Commands.CLIENT_HELLO)
        {
        }

        protected override void Decode(BinaryReader reader)
        {
            this.name = reader.ReadString();
        }

        protected override void Encode(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override void Process(Client client)
        {
            Player player = new Player(client, NEXT_PLAYER_ID++, name);
            ArcadeServer.players.Add(player.playerId, player);

            client.SendCommand(new ServerHelloCommand(player.playerId));
            client.playerId = player.playerId;

            var players = ArcadeServer.players.Values.Where(player => player.playerId != client.playerId).ToList();
            client.SendCommand(new PlayerListCommand(players));

            foreach(var otherPlayer in players)
            {
                otherPlayer.client.SendCommand(new PlayerJoinedCommand(player));
            }
        }
    }
}
