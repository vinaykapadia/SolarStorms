using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Flight values for one ship class.
    /// TEMPORARY: these are the engine values at standard power. The Step 2 reactor model replaces them.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Ship Class", fileName = "ShipClass")]
    public sealed class ShipClassData : ScriptableObject
    {
        [Header("Engines at standard power (temporary until the Step 2 reactor)")]
        [Tooltip("Top speed in world units per second. Starting value: crosses the game view's width in 1.5 s.")]
        [Min(0.01f)] public float maxSpeed = 13.33f;

        [Tooltip("Seconds from standstill to full speed. Target: 0.5.")]
        [Min(0.01f)] public float timeToFullSpeed = 0.5f;

        [Tooltip("Seconds from full speed to a stop. Target: 0.5.")]
        [Min(0.01f)] public float timeToStop = 0.5f;

        [Tooltip("Seconds for a full 360° turn. Target: 1.5.")]
        [Min(0.01f)] public float fullTurnTime = 1.5f;

        public float Acceleration => maxSpeed / timeToFullSpeed;
        public float Deceleration => maxSpeed / timeToStop;
        public float TurnRateDegrees => 360f / fullTurnTime;
    }
}
