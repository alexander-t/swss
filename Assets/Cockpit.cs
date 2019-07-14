using UnityEngine;
using UnityEngine.UI;

public class Cockpit : MonoBehaviour
{
    public Text textVelocity;
        
    void OnVelocityChange(float[] velocities)
    {
        if (textVelocity != null)
        {
            textVelocity.text = (int)velocities[0] + "";
        }
    }
}
