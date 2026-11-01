using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Flight values for one ship class. Engine values are at standard power (Engines at 25);
    /// the reactor's Engines multiplier scales top speed, acceleration, and turn rate from here.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Ship Class", fileName = "ShipClass")]
    public sealed class ShipClassData : ScriptableObject
    {
        [Header("Engines at standard power")]
        [Tooltip("Top speed in world units per second. Set to 8 in play so bolts (16.67) clearly outrun the ship.")]
        [Min(0.01f)] public float maxSpeed = 8f;

        [Tooltip("Seconds from standstill to full speed. Target: 0.5. Unchanged by Engines power, which scales speed and acceleration together.")]
        [Min(0.01f)] public float timeToFullSpeed = 0.5f;

        [Tooltip("Seconds from full speed to a stop. Target: 0.5.")]
        [Min(0.01f)] public float timeToStop = 0.5f;

        [Tooltip("Seconds for a full 360° turn at standard power. Target: 1.5.")]
        [Min(0.01f)] public float fullTurnTime = 1.5f;

        [Header("Boost")]
        [Tooltip("Top speed multiplier while boosting, on top of the Engines multiplier.")]
        [Min(1f)] public float boostSpeedMultiplier = 1.35f;

        [Tooltip("Reactor heat added per second of boost.")]
        [Min(0f)] public float boostHeatPerSecond = 18f;

        [Header("Limits")]
        [Tooltip("Hard top speed, including boosted Engines plus boost. Must stay below bolt speed (16.67).")]
        [Min(0.01f)] public float speedCap = 14f;

        public float Acceleration => maxSpeed / timeToFullSpeed;
        public float Deceleration => maxSpeed / timeToStop;
        public float TurnRateDegrees => 360f / fullTurnTime;
    }
}
