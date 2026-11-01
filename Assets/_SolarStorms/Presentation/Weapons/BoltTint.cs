using SolarStorms.Gameplay;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Sets a bolt's brightness from the intensity it was fired with, so beam brightness follows Weapons power.
    /// Sits on the bolt prefab and reads only its own bolt.
    /// </summary>
    public sealed class BoltTint : MonoBehaviour
    {
        [SerializeField] BeamBolt bolt;
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] ReactorFeedbackData feedback;

        Color baseColor;

        void Awake()
        {
            baseColor = sprite.color;
        }

        void LateUpdate()
        {
            float brightness = Mathf.Lerp(feedback.boltBrightnessRange.x, feedback.boltBrightnessRange.y, bolt.Intensity);
            sprite.color = new Color(baseColor.r * brightness, baseColor.g * brightness, baseColor.b * brightness, baseColor.a);
        }
    }
}
