using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Decides when the beam fires. Holding the trigger repeats at the fire interval;
    /// tapping can never fire faster than holding, and releasing never banks shots.
    /// </summary>
    public sealed class BeamFireTimer
    {
        readonly BeamWeaponData weapon;
        float cooldown;

        public BeamFireTimer(BeamWeaponData weapon)
        {
            this.weapon = weapon;
        }

        /// <summary>Advance one step. Returns true when a bolt fires this step.</summary>
        public bool Tick(bool triggerHeld, float deltaTime)
        {
            bool fired = false;
            if (triggerHeld && cooldown <= 0f)
            {
                // Add rather than reset, so the leftover carries over and the average rate stays exact.
                cooldown += weapon.fireInterval;
                fired = true;
            }

            cooldown -= deltaTime;
            if (!triggerHeld)
            {
                cooldown = Mathf.Max(cooldown, 0f);
            }

            return fired;
        }
    }
}
