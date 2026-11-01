using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// How reactor state is shown and heard: side-panel colours, engine glow, beam brightness, and placeholder tones.
    /// Placeholder look and sound, to be replaced in the Phase 2 art and audio pass.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Reactor Feedback", fileName = "ReactorFeedback")]
    public sealed class ReactorFeedbackData : ScriptableObject
    {
        [Header("Side panel colours")]
        public Color panelBackgroundColor = new Color(0.1f, 0.1f, 0.12f);
        public Color panelTextColor = new Color(1f, 0.85f, 0.2f);
        public Color barBackgroundColor = new Color(0.2f, 0.2f, 0.24f);
        public Color weaponsColor = new Color(1f, 0.42f, 0.24f);
        public Color enginesColor = new Color(0.24f, 0.65f, 1f);
        public Color sensorsColor = new Color(0.49f, 1f, 0.42f);
        public Color shieldsColor = new Color(0.85f, 0.85f, 1f);
        public Color heatColor = new Color(1f, 0.6f, 0.1f);
        public Color overheatColor = new Color(1f, 0.15f, 0.15f);

        [Tooltip("Flashes per second of the heat bar and OVERHEAT label during a lockout.")]
        [Min(0.1f)] public float overheatFlashRate = 4f;

        [Header("Engine glow (floor power → cap power)")]
        public Vector2 glowAlphaRange = new Vector2(0.2f, 1f);
        public Vector2 glowScaleRange = new Vector2(0.5f, 1.5f);

        [Header("Beam brightness (floor power → cap power)")]
        public Vector2 boltBrightnessRange = new Vector2(0.4f, 1f);

        [Header("Placeholder tones (Hz)")]
        [Tooltip("One distinct pitch per system, played on every routing change.")]
        public float weaponsTick = 880f;
        public float shieldsTick = 660f;
        public float enginesTick = 440f;
        public float sensorsTick = 1100f;
        public float presetTick = 550f;
        public float overheatAlarm = 220f;
        public float overheatRelease = 740f;

        [Min(0.01f)] public float tickSeconds = 0.05f;
        [Range(0f, 1f)] public float volume = 0.35f;

        public Color ColorFor(ReactorSystem system)
        {
            switch (system)
            {
                case ReactorSystem.Weapons: return weaponsColor;
                case ReactorSystem.Shields: return shieldsColor;
                case ReactorSystem.Engines: return enginesColor;
                default: return sensorsColor;
            }
        }

        public float TickFor(ReactorSystem system)
        {
            switch (system)
            {
                case ReactorSystem.Weapons: return weaponsTick;
                case ReactorSystem.Shields: return shieldsTick;
                case ReactorSystem.Engines: return enginesTick;
                default: return sensorsTick;
            }
        }
    }
}
