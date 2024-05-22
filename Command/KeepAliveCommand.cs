namespace ArcadeServer
{
    public sealed class KeepAliveCommand : Command
    {
        private int playerId;

        private KeepAliveCommand() : base(Commands.KEEP_ALIVE)
        {
        }

        protected override void Decode(BinaryReader reader)
        {
            this.playerId = reader.ReadInt32();
        }

        protected override void Encode(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override void Process(Client client)
        {
            if(client.playerId == this.playerId)
            {
                // TODO: send keep alive OK command if player id matches.
                client.lastKeepAliveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }
    }
}
