namespace ArcadeServer
{
    public sealed class PlayerLeftCommand : Command
    {
        private readonly int playerId;

        public PlayerLeftCommand(int playerId) : base(Commands.PLAYER_LEAVE)
        {
            this.playerId = playerId;
        }

        protected override void Decode(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write(this.playerId);
        }

        protected override void Process(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
