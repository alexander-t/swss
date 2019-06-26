using UnityEngine;

namespace AITesting
{
    public class ObstacleController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.D)) {
                transform.position += Vector3.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= Vector3.up;
            }
        }
    }
}