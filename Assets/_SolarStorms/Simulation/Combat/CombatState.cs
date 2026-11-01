namespace SolarStorms.Simulation
{
    /// <summary>
    /// The one answer to "is the player in combat?", read by presets (and later travel mode and the combat lamp).
    /// STUB until Phase 0 Step 5: always reports "not in combat", so presets are always available.
    /// </summary>
    public sealed class CombatState
    {
        public bool InCombat => false;
    }
}
