using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Values for one beam weapon, single burst only.
    /// TEMPORARY: these are the beam values at standard power. The Step 2 reactor model replaces them.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Beam Weapon", fileName = "BeamWeapon")]
    public sealed class BeamWeaponData : ScriptableObject
    {
        [Header("Beam at standard power (temporary until the Step 2 reactor)")]
        [Tooltip("World units per second. Starting value: the camera's corner distance (12.5) ÷ 0.75 s.")]
        [Min(0.01f)] public float boltSpeed = 16.667f;

        [Tooltip("Seconds between shots while the trigger is held. 3.3 s ÷ 9 gaps = 10 shots in 3.3 s.")]
        [Min(0.01f)] public float fireInterval = 0.366667f;

        [Tooltip("Seconds before a bolt disappears. Just past the 0.75 s it takes to reach the corner.")]
        [Min(0.01f)] public float boltLifetime = 0.8f;

        [Tooltip("Add the ship's velocity to each bolt. Off keeps every bolt at the same speed.")]
        public bool inheritShipVelocity;
    }
}
