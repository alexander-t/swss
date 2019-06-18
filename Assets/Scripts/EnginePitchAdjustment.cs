using UnityEngine;

/**
 * Adjusts the pitch of the engine audio source based on the velocity
 */
public class EnginePitchAdjustment : MonoBehaviour
{
    public AudioSource engineAudioSource;

    void OnVelocityChange(float[] velocities)
    {
        engineAudioSource.pitch = Mathf.Lerp(0.6f, 1, velocities[0] / (velocities[1] + 0.1f));
    }
}
