using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Camera values. The game view covers a fixed area in world units, so distances such as
    /// "ship to screen corner" are the same at any resolution.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Camera Rig", fileName = "CameraRig")]
    public sealed class CameraRigData : ScriptableObject
    {
        [Header("Game view")]
        [Tooltip("Half the game view's height, in world units.")]
        [Min(0.1f)] public float viewHalfHeight = 7.5f;

        [Tooltip("Width ÷ height of the game view, the area left of the side panel.")]
        [Min(0.1f)] public float gameViewAspect = 4f / 3f;

        [Tooltip("Width ÷ height of the whole layout (game view plus side panel). Other screen shapes are letterboxed.")]
        [Min(0.1f)] public float layoutAspect = 16f / 9f;

        [Header("Look-ahead")]
        [Tooltip("Whether look-ahead starts on. The DebugTools readout toggles it in play.")]
        public bool lookAheadEnabled = true;

        [Tooltip("How far ahead the camera looks at full speed, as a fraction of the view's half-height.")]
        [Range(0f, 1f)] public float lookAheadFraction = 0.25f;

        [Tooltip("Seconds for the look-ahead offset to catch up.")]
        [Min(0f)] public float lookAheadSmoothTime = 0.3f;

        public float ViewHalfWidth => viewHalfHeight * gameViewAspect;

        /// <summary>Distance from the centre of the game view to its corner, in world units.</summary>
        public float CornerDistance => Mathf.Sqrt(ViewHalfWidth * ViewHalfWidth + viewHalfHeight * viewHalfHeight);

        /// <summary>The game view as a normalised screen rectangle (0–1), for a screen of the given width ÷ height.</summary>
        public Rect GameViewRect(float screenAspect)
        {
            Rect layout = LayoutRect(screenAspect);
            return new Rect(layout.x, layout.y, layout.width * gameViewAspect / layoutAspect, layout.height);
        }

        /// <summary>The side panel (right of the game view) as a normalised screen rectangle (0–1).</summary>
        public Rect PanelRect(float screenAspect)
        {
            Rect layout = LayoutRect(screenAspect);
            Rect gameView = GameViewRect(screenAspect);
            return new Rect(gameView.xMax, layout.y, layout.xMax - gameView.xMax, layout.height);
        }

        /// <summary>The whole layout, letterboxed to layoutAspect and centred on the screen.</summary>
        Rect LayoutRect(float screenAspect)
        {
            float width = 1f;
            float height = 1f;
            if (screenAspect > layoutAspect)
            {
                width = layoutAspect / screenAspect;
            }
            else
            {
                height = screenAspect / layoutAspect;
            }

            return new Rect((1f - width) * 0.5f, (1f - height) * 0.5f, width, height);
        }
    }
}
