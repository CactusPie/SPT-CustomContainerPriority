using Comfort.Common;
using EFT;

namespace CactusPie.CustomContainerPriority.Helpers
{
    public static class GameHelper
    {
        public static bool IsInGame()
        {
            return Singleton<GameWorld>.Instance != null;
        }
    }
}