using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Values for one beam weapon, single burst only, at standard Weapons power.
    /// The reactor's Weapons multiplier scales fire rate and damage from here.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Beam Weapon", fileName = "BeamWeapon")]
    public sealed class BeamWeaponData : ScriptableObject
    {
        [Header("Beam at standard power")]
        [Tooltip("World units per second. Starting value: the camera's corner distance (12.5) ÷ 0.75 s.")]
        [Min(0.01f)] public float boltSpeed = 16.667f;

        [Tooltip("Seconds between shots at standard Weapons power (3.3 s ÷ 9 gaps = 10 shots in 3.3 s). Divided by the Weapons multiplier.")]
        [Min(0.01f)] public float fireInterval = 0.366667f;

        [Tooltip("Seconds before a bolt disappears. Just past the 0.75 s it takes to reach the corner.")]
        [Min(0.01f)] public float boltLifetime = 0.8f;

        [Tooltip("Add the ship's velocity to each bolt. Off keeps every bolt at the same speed.")]
        public bool inheritShipVelocity;

        [Header("Heat and damage")]
        [Tooltip("Reactor heat added per shot.")]
        [Min(0f)] public float heatPerShot = 10f;

        [Tooltip("Damage per bolt at standard Weapons power, scaled by the Weapons multiplier. Stored on each bolt for Step 3.")]
        [Min(0f)] public float damageAtStandard = 10f;
    }
}
