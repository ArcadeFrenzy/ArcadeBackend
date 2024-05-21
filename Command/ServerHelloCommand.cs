namespace ArcadeServer
{
    public sealed class ServerHelloCommand : Command
    {
        private int playerId;

        public ServerHelloCommand(int playerId) : base(Commands.SERVER_HELLO)
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
