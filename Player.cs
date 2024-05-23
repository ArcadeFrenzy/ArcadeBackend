using System.Numerics;

namespace ArcadeServer
{
    public sealed class Player
    {
        public Client client;

        public int playerId;
        public string playerName;

        public Vector3 location = Vector3.Zero;
        public bool alive = false;

        public Game currentGame;
        public bool ready = false;

        public Player(Client client, int playerId, string playerName)
        {
            this.client = client;
            this.playerId = playerId;
            this.playerName = playerName;
        }
    }
}
