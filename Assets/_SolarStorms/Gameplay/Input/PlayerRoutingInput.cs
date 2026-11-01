using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// Turns the four routing inputs into quick-routing bumps: one per tap, repeating while held.
    /// Runs every frame rather than every physics step, so no tap is missed.
    /// </summary>
    public sealed class PlayerRoutingInput : MonoBehaviour
    {
        [SerializeField] PlayerInputReader input;
        [SerializeField] ShipReactor reactor;

        readonly HoldRepeater[] repeaters =
        {
            new HoldRepeater(), new HoldRepeater(), new HoldRepeater(), new HoldRepeater(),
        };

        void Update()
        {
            ReactorModel model = reactor.Model;
            ReactorData data = model.Data;

            for (int i = 0; i < ReactorModel.SystemCount; i++)
            {
                var system = (ReactorSystem)i;
                int bumps = repeaters[i].Tick(input.IsRouteHeld(system), Time.deltaTime, data.repeatDelay, data.repeatInterval);
                for (int bump = 0; bump < bumps; bump++)
                {
                    model.QuickRoute(system);
                }
            }
        }
    }
}
