using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Direct, drift-free flight. Velocity always points along the heading, and speed changes linearly.
    /// The Gameplay ship feeds it input and applies its heading and velocity to a Rigidbody2D.
    /// </summary>
    public sealed class FlightModel
    {
        readonly ShipClassData shipClass;

        public FlightModel(ShipClassData shipClass, float headingDegrees = 0f)
        {
            this.shipClass = shipClass;
            HeadingDegrees = headingDegrees;
        }

        /// <summary>
        /// Degrees, counter-clockwise, 0 = facing +Y. Never wrapped, so rotation interpolates without jumps.
        /// </summary>
        public float HeadingDegrees { get; private set; }

        public float Speed { get; private set; }
        public float MaxSpeed => shipClass.maxSpeed;
        public Vector2 Forward => DirectionFromHeading(HeadingDegrees);
        public Vector2 Velocity => Forward * Speed;

        public void Step(FlightCommand command, float deltaTime)
        {
            float maxTurn = shipClass.TurnRateDegrees * deltaTime;
            if (command.HasTargetHeading)
            {
                float remaining = Mathf.DeltaAngle(HeadingDegrees, command.TargetHeadingDegrees);
                HeadingDegrees += Mathf.Clamp(remaining, -maxTurn, maxTurn);
            }
            else
            {
                HeadingDegrees += Mathf.Clamp(command.TurnInput, -1f, 1f) * maxTurn;
            }

            float targetSpeed = Mathf.Clamp01(command.Throttle) * shipClass.maxSpeed;
            float rate = targetSpeed > Speed ? shipClass.Acceleration : shipClass.Deceleration;
            Speed = Mathf.MoveTowards(Speed, targetSpeed, rate * deltaTime);
        }

        public static Vector2 DirectionFromHeading(float headingDegrees)
        {
            float radians = headingDegrees * Mathf.Deg2Rad;
            return new Vector2(-Mathf.Sin(radians), Mathf.Cos(radians));
        }

        public static float HeadingFromDirection(Vector2 direction) =>
            Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
    }
}
