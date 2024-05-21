namespace ArcadeServer
{
    public sealed class PlayerListCommand : Command
    {
        private List<Player> players = new List<Player>();

        public PlayerListCommand(List<Player> players) : base(Commands.PLAYER_LIST)
        {
            this.players = players;
        }

        protected override void Decode(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write(players.Count);

            foreach(var player in players)
            {
                writer.Write(player.playerId);
                writer.Write(player.playerName);

                writer.Write(player.location.X);
                writer.Write(player.location.Y);
                writer.Write(player.location.Z);
            }
        }

        protected override void Process(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
