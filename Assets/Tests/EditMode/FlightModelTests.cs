using System;
using NUnit.Framework;
using SolarStorms.Simulation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SolarStorms.Tests.EditMode
{
    public sealed class FlightModelTests
    {
        // Unity's default physics step. A result may land one step either side of its target.
        const float Step = 0.02f;
        const float Tolerance = Step + 0.001f;
        const float SpeedEpsilon = 0.001f;

        ShipClassData shipClass;

        [SetUp]
        public void SetUp()
        {
            shipClass = ScriptableObject.CreateInstance<ShipClassData>();
            shipClass.maxSpeed = 10f;
            shipClass.timeToFullSpeed = 0.5f;
            shipClass.timeToStop = 0.5f;
            shipClass.fullTurnTime = 1.5f;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(shipClass);
        }

        [Test]
        public void ReachesFullSpeedFromStandstillInHalfASecond()
        {
            var flight = new FlightModel(shipClass);

            float elapsed = RunUntil(flight, FlightCommand.Turning(0f, 1f), () => flight.Speed >= flight.MaxSpeed - SpeedEpsilon);

            Assert.That(elapsed, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void StopsFromFullSpeedInHalfASecond()
        {
            var flight = new FlightModel(shipClass);
            RunUntil(flight, FlightCommand.Turning(0f, 1f), () => flight.Speed >= flight.MaxSpeed - SpeedEpsilon);

            float elapsed = RunUntil(flight, FlightCommand.Turning(0f, 0f), () => flight.Speed <= 0f);

            Assert.That(elapsed, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void TurnsAFullCircleInOneAndAHalfSeconds()
        {
            var flight = new FlightModel(shipClass);

            float elapsed = RunUntil(flight, FlightCommand.Turning(1f, 0f), () => flight.HeadingDegrees >= 360f - 0.001f);

            Assert.That(elapsed, Is.EqualTo(1.5f).Within(Tolerance));
        }

        [Test]
        public void TurnsTowardAStickHeadingAtTheSameRateAndStopsThere()
        {
            var flight = new FlightModel(shipClass);

            float elapsed = RunUntil(flight, FlightCommand.TowardHeading(90f, 0f), () => Mathf.Approximately(flight.HeadingDegrees, 90f));
            for (int i = 0; i < 10; i++)
            {
                flight.Step(FlightCommand.TowardHeading(90f, 0f), Step);
            }

            Assert.That(elapsed, Is.EqualTo(0.375f).Within(Tolerance));
            Assert.That(flight.HeadingDegrees, Is.EqualTo(90f).Within(0.001f));
        }

        [Test]
        public void TurnsTheShortWayRoundTowardAStickHeading()
        {
            var flight = new FlightModel(shipClass);

            flight.Step(FlightCommand.TowardHeading(270f, 0f), Step);

            Assert.That(flight.HeadingDegrees, Is.LessThan(0f), "270° is 90° clockwise from 0°, so the heading should decrease.");
        }

        [Test]
        public void VelocityAlwaysPointsAlongTheHeading()
        {
            var flight = new FlightModel(shipClass);
            RunUntil(flight, FlightCommand.Turning(0f, 1f), () => flight.Speed >= flight.MaxSpeed - SpeedEpsilon);

            for (int i = 0; i < 40; i++)
            {
                flight.Step(FlightCommand.Turning(1f, i < 20 ? 1f : 0f), Step);
                if (flight.Speed > 0f)
                {
                    // Compare by the sideways component (2D cross product), not Vector2.Angle:
                    // Angle uses acos, which turns float rounding near 0° into hundredths of a degree.
                    Vector2 direction = flight.Velocity / flight.Speed;
                    Vector2 forward = flight.Forward;
                    float sideways = direction.x * forward.y - direction.y * forward.x;
                    Assert.That(Mathf.Abs(sideways), Is.LessThan(1e-5f), $"Sideways drift at step {i}");
                    Assert.That(Vector2.Dot(direction, forward), Is.GreaterThan(0f), $"Velocity reversed at step {i}");
                }
            }
        }

        [Test]
        public void PartialThrottleSettlesAtPartialSpeed()
        {
            var flight = new FlightModel(shipClass);

            RunUntil(flight, FlightCommand.Turning(0f, 0.5f), () => flight.Speed >= 5f - SpeedEpsilon);
            flight.Step(FlightCommand.Turning(0f, 0.5f), Step);

            Assert.That(flight.Speed, Is.EqualTo(5f).Within(SpeedEpsilon));
        }

        static float RunUntil(FlightModel flight, FlightCommand command, Func<bool> done)
        {
            const float timeout = 10f;
            float elapsed = 0f;
            while (!done())
            {
                flight.Step(command, Step);
                elapsed += Step;
                if (elapsed > timeout)
                {
                    Assert.Fail($"Condition not reached within {timeout} s");
                }
            }

            return elapsed;
        }
    }
}
