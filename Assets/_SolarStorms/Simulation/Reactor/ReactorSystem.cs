namespace SolarStorms.Simulation
{
    /// <summary>
    /// The four routable reactor systems. The order is also the fixed order in which
    /// quick-routing takes leftover units, so results never depend on chance.
    /// </summary>
    public enum ReactorSystem
    {
        Weapons = 0,
        Shields = 1,
        Engines = 2,
        Sensors = 3,
    }
}
