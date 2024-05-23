namespace ArcadeServer
{
    public static class Utils
    {
        public static IEnumerable<T> Excluding<T>(this IEnumerable<T> enumerable, T item) where T : class
        {
            return enumerable.Where(element => element != item);
        }
    }
}
