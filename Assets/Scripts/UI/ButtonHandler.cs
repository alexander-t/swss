using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

public class ButtonHandler : MonoBehaviour
{
    public void OnAttackAnUndefendedDepot() {
        
        SceneManager.LoadScene(Constants.Scene_MissionAttackAnUndefendedDepot);
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
