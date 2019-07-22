﻿using Core;
using UnityEngine;

namespace Targeting
{
    public class BoundingBox : MonoBehaviour
    {
        [Tooltip("Prefab that contains a Line Renderer component setup so that it will produce a nice bounding box")]
        public GameObject boundingBoxPrefab;

        private LineRenderer lineRenderer;
        private Transform playerTransform;

        void Awake()
        {
            playerTransform = GameObject.Find(Constants.Player).GetComponent<Transform>();
            lineRenderer = boundingBoxPrefab.GetComponent<LineRenderer>();

            lineRenderer.useWorldSpace = false;
            lineRenderer.loop = true;
            lineRenderer.enabled = false;

            lineRenderer.SetPosition(0, new Vector3(-3, 3, 0));
            lineRenderer.SetPosition(1, new Vector3(3, 3, 0));
            lineRenderer.SetPosition(2, new Vector3(3, -3, 0));
            lineRenderer.SetPosition(3, new Vector3(-3, -3, 0));
        }

        void Update()
        {
            lineRenderer.transform.rotation = playerTransform.rotation;

            // Scale the width a little so that too distant targets don't get flickering thin lines.
            lineRenderer.widthMultiplier = Mathf.Lerp(0.05f, 0.25f, Vector3.Distance(playerTransform.position, transform.position) / 100);
        }

        public void OnTargetted(bool active)
        {
            lineRenderer.enabled = active;
        }
    }
}