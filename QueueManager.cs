namespace ArcadeServer
{
    public sealed class QueueManager
    {
        private static QueueManager instance;

        public static QueueManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new QueueManager();
                }

                return instance;
            }
        }

        private object gameQueueLock = new object();
        private Dictionary<Game, List<Player>> gameQueues = new Dictionary<Game, List<Player>>();

        private QueueManager()
        {
        }

        public int QueuePlayer(Player player, Game game)
        {
            if(!gameQueues.ContainsKey(game)) 
            {
                gameQueues.Add(game, new List<Player>());
            }

            lock (gameQueueLock)
            {
                gameQueues[game].Add(player);

                if (gameQueues[game].Count >= game.PlayerCount)
                {
                    Game gameCopy = (Game)Activator.CreateInstance(game.GetType(), true);
                    gameQueues[game].Take(game.PlayerCount).ToList().ForEach(player =>
                    {
                        player.ready = false;
                        player.currentGame = gameCopy;
                        gameCopy.Players.Add(player);

                        player.client.SendCommand(new GameQueueCommand(game.SceneName));
                    });

                    gameQueues[game].RemoveRange(0, game.PlayerCount);
                }
            }

            return gameQueues[game].Count;
        }

        public List<Player> GetQueuedPlayers(Game game)
        {
            if(!gameQueues.ContainsKey(game))
            {
                return new List<Player>();
            }

            return gameQueues[game];
        }

        public void RemoveQueuedPlayer(Player player)
        {
            lock (gameQueueLock)
            {
                foreach (var gameQueue in gameQueues)
                {
                    if (gameQueue.Value.Contains(player))
                    {
                        gameQueue.Value.Remove(player);
                    }
                }
            }
        }
    }
}
