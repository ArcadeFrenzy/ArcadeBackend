namespace ArcadeServer
{
    public sealed class PlayerJoinedCommand : Command
    {
        private readonly Player player;

        public PlayerJoinedCommand(Player player) : base(Commands.PLAYER_JOIN)
        {
            this.player = player;
        }

        protected override void Decode(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write(this.player.playerId);
            writer.Write(this.player.playerName);

            writer.Write(this.player.location.X);
            writer.Write(this.player.location.Y);
            writer.Write(this.player.location.Z);
        }

        protected override void Process(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
