using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetingComputerController : MonoBehaviour
{
    [Tooltip("UI element that displays the name of the target.")]
    public Text targetNameText;

    private FollowingTargetCamera followingTargetCamera;

    private string lastTargetName;

    private const float Cooldown = 0.1f;
    private float nextAcquisitionTime;

    void Start()
    {
        followingTargetCamera = GameObject.Find("Targeting Computer").GetComponent<FollowingTargetCamera>();
        lastTargetName = "";
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (Time.time >= nextAcquisitionTime) {
                GameObject[] targets = GameObject.FindGameObjectsWithTag("Targettable");
                if (targets.Length > 0)
                {
                    if (targets.FirstOrDefault(t => t.name == lastTargetName) != null)
                    {
                        for (int i = 0; i < targets.Length; i++) {
                            if (targets[i].name == lastTargetName) {
                                if (i < targets.Length - 1)
                                {
                                    lastTargetName = targets[i + 1].name;
                                    followingTargetCamera.target = targets[i + 1];
                                }
                                else
                                {
                                    lastTargetName = targets[0].name;
                                    followingTargetCamera.target = targets[0];
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        lastTargetName = targets[0].name;
                    }
                }

                targetNameText.text = lastTargetName;
            }
            nextAcquisitionTime = Time.time + Cooldown;
        }
    }
}
