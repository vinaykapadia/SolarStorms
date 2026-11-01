using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// The player's ship. Steps the flight model each physics step and applies its heading and velocity.
    /// The Rigidbody2D should be Dynamic, with gravity scale 0 and interpolation on.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerShip : MonoBehaviour
    {
        [SerializeField] ShipClassData shipClass;
        [SerializeField] PlayerInputReader input;

        Rigidbody2D body;

        public FlightModel Flight { get; private set; }

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            Flight = new FlightModel(shipClass, body.rotation);
        }

        void FixedUpdate()
        {
            Flight.Step(input.ReadFlightCommand(), Time.fixedDeltaTime);
            body.MoveRotation(Flight.HeadingDegrees);
            body.linearVelocity = Flight.Velocity;
            body.angularVelocity = 0f;
        }
    }
}
