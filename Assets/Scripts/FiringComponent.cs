﻿using Core;
using Firing;
using UnityEngine;

public class FiringComponent : MonoBehaviour
{
    private GameObject target;
    public GameObject muzzle;
    public AudioSource audioSource;
    [Space(10)]
    public float rateOfFire = 5;
    public float attackRange = 250;

    private BeamPool beamPool;
    private float nextFireTime;

    void Awake()
    {
        beamPool = GameObject.Find(Constants.BeamPool).GetComponent<BeamPool>();
    }

    void Update()
    {
        if (target != null)
        {
            muzzle.transform.LookAt(target.transform.position);
            if (Time.time >= nextFireTime)
            {
                beamPool.FireAt(gameObject, muzzle.transform, target.transform);
                audioSource.Play();
                nextFireTime = Time.time + rateOfFire;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameObjects.IsBeam(other))
        {
            return;
        }

        target = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (GameObjects.IsBeam(other))
        {
            return;
        }

       target = null;
    }
}