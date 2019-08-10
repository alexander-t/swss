using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MissionBriefingController : MonoBehaviour
    {
        public Image display;
        public Button previousButton;
        public Button nextButton;
        public AudioSource blipAudioSource;

        private Sprite[] briefingImages;
        private int currentImage = 0;

        void Awake()
        {
            briefingImages = MissionSelectData.briefingImages;    
        }

        void Start()
        {
            display.sprite = briefingImages[currentImage];
            SetupSlideshowButtons();
        }

        public void OnNext()
        {
            if (currentImage < briefingImages.Length - 1)
            {
                display.sprite = briefingImages[++currentImage];
                blipAudioSource.Play();
            }
            SetupSlideshowButtons();
        }

        public void OnPrevious()
        {
            if (currentImage > 0)
            {
                display.sprite = briefingImages[--currentImage];
                blipAudioSource.Play();
            }
            SetupSlideshowButtons();
        }

        private void SetupSlideshowButtons() {
            if (briefingImages.Length < 2)
            {
                previousButton.interactable = false;
                nextButton.interactable = false;
            }
            else
            {
                previousButton.interactable = currentImage > 0;
                nextButton.interactable = currentImage < briefingImages.Length - 1;
            }
        }
    }
}
