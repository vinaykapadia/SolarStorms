using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// A saved power distribution, applied in one action out of combat.
    /// The four levels must add up to the reactor's total output.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Reactor Preset", fileName = "Preset")]
    public sealed class ReactorPresetData : ScriptableObject
    {
        [Tooltip("Name shown in debug readouts and logs.")]
        public string displayName = "Balanced";

        [Min(0)] public int weapons = 25;
        [Min(0)] public int shields = 25;
        [Min(0)] public int engines = 25;
        [Min(0)] public int sensors = 25;

        public int Total => weapons + shields + engines + sensors;

        public int Level(ReactorSystem system)
        {
            switch (system)
            {
                case ReactorSystem.Weapons: return weapons;
                case ReactorSystem.Shields: return shields;
                case ReactorSystem.Engines: return engines;
                default: return sensors;
            }
        }
    }
}
