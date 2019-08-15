using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;

namespace UI
{
    public class Cockpit : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private TextMeshProUGUI textSpeed;
        [SerializeField]
        private Image speedIndicator;
        [SerializeField]
        private Color low;
        [SerializeField]
        private Color high;
        [Space(7)]
        [SerializeField]
        private Image healthIndicatorImage;
        [SerializeField]
        private Sprite maxHealthSprite;
        [SerializeField]
        private Sprite criticalHealthSprite;
#pragma warning restore 0649

        private string shipName;

        void Awake()
        {
            shipName = GameObjects.RequirePlayerShip(gameObject).name;
        }

        void Start()
        {
            healthIndicatorImage.sprite = maxHealthSprite;

            EventManager.onShipHit += OnShipHit;
            EventManager.onPlayerSpeedChanged += OnPlayerSpeedChanged;
        }

        void OnDestroy()
        {
            EventManager.onShipHit -= OnShipHit;
            EventManager.onPlayerSpeedChanged -= OnPlayerSpeedChanged;
        }

        void OnPlayerSpeedChanged(float speed, float maxSpeed)
        {
            if (textSpeed != null)
            {
                textSpeed.text = (int)speed + "";
            }

            speedIndicator.transform.localScale = new Vector3(speed / maxSpeed, 1, 1);
            speedIndicator.color = Color.Lerp(low, high, speed /maxSpeed);
        }

        private void OnShipHit(string attacked, string attacker)
        {
            // Quite an over simplification: since the tie fighter only can take one hit we can swap image here
            if (attacked == shipName)
            {
                healthIndicatorImage.sprite = criticalHealthSprite;
            }
        }
    }
}