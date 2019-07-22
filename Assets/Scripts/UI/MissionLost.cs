using UnityEngine;
using TMPro;

namespace UI
{
    public class MissionLost : MonoBehaviour
    {
        public TextMeshProUGUI textLosingReason;

        void Start()
        {
            textLosingReason.text = MissionEndData.losingReason;
        }
    }
}