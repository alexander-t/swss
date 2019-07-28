using UnityEngine;
using TMPro;

namespace UI
{
    public class MissionLost : MonoBehaviour
    {
        public TextMeshProUGUI textLosingReason;
        public TextMeshProUGUI textMissionTime;

        void Start()
        {
            textLosingReason.text = MissionEndData.losingReason;
            textMissionTime.text = "Mission time: " + MissionEndData.FormattedMissionTime;
        }
    }
}