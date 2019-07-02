﻿using UnityEngine;
/*
    Emits laser beams originating from two different lasers with some cooldown. 
 */
public class LaserBeamEmitter : MonoBehaviour
{
    private enum FiringMode { Single, Dual };

    public GameObject laserBeamPrefab;
    public GameObject rightGun;
    public GameObject leftGun;
    [Space(7)]
    public AudioSource audioSource;
    public AudioClip laserSound;

    private const float Cooldown = 0.2f;

    private bool rightGunFiringNext = true;
    private FiringMode firingMode = FiringMode.Single;
    private float nextShotTime;

    void Awake()
    {
        audioSource.clip = laserSound;
    }

    void Update()
    {
        Vector3 gunFocalPoint = transform.position + transform.forward.normalized * Beam.MaxRange;
        if (Constants.Player.Equals(name))
        {
            // Control only player's beams with keyboard. Do the rest via events.
            if (Input.GetKey(KeyCode.X))
            {
                ToggleFiringMode();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }
        }
    }

    void OnFire() {
        Fire();
    }
        
    private void Fire() {
        if (Time.time > nextShotTime) {
            if (firingMode == FiringMode.Single)
            {
                FireGun(rightGunFiringNext ? rightGun : leftGun);
                rightGunFiringNext = !rightGunFiringNext;
                nextShotTime = Time.time + Cooldown;
            }
            else if (firingMode == FiringMode.Dual)
            {
                FireGun(rightGun);
                FireGun(leftGun);
                nextShotTime = Time.time + Cooldown * 2;
            }
            audioSource.Play();
        }
    }

    private void FireGun(GameObject gun)
    {
        Vector3 gunFocalPoint = transform.position + transform.forward.normalized * Beam.MaxRange;
        GameObject laserBeam = Instantiate(laserBeamPrefab, gun.transform.position, Quaternion.identity) as GameObject;
        laserBeam.GetComponent<Beam>().target = gunFocalPoint;
    }

    private void ToggleFiringMode()
    {
        firingMode = firingMode == FiringMode.Single ? FiringMode.Dual : FiringMode.Single;
    }

}