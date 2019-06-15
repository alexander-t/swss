using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void OnKillTheRookie() {
        Debug.Log("Rookie");
        SceneManager.LoadScene("Combat");
    }

    public void OnDogfight()
    {
        Debug.Log("DogFight");
    }

    public void OnChallengeFrigate()
    {
        Debug.Log("Frigate");
    }
}
