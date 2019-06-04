using UnityEngine;
/*
    Emits laser beams originating from two different lasers with some cooldown. 
 */ 
public class LaserBeamEmitter : MonoBehaviour
{
    public GameObject laserBeamPrefab;
    public GameObject rightGun;
    public GameObject leftGun;

    private const float Cooldown = 0.2f;

    private bool rightGunFiring = true;
    private float nextShotTime;

    void Update()
    {
        Vector3 gunFocalPoint = transform.position + transform.forward.normalized * Beam.MaxRange;
        if (Input.GetKey(KeyCode.Space) && Time.time > nextShotTime) {
            Vector3 beamStartPosition = rightGunFiring ? rightGun.transform.position : leftGun.transform.position;
            GameObject laserBeam = Instantiate(laserBeamPrefab, beamStartPosition, Quaternion.identity) as GameObject;
            laserBeam.GetComponent<Beam>().target = gunFocalPoint;
            rightGunFiring = !rightGunFiring;
            nextShotTime = Time.time + Cooldown;
        }        
    }
}
