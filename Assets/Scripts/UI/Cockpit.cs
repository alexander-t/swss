using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;

namespace UI
{
    public class Cockpit : MonoBehaviour
    {
        public TextMeshProUGUI textSpeed;
        public Image speedIndicator;
        public Color low;
        public Color high;
        [Space(7)]
        public Image healthIndicatorImage;
        public Sprite maxHealthSprite;
        public Sprite criticalHealthSprite;

        void Start()
        {
            EventManager.onShipHit += OnShipHit;
            healthIndicatorImage.sprite = maxHealthSprite;    
        }

        void OnDestroy()
        {
            EventManager.onShipHit -= OnShipHit;    
        }

        void OnVelocityChange(float[] velocities)
        {
            if (textSpeed != null)
            {
                textSpeed.text = (int)velocities[0] + "";
            }

            speedIndicator.transform.localScale = new Vector3(velocities[0] / velocities[1], 1, 1);
            speedIndicator.color = Color.Lerp(low, high, velocities[0] / velocities[1]);
        }

        void OnShipHit(string name)
        {
            // Quite an over simplification: since the tie fighter only can take one hit we can swap image here
            if (GameObjects.IsPlayer(name))
            {
                healthIndicatorImage.sprite = criticalHealthSprite;
            }
        }
    }
}