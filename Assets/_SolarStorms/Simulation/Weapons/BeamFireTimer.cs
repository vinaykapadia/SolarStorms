using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Decides when the beam fires, and charges each shot's heat to the reactor.
    /// Holding the trigger repeats at the fire interval, which shortens with Weapons power;
    /// tapping can never fire faster than holding, releasing never banks shots, and nothing fires during a lockout.
    /// </summary>
    public sealed class BeamFireTimer
    {
        readonly BeamWeaponData weapon;
        readonly ReactorModel reactor;
        float cooldown;

        public BeamFireTimer(BeamWeaponData weapon, ReactorModel reactor)
        {
            this.weapon = weapon;
            this.reactor = reactor;
        }

        /// <summary>Seconds between shots at the current Weapons power.</summary>
        public float CurrentInterval => weapon.fireInterval / reactor.Multiplier(ReactorSystem.Weapons);

        /// <summary>Advance one step. Returns true when a bolt fires this step.</summary>
        public bool Tick(bool triggerHeld, float deltaTime)
        {
            bool canFire = triggerHeld && !reactor.IsLockedOut;
            bool fired = false;
            if (canFire && cooldown <= 0f)
            {
                // Add rather than reset, so the leftover carries over and the average rate stays exact.
                cooldown += CurrentInterval;
                reactor.AddHeat(weapon.heatPerShot);
                fired = true;
            }

            cooldown -= deltaTime;
            if (!canFire)
            {
                cooldown = Mathf.Max(cooldown, 0f);
            }

            return fired;
        }
    }
}
