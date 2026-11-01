using SolarStorms.Simulation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// Reads the single Flight action map and turns it into flight, fire, boost, and routing input.
    /// Controller: the left stick's direction is the heading and its tilt is the throttle; the D-pad routes power.
    /// Keyboard: A/D turn, W thrusts, Shift boosts; arrow keys or numpad 8/6/2/4 route power.
    /// </summary>
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] InputActionAsset actions;

        InputActionMap flightMap;
        InputAction steer;
        InputAction turn;
        InputAction thrust;
        InputAction boost;
        InputAction fire;
        readonly InputAction[] route = new InputAction[ReactorModel.SystemCount];

        public bool FireHeld => fire.IsPressed();

        /// <summary>Display name of the device that last moved any flight input. For the debug readout.</summary>
        public string LastDeviceName { get; private set; } = "none";

        void Awake()
        {
            flightMap = actions.FindActionMap("Flight", throwIfNotFound: true);
            steer = flightMap.FindAction("Steer", throwIfNotFound: true);
            turn = flightMap.FindAction("Turn", throwIfNotFound: true);
            thrust = flightMap.FindAction("Thrust", throwIfNotFound: true);
            boost = flightMap.FindAction("Boost", throwIfNotFound: true);
            fire = flightMap.FindAction("Fire", throwIfNotFound: true);
            route[(int)ReactorSystem.Weapons] = flightMap.FindAction("RouteWeapons", throwIfNotFound: true);
            route[(int)ReactorSystem.Shields] = flightMap.FindAction("RouteShields", throwIfNotFound: true);
            route[(int)ReactorSystem.Engines] = flightMap.FindAction("RouteEngines", throwIfNotFound: true);
            route[(int)ReactorSystem.Sensors] = flightMap.FindAction("RouteSensors", throwIfNotFound: true);
        }

        void OnEnable() => flightMap.Enable();
        void OnDisable() => flightMap.Disable();

        public FlightCommand ReadFlightCommand()
        {
            NoteActiveDevice();

            bool boosting = boost.IsPressed();
            Vector2 stick = Vector2.ClampMagnitude(steer.ReadValue<Vector2>(), 1f);
            if (stick.sqrMagnitude > 0f)
            {
                return FlightCommand.TowardHeading(FlightModel.HeadingFromDirection(stick), stick.magnitude, boosting);
            }

            return FlightCommand.Turning(turn.ReadValue<float>(), thrust.IsPressed() ? 1f : 0f, boosting);
        }

        /// <summary>
        /// Whether the routing input for <paramref name="system"/> is held this frame.
        /// Also true on the frame of a press, so a tap shorter than one frame still counts.
        /// </summary>
        public bool IsRouteHeld(ReactorSystem system)
        {
            InputAction action = route[(int)system];
            return action.IsPressed() || action.WasPressedThisFrame();
        }

        void NoteActiveDevice()
        {
            InputControl control = steer.activeControl ?? turn.activeControl ?? thrust.activeControl
                ?? boost.activeControl ?? fire.activeControl;
            if (control != null)
            {
                LastDeviceName = control.device.displayName;
            }
        }
    }
}
