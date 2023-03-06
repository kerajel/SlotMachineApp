namespace SlotMachineApp.Helpers
{
    internal static class IEnumerableExtensions
    {
        private static readonly Random _random = new();

        public static IEnumerable<T> OrderByRandom<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(_ => _random.Next());
        }
    }
}
