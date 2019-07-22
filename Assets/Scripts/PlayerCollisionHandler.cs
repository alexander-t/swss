using Core;
using Flying;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Handles collisions with objects and death.
 */
public class PlayerCollisionHandler : MonoBehaviour
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
        ship = GetComponent<Ship>();
        exploding = GetComponent<Exploding>();
        engineSound = GetComponent<EngineSound>();
        hud = GameObject.FindGameObjectWithTag(Constants.Tag_HUD);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_LaserBeam))
        {
            return;
        }
        else if (other.CompareTag(Constants.Tag_Ship))
        {
            Die("You crashed into " + other.name);
        }
        else
        {
            Die("You crashed");
        }
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
            exploding.Explode();
            MissionEndData.losingReason = reason;
            StartCoroutine(FailMissionAfterExplosion());
        }
    }

    private IEnumerator FailMissionAfterExplosion()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(Constants.Scene_MissionFailed);
    }
}
