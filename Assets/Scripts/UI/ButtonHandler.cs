using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

namespace UI {

    [RequireComponent(typeof(AudioSource))]
    public class ButtonHandler : MonoBehaviour
    {
        public AudioSource audioSource;

        void Update()
        {
            Cursor.visible = true;
        }

        public void OnPointerEnter()
        {
            audioSource.Play();
        }

        public void OnSelectMission()
        {
            SceneManager.LoadScene(Constants.Scene_MissionSelection);
        }

        public void OnFlyMission()
        {
            SceneManager.LoadScene(MissionSelectData.missionScene);
        }

        public void OnAttackAnUndefendedDepot() {
            MissionSelectData.missionScene = Constants.Scene_MissionAttackAnUndefendedDepot;
            MissionSelectData.briefingImages = Resources.LoadAll<Sprite>("Briefings/Mission_AttackAnUndefendedDepot");
            SceneManager.LoadScene(Constants.Scene_Briefing);
        }

        public void OnWipeOutDeltaSquadron()
        {
            MissionSelectData.missionScene = Constants.Scene_MissionWipeOutDeltaSquadron;
            MissionSelectData.briefingImages = Resources.LoadAll<Sprite>("Briefings/Mission_WipeOutDeltaSquadron");
            SceneManager.LoadScene(Constants.Scene_Briefing);
        }

        public void OnMercyKillDefiance()
        {
            MissionSelectData.missionScene = Constants.Scene_MissionMercyKillDefiance;
            SceneManager.LoadScene(Constants.Scene_MissionMercyKillDefiance);
        }
    }
}
