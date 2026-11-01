using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Direct, drift-free flight. Velocity always points along the heading, and speed changes linearly.
    /// Engines power is read from the reactor every step and scales top speed, acceleration, and turn rate.
    /// Boost raises top speed and adds heat; it stops during an overheat lockout. Speed never passes the class's cap.
    /// The Gameplay ship feeds it input and applies its heading and velocity to a Rigidbody2D.
    /// </summary>
    public sealed class FlightModel
    {
        readonly ShipClassData shipClass;
        readonly ReactorModel reactor;

        public FlightModel(ShipClassData shipClass, ReactorModel reactor, float headingDegrees = 0f)
        {
            this.shipClass = shipClass;
            this.reactor = reactor;
            HeadingDegrees = headingDegrees;
        }

        /// <summary>
        /// Degrees, counter-clockwise, 0 = facing +Y. Never wrapped, so rotation interpolates without jumps.
        /// </summary>
        public float HeadingDegrees { get; private set; }

        public float Speed { get; private set; }
        public bool IsBoosting { get; private set; }

        /// <summary>Top speed right now without boost, at the current Engines power.</summary>
        public float MaxSpeed => TopSpeed(boosting: false);

        public Vector2 Forward => DirectionFromHeading(HeadingDegrees);
        public Vector2 Velocity => Forward * Speed;

        public void Step(FlightCommand command, float deltaTime)
        {
            float engines = reactor.Multiplier(ReactorSystem.Engines);

            float maxTurn = shipClass.TurnRateDegrees * engines * deltaTime;
            if (command.HasTargetHeading)
            {
                float remaining = Mathf.DeltaAngle(HeadingDegrees, command.TargetHeadingDegrees);
                HeadingDegrees += Mathf.Clamp(remaining, -maxTurn, maxTurn);
            }
            else
            {
                HeadingDegrees += Mathf.Clamp(command.TurnInput, -1f, 1f) * maxTurn;
            }

            float throttle = Mathf.Clamp01(command.Throttle);
            IsBoosting = command.Boost && throttle > 0f && !reactor.IsLockedOut;

            float targetSpeed = throttle * TopSpeed(IsBoosting);
            float rate = (targetSpeed > Speed ? shipClass.Acceleration : shipClass.Deceleration) * engines;
            Speed = Mathf.MoveTowards(Speed, targetSpeed, rate * deltaTime);

            if (IsBoosting)
            {
                reactor.AddHeat(shipClass.boostHeatPerSecond * deltaTime);
            }
        }

        float TopSpeed(bool boosting)
        {
            float speed = shipClass.maxSpeed * reactor.Multiplier(ReactorSystem.Engines);
            if (boosting)
            {
                speed *= shipClass.boostSpeedMultiplier;
            }

            return Mathf.Min(speed, shipClass.speedCap);
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
