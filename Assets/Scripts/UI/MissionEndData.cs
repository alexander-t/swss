using System;

namespace UI
{
    /*
     * Class that _only_ holds some stats between scenes when a mission is won/lost.
     */
    public class MissionEndData
    {
        public static string losingReason;
        public static float missionTime;

        public static string FormattedMissionTime {
            get => TimeSpan.FromSeconds(missionTime).ToString(@"mm\:ss");
        }
    }

}
