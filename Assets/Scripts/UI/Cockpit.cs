using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class Cockpit : MonoBehaviour
    {
        public TextMeshProUGUI textSpeed;
        public Image bar;
        public Color low;
        public Color high;

        void OnVelocityChange(float[] velocities)
        {
            if (textSpeed != null)
            {
                textSpeed.text = (int)velocities[0] + "";
            }

            bar.transform.localScale = new Vector3(velocities[0] / velocities[1], 1, 1);
            bar.color = Color.Lerp(low, high, velocities[0] / velocities[1]);
        }
    }
}