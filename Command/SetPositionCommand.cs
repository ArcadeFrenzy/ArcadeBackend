using System.Numerics;

namespace ArcadeServer
{
    public sealed class SetPositionCommand : Command
    {
        private int playerId;
        private Vector3 location;

        private SetPositionCommand() : base(Commands.PLAYER_LOBBY_MOVE)
        {
        }

        public SetPositionCommand(int playerId, Vector3 location) : base(Commands.PLAYER_LOBBY_MOVE)
        {
            this.playerId = playerId;
            this.location = location;
        }

        protected override void Decode(BinaryReader reader)
        {
            this.playerId = reader.ReadInt32();
            this.location.X = reader.ReadSingle();
            this.location.Y = reader.ReadSingle();
            this.location.Z = reader.ReadSingle();
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write(this.playerId);
            writer.Write(this.location.X);
            writer.Write(this.location.Y);
            writer.Write(this.location.Z);
        }

        protected override void Process(Client client)
        {
            ArcadeServer.players[client.playerId].location = this.location;

            foreach(var player in ArcadeServer.players.Values)
            {
                if(player.playerId == client.playerId)
                {
                    continue;
                }

                player.client.SendCommand(this);
            }
        }
    }
}
