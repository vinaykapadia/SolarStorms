using SolarStorms.Simulation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// Reads the single Flight action map and turns it into flight and fire input.
    /// Controller: the left stick's direction is the heading and its tilt is the throttle.
    /// Keyboard: A/D turn and W thrusts.
    /// </summary>
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] InputActionAsset actions;

        InputActionMap flightMap;
        InputAction steer;
        InputAction turn;
        InputAction thrust;
        InputAction fire;

        public bool FireHeld => fire.IsPressed();

        /// <summary>Display name of the device that last moved any flight input. For the debug readout.</summary>
        public string LastDeviceName { get; private set; } = "none";

        void Awake()
        {
            flightMap = actions.FindActionMap("Flight", throwIfNotFound: true);
            steer = flightMap.FindAction("Steer", throwIfNotFound: true);
            turn = flightMap.FindAction("Turn", throwIfNotFound: true);
            thrust = flightMap.FindAction("Thrust", throwIfNotFound: true);
            fire = flightMap.FindAction("Fire", throwIfNotFound: true);
        }

        void OnEnable() => flightMap.Enable();
        void OnDisable() => flightMap.Disable();

        public FlightCommand ReadFlightCommand()
        {
            NoteActiveDevice();

            Vector2 stick = Vector2.ClampMagnitude(steer.ReadValue<Vector2>(), 1f);
            if (stick.sqrMagnitude > 0f)
            {
                return FlightCommand.TowardHeading(FlightModel.HeadingFromDirection(stick), stick.magnitude);
            }

            return FlightCommand.Turning(turn.ReadValue<float>(), thrust.IsPressed() ? 1f : 0f);
        }

        void NoteActiveDevice()
        {
            InputControl control = steer.activeControl ?? turn.activeControl ?? thrust.activeControl ?? fire.activeControl;
            if (control != null)
            {
                LastDeviceName = control.device.displayName;
            }
        }
    }
}
