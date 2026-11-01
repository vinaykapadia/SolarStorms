using SolarStorms.Gameplay;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Placeholder engine glow behind the ship. Its length and opacity follow Engines power, updated on PowerChanged.
    /// Scales the pivot (placed at the engine) so the glow grows backwards, away from the hull.
    /// </summary>
    public sealed class EngineGlowView : MonoBehaviour
    {
        [SerializeField] ShipReactor reactor;
        [SerializeField] ReactorFeedbackData feedback;
        [SerializeField] Transform glowPivot;
        [SerializeField] SpriteRenderer glowSprite;

        Vector3 baseScale;
        Color baseColor;

        void Awake()
        {
            baseScale = glowPivot.localScale;
            baseColor = glowSprite.color;
        }

        void OnEnable() => reactor.Model.PowerChanged += OnPowerChanged;
        void OnDisable() => reactor.Model.PowerChanged -= OnPowerChanged;
        void Start() => Show();

        void OnPowerChanged(PowerChange change) => Show();

        void Show()
        {
            ReactorModel model = reactor.Model;
            float power = Mathf.InverseLerp(model.Data.floorMultiplier, model.Data.capMultiplier, model.Multiplier(ReactorSystem.Engines));

            float length = Mathf.Lerp(feedback.glowScaleRange.x, feedback.glowScaleRange.y, power);
            glowPivot.localScale = new Vector3(baseScale.x, baseScale.y * length, baseScale.z);

            Color color = baseColor;
            color.a = Mathf.Lerp(feedback.glowAlphaRange.x, feedback.glowAlphaRange.y, power);
            glowSprite.color = color;
        }
    }
}
