using SolarStorms.Gameplay;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Keeps the ship at the centre of the game view, with optional look-ahead along its velocity.
    /// Fits the camera to the game view (left of the side panel) and letterboxes other screen shapes.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class CameraRig : MonoBehaviour
    {
        [SerializeField] CameraRigData data;
        [SerializeField] PlayerShip ship;

        Camera viewCamera;
        Vector2 lookAheadOffset;
        Vector2 lookAheadVelocity;

        public CameraRigData Data => data;

        /// <summary>Starts from the data asset; the debug readout toggles it in play without changing the asset.</summary>
        public bool LookAheadEnabled { get; set; }

        void Awake()
        {
            viewCamera = GetComponent<Camera>();
            viewCamera.orthographic = true;
            LookAheadEnabled = data.lookAheadEnabled;
        }

        void LateUpdate()
        {
            FitToGameView();

            Vector2 targetOffset = Vector2.zero;
            if (LookAheadEnabled)
            {
                float lookAheadDistance = data.lookAheadFraction * data.viewHalfHeight;
                targetOffset = ship.Flight.Velocity / ship.Flight.MaxSpeed * lookAheadDistance;
            }

            lookAheadOffset = Vector2.SmoothDamp(lookAheadOffset, targetOffset, ref lookAheadVelocity, data.lookAheadSmoothTime);

            Vector3 shipPosition = ship.transform.position;
            transform.position = new Vector3(
                shipPosition.x + lookAheadOffset.x,
                shipPosition.y + lookAheadOffset.y,
                transform.position.z);
        }

        void FitToGameView()
        {
            viewCamera.orthographicSize = data.viewHalfHeight;

            float screenAspect = (float)Screen.width / Screen.height;
            float layoutWidth = 1f;
            float layoutHeight = 1f;
            if (screenAspect > data.layoutAspect)
            {
                layoutWidth = data.layoutAspect / screenAspect;
            }
            else
            {
                layoutHeight = screenAspect / data.layoutAspect;
            }

            float layoutLeft = (1f - layoutWidth) * 0.5f;
            float layoutBottom = (1f - layoutHeight) * 0.5f;
            float gameViewWidth = layoutWidth * data.gameViewAspect / data.layoutAspect;
            viewCamera.rect = new Rect(layoutLeft, layoutBottom, gameViewWidth, layoutHeight);
        }
    }
}
