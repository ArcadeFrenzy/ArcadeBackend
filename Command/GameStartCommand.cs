namespace ArcadeServer
{
    public sealed class GameStartCommand : Command
    {
        private int playerNumber;

        public GameStartCommand(int playerNumber) : base(Commands.GAME_START)
        {
            this.playerNumber = playerNumber;
        }

        protected override void Decode(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write(this.playerNumber);
        }

        protected override void Process(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
