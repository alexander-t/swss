using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

namespace UI {
    public class ButtonHandler : MonoBehaviour
    {
        void Update()
        {
            Cursor.visible = true;
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
            SceneManager.LoadScene(Constants.Scene_Briefing);
        }

        public void OnDogfight()
        {
            SceneManager.LoadScene("Combat");
        }

        public void OnChallengeFrigate()
        {
            Debug.Log("Frigate");
        }
       
    }
}
