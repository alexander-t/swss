using UnityEngine;

/**
 * Adjusts the pitch of the engine audio source based on the velocity
 */
public class EngineSound : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private AudioSource engineAudioSource;
#pragma warning restore 0649

    void Awake()
    {
        EventManager.onPlayerSpeedChanged += OnPlayerSpeedChanged;
    }

    public void Mute()
    {
        engineAudioSource.mute = true;
    }

    private void OnPlayerSpeedChanged(float speed, float maxSpeed)
    {
        engineAudioSource.pitch = Mathf.Lerp(0.6f, 1, speed / maxSpeed);
    }
}
