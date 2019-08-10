using UnityEngine;
using TMPro;

namespace UI
{
    public class MissionEnded : MonoBehaviour
    {
        public TextMeshProUGUI textLosingReason;
        public TextMeshProUGUI textMissionTime;

        void Start()
        {
            if (textLosingReason != null)
            {
                textLosingReason.text = MissionEndData.losingReason;
            }
            textMissionTime.text = "Mission time: " + MissionEndData.FormattedMissionTime;
        }
    }
}