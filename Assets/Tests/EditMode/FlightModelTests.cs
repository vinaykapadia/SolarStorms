using System;
using NUnit.Framework;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Tests.EditMode
{
    public sealed class FlightModelTests
    {
        const float Step = TestData.Step;
        const float Tolerance = TestData.Tolerance;
        const float SpeedEpsilon = 0.001f;

        TestData test;
        ShipClassData shipClass;
        ReactorData reactorData;

        // Balanced (25/25/25/25): every Step 1 number must still pass at standard power.
        ReactorModel reactor;

        [SetUp]
        public void SetUp()
        {
            test = new TestData();
            shipClass = test.Create<ShipClassData>();
            shipClass.maxSpeed = 10f;
            shipClass.timeToFullSpeed = 0.5f;
            shipClass.timeToStop = 0.5f;
            shipClass.fullTurnTime = 1.5f;
            reactorData = test.Create<ReactorData>();
            reactor = test.Reactor(reactorData);
        }

        [TearDown]
        public void TearDown() => test.DestroyAll();

        [Test]
        public void ReachesFullSpeedFromStandstillInHalfASecond()
        {
            var flight = new FlightModel(shipClass, reactor);

            float elapsed = RunUntil(flight, FlightCommand.Turning(0f, 1f), () => flight.Speed >= flight.MaxSpeed - SpeedEpsilon);

            Assert.That(elapsed, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void StopsFromFullSpeedInHalfASecond()
        {
            var flight = new FlightModel(shipClass, reactor);
            RunUntil(flight, FlightCommand.Turning(0f, 1f), () => flight.Speed >= flight.MaxSpeed - SpeedEpsilon);

            float elapsed = RunUntil(flight, FlightCommand.Turning(0f, 0f), () => flight.Speed <= 0f);

            Assert.That(elapsed, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void TurnsAFullCircleInOneAndAHalfSeconds()
        {
            var flight = new FlightModel(shipClass, reactor);

            float elapsed = RunUntil(flight, FlightCommand.Turning(1f, 0f), () => flight.HeadingDegrees >= 360f - 0.001f);

            Assert.That(elapsed, Is.EqualTo(1.5f).Within(Tolerance));
        }

        [Test]
        public void TurnsTowardAStickHeadingAtTheSameRateAndStopsThere()
        {
            var flight = new FlightModel(shipClass, reactor);

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
            var flight = new FlightModel(shipClass, reactor);

            flight.Step(FlightCommand.TowardHeading(270f, 0f), Step);

            Assert.That(flight.HeadingDegrees, Is.LessThan(0f), "270° is 90° clockwise from 0°, so the heading should decrease.");
        }

        [Test]
        public void VelocityAlwaysPointsAlongTheHeading()
        {
            var flight = new FlightModel(shipClass, reactor);
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
            var flight = new FlightModel(shipClass, reactor);

            RunUntil(flight, FlightCommand.Turning(0f, 0.5f), () => flight.Speed >= 5f - SpeedEpsilon);
            flight.Step(FlightCommand.Turning(0f, 0.5f), Step);

            Assert.That(flight.Speed, Is.EqualTo(5f).Within(SpeedEpsilon));
        }

        [TestCase(5, 0.6f, TestName = "Engines at the floor: 0.6x speed and turn rate")]
        [TestCase(60, 1.3f, TestName = "Engines at the cap: 1.3x speed and turn rate")]
        public void EnginesPowerScalesTopSpeedAndTurnRate(int enginesLevel, float multiplier)
        {
            int others = (100 - enginesLevel) / 3;
            reactor = test.Reactor(reactorData, test.Preset(others, others, enginesLevel, 100 - enginesLevel - 2 * others));
            var flight = new FlightModel(shipClass, reactor);

            Assert.That(flight.MaxSpeed, Is.EqualTo(shipClass.maxSpeed * multiplier).Within(SpeedEpsilon));

            float turnTime = RunUntil(flight, FlightCommand.Turning(1f, 0f), () => flight.HeadingDegrees >= 360f - 0.001f);
            Assert.That(turnTime, Is.EqualTo(1.5f / multiplier).Within(Tolerance));
        }

        [TestCase(5, TestName = "Time to full speed stays 0.5 s with Engines at the floor")]
        [TestCase(60, TestName = "Time to full speed stays 0.5 s with Engines at the cap")]
        public void TimeToFullSpeedIsTheSameAtAnyEnginesPower(int enginesLevel)
        {
            int others = (100 - enginesLevel) / 3;
            reactor = test.Reactor(reactorData, test.Preset(others, others, enginesLevel, 100 - enginesLevel - 2 * others));
            var flight = new FlightModel(shipClass, reactor);

            float elapsed = RunUntil(flight, FlightCommand.Turning(0f, 1f), () => flight.Speed >= flight.MaxSpeed - SpeedEpsilon);

            Assert.That(elapsed, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void BoostRaisesTopSpeedAndAddsHeat()
        {
            var flight = new FlightModel(shipClass, reactor);

            RunUntil(flight, FlightCommand.Turning(0f, 1f, boost: true), () => flight.Speed >= shipClass.maxSpeed * shipClass.boostSpeedMultiplier - SpeedEpsilon);

            Assert.That(flight.IsBoosting, Is.True);
            Assert.That(reactor.Heat, Is.GreaterThan(0f));
        }

        [Test]
        public void BoostWithoutThrottleDoesNothing()
        {
            var flight = new FlightModel(shipClass, reactor);

            flight.Step(FlightCommand.Turning(0f, 0f, boost: true), Step);

            Assert.That(flight.IsBoosting, Is.False);
            Assert.That(reactor.Heat, Is.EqualTo(0f));
        }

        [Test]
        public void SpeedNeverPassesTheCapAtAnyEnginesPowerWithBoost()
        {
            // The real starting values: max speed 8, boost 1.35, cap 14, against bolt speed 16.67.
            shipClass.maxSpeed = 8f;
            var weapon = test.Create<BeamWeaponData>();
            Assert.That(shipClass.speedCap, Is.LessThan(weapon.boltSpeed), "The cap itself must stay below bolt speed.");

            for (int enginesLevel = reactorData.floorLevel; enginesLevel <= reactorData.capLevel; enginesLevel++)
            {
                int others = (100 - enginesLevel) / 3;
                ReactorModel levelReactor = test.Reactor(reactorData, test.Preset(others, others, enginesLevel, 100 - enginesLevel - 2 * others));
                var flight = new FlightModel(shipClass, levelReactor);

                for (int i = 0; i < 100; i++)
                {
                    flight.Step(FlightCommand.Turning(0f, 1f, boost: true), Step);
                    levelReactor.Tick(Step);
                    Assert.That(flight.Speed, Is.LessThanOrEqualTo(shipClass.speedCap + SpeedEpsilon), $"Engines {enginesLevel}");
                }
            }
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
