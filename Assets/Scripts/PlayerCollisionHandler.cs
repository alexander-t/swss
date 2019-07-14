﻿using Core;
using Flying;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    private Ship ship;
    private Exploding exploding;
    private EngineSound engineSound;

    private GameObject model;
    private bool isAlive = true;

    void Awake()
    {
        ship = GetComponent<Ship>();
        exploding = GetComponent<Exploding>();
        engineSound = GetComponent<EngineSound>();                

        model = GameObject.FindGameObjectWithTag("Ship");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_LaserBeam)) {
            return;
        }

        Die();
    }

    void Die() {
        if (isAlive)
        {
            isAlive = false;
            ship.Halt();
            engineSound.Mute();
            model.SetActive(false);
            Camera.main.transform.Translate(new Vector3(0, 0, -20));
            exploding.Explode();
            StartCoroutine(FailMissionAfterExplosion());
        }
    }

    private IEnumerator FailMissionAfterExplosion() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(Constants.Scene_MissionFailed);
    }
}
