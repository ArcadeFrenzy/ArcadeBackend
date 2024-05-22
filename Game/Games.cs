using System.Reflection;

namespace ArcadeServer
{
    public sealed class Games
    {
        private static Dictionary<string, Game> gameMap = new Dictionary<string, Game>();

        public static void RegisterAllGames()
        {
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Game))).ToList().ForEach(type =>
                {
                    ConstructorInfo constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, new Type[0]);

                    if (constructor == null)
                    {
                        return;
                    }

                    Game game = (Game)constructor.Invoke(new object[0]);

                    FieldInfo gameNameField = typeof(Game).GetField("gameName", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (gameNameField == null)
                    {
                        return;
                    }

                    string gameName = (string)gameNameField.GetValue(game);
                    gameMap.Add(gameName, game);
                });
            });
        }

        public static Game GetGame(string name)
        {
            if(!gameMap.ContainsKey(name))
            {
                return null;
            }

            return gameMap[name];
        }
    }
}
