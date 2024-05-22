namespace ArcadeServer
{
    public abstract class Game
    {
        private readonly string gameName;
        private readonly string sceneName;
        private readonly int playerCount;

        public string GameName
        {
            get
            {
                return this.gameName;
            }
        }

        public string SceneName
        {
            get
            {
                return this.sceneName;
            }
        }

        public int PlayerCount
        {
            get
            {
                return this.playerCount;
            }
        }

        public Game(string gameName, string sceneName, int playerCount)
        {
            this.gameName = gameName;
            this.sceneName = sceneName;
            this.playerCount = playerCount;
        }
    }
}
