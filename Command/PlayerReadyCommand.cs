namespace ArcadeServer
{
    public sealed class PlayerReadyCommand : Command
    {
        private PlayerReadyCommand() : base(Commands.PLAYER_READY)
        {
        }

        protected override void Decode(BinaryReader reader)
        {
        }

        protected override void Encode(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        protected override void Process(Client client)
        {
            client.Player.ready = true;

            if(client.Player.currentGame.Players.All(player => player.ready))
            {
                int playerNumber = 0;
                client.Player.currentGame.Players.ForEach(player =>
                {
                    player.client.SendCommand(new GameStartCommand(playerNumber, client.Player.currentGame.Players.Excluding(client.Player).ToList()));
                });
            }
        }
    }
}
