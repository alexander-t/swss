using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRandomizer : MonoBehaviour
{
    public GameObject asteroidBody;

    void Start()
    {
        asteroidBody.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        asteroidBody.transform.localScale *= Random.Range(0.3f, 2);
    }
}
