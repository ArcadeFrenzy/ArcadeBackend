namespace ArcadeServer
{
    public sealed class GameQueueCommand : Command
    {
        enum GameQueueStatus : int
        {
            Request = 0,
            Response = 1,
            PlayerQueued = 2,
            PlayerDequeued = 3,
            QueueReady = 4
        }

        private GameQueueStatus status;
        private string gameName;

        private string sceneName;
        private int queuePosition;

        private GameQueueCommand() : base(Commands.LOBBY_QUEUE)
        {
        }

        public GameQueueCommand(string sceneName) : this()
        {
            this.status = GameQueueStatus.QueueReady;
            this.sceneName = sceneName;
        }

        protected override void Decode(BinaryReader reader)
        {
            this.status = (GameQueueStatus)reader.ReadInt32();

            switch (this.status)
            {
                case GameQueueStatus.Request:
                    {
                        this.gameName = reader.ReadString();
                    } break;
                default:
                    {
                        Console.WriteLine($"Invalid status {this.status} for GameQueueCommand provided.");
                    } break;
            }
        }

        protected override void Encode(BinaryWriter writer)
        {
            writer.Write((int)this.status);

            switch (this.status)
            {
                case GameQueueStatus.Response:
                    {
                        writer.Write(this.queuePosition);
                    } break;
                case GameQueueStatus.QueueReady:
                    {
                        writer.Write(this.sceneName);
                    } break;
            }
        }

        protected override void Process(Client client)
        {
            int queuePosition = QueueManager.Instance.QueuePlayer(client.Player, Games.GetGame(this.gameName));

            client.SendCommand(new GameQueueCommand()
            {
                status = GameQueueStatus.Response,
                queuePosition = queuePosition
            });
        }
    }
}
