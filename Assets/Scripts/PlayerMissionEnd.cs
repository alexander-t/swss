using Core;
using Flying;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Handles the player's death and mission victory.
 */
public class PlayerMissionEnd : MonoBehaviour
{
    [Tooltip("Model that will be hidden during explosion")]
    public GameObject model;

    private Ship ship;
    private Exploding exploding;
    private EngineSound engineSound;
    private GameObject hud;

    private bool isAlive = true;

    void Awake()
    {
        ship = GetComponentInChildren<Ship>();
        exploding = GetComponentInChildren<Exploding>();
        engineSound = GetComponent<EngineSound>();
        hud = GameObject.FindGameObjectWithTag(Constants.Tag_HUD);
    }

    public void Die(string reason)
    {
        if (isAlive)
        {
            isAlive = false;
            ship.Halt();
            engineSound.Mute();
            model.SetActive(false);
            hud.gameObject.SetActive(false);
            Camera.main.transform.Translate(new Vector3(0, 0, -20));
            exploding.Explode(ship.name);
            MissionEndData.losingReason = reason;
            MissionEndData.missionTime = Time.timeSinceLevelLoad;

            StartCoroutine(LoadSceneAfterDelay(Constants.Scene_MissionFailed));
        }
    }

    public void WinMission() {
        MissionEndData.missionTime = Time.timeSinceLevelLoad;
        StartCoroutine(LoadSceneAfterDelay(Constants.Scene_MissionWon));
    }

    private IEnumerator LoadSceneAfterDelay(string scene)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(scene);
    }
}
