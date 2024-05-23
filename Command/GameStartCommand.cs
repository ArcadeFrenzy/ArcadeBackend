namespace ArcadeServer
{
    public sealed class GameStartCommand : Command
    {
        private int playerNumber;
        private List<Player> players;

        public GameStartCommand(int playerNumber, List<Player> players) : base(Commands.GAME_START)
        {
            this.playerNumber = playerNumber;
            this.players = players;
        }

        protected override void Decode(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write(this.playerNumber);

            writer.Write(players.Count);
            foreach(var player in players)
            {
                writer.Write(player.playerId);
                writer.Write(player.playerName);

                writer.Write(0.0f);
                writer.Write(0.0f);
                writer.Write(0.0f);
            }
        }

        protected override void Process(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
