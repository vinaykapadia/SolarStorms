using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Values for one reactor: output, power levels, the power-to-effect curve, quick-routing, and heat.
    /// All are starting values for the Phase 0 prototype.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Reactor", fileName = "Reactor")]
    public sealed class ReactorData : ScriptableObject
    {
        [Header("Output and power levels (units)")]
        [Tooltip("Total units shared across the four systems. The allocations always add up to this.")]
        [Min(1)] public int totalOutput = 100;

        [Tooltip("Lowest level any system can be routed down to.")]
        [Min(0)] public int floorLevel = 5;

        [Tooltip("The level at which a system behaves exactly as its base stats describe (multiplier 1.0).")]
        [Min(0)] public int standardLevel = 25;

        [Tooltip("Highest level any system can be routed up to.")]
        [Min(0)] public int capLevel = 60;

        [Header("Power-to-effect curve (piecewise linear; 1.0 at standard)")]
        [Tooltip("Effect multiplier at the floor level.")]
        [Min(0f)] public float floorMultiplier = 0.6f;

        [Tooltip("Effect multiplier at the cap level.")]
        [Min(0f)] public float capMultiplier = 1.3f;

        [Header("Quick-routing")]
        [Tooltip("X: each bump takes X from each other system and adds 3X to the chosen one.")]
        [Min(1)] public int quickRouteStep = 1;

        [Tooltip("Seconds a routing input must be held before it starts repeating.")]
        [Min(0f)] public float repeatDelay = 0.3f;

        [Tooltip("Seconds between repeated bumps while the input stays held.")]
        [Min(0.01f)] public float repeatInterval = 0.1f;

        [Header("Heat")]
        [Tooltip("Heat at which the reactor locks out the beam and boost.")]
        [Min(1f)] public float maxHeat = 100f;

        [Tooltip("Heat shed per second at standard Weapons power. Scales with the Weapons multiplier.")]
        [Min(0f)] public float heatDissipationPerSecond = 30f;

        [Tooltip("A lockout ends once heat falls below this.")]
        [Min(0f)] public float lockoutReleaseHeat = 40f;
    }
}
