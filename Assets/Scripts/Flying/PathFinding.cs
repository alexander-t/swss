using System;
using UnityEngine;

namespace Flying
{
    [Flags]
    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }

    public class PathFinding
    {
        private const float DetectionDistance = 50;
        private Transform transform;

        public PathFinding(Transform transform) {
            this.transform = transform;
        }

        public Direction FirstPassRaycast()
        {
            Direction availableDirections = Direction.None;
            RaycastHit hit;
            Vector3 left = transform.position - transform.right * 7;
            Vector3 right = transform.position + transform.right * 7;
            Vector3 up = transform.position + transform.up * 3;
            Vector3 down = transform.position - transform.up * 3;

            Debug.DrawRay(left, transform.forward * DetectionDistance, Color.green);
            Debug.DrawRay(right, transform.forward * DetectionDistance, Color.green);
            Debug.DrawRay(up, transform.forward * DetectionDistance, Color.green);
            Debug.DrawRay(down, transform.forward * DetectionDistance, Color.green);

            if (Physics.Raycast(up, transform.forward, out hit, DetectionDistance))
                Debug.DrawRay(up, transform.forward * DetectionDistance, Color.red);
            else
                availableDirections |= Direction.Up;

            if (Physics.Raycast(down, transform.forward, out hit, DetectionDistance))
                Debug.DrawRay(down, transform.forward * DetectionDistance, Color.red);
            else
                availableDirections |= Direction.Down;

            if (Physics.Raycast(left, transform.forward, out hit, DetectionDistance))
                Debug.DrawRay(left, transform.forward * DetectionDistance, Color.red);
            else
                availableDirections |= Direction.Left;

            if (Physics.Raycast(right, transform.forward, out hit, DetectionDistance))
                Debug.DrawRay(right, transform.forward * DetectionDistance, Color.red);
            else
                availableDirections |= Direction.Right;

            return availableDirections;
        }

        public Vector3 DetermineTurningDirection(Direction direction)
        {
            Vector3 turningDirection = Vector3.zero;

            if ((direction & Direction.Up) == Direction.Up && (direction & Direction.Down) == Direction.None)
            {
                turningDirection = -Vector3.right;
            }
            else if ((direction & Direction.Up) == Direction.None && (direction & Direction.Down) == Direction.Down)
            {
                turningDirection = Vector3.right;
            }
            else if ((direction & Direction.Left) == Direction.Left && (direction & Direction.Right) == Direction.None)
            {
                turningDirection = -Vector3.up;
            }
            else if ((direction & Direction.Left) == Direction.None && (direction & Direction.Right) == Direction.Right)
            {
                turningDirection = Vector3.up;
            }
            return turningDirection;
        }

    }
}