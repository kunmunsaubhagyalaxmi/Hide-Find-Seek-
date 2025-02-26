using UnityEngine;

namespace Game
{
    public static class GameConstants
    {
        public static string CoinIcon = "<sprite=\"CoinIcon\" index=0>";
        public static string AdsIcon = "<sprite=\"AdsIcon\" index=0>";
        public static string ClockIcon = "<sprite=\"ClockIcon\" index=0>";

        public const string UnitLayer = "Unit";
        public const string DefaultLayer = "Default";
        public const string SeekerLabel = "SEEKER";

        public static bool IsDebugBuild()
        {
            return Debug.isDebugBuild;
        }

        public static bool IsDebugIPad()
        {
            bool isDeveloperDevice = IsDebugBuild();
            var identifier = SystemInfo.deviceModel;
            bool isiPad = identifier.StartsWith("iPad");
            return isDeveloperDevice && isiPad;
        }
    }
}