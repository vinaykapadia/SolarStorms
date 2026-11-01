using NUnit.Framework;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Tests.EditMode
{
    /// <summary>
    /// The Step 2 heat targets, simulated at the physics step with the real flight model and fire timer:
    /// standard firing without boost never overheats; boosted Weapons firing with boost overheats in roughly 6–8 s.
    /// </summary>
    public sealed class ReactorHeatTests
    {
        const float Step = TestData.Step;

        TestData test;
        ReactorData reactorData;
        ShipClassData shipClass;
        BeamWeaponData weapon;

        [SetUp]
        public void SetUp()
        {
            test = new TestData();
            reactorData = test.Create<ReactorData>();
            shipClass = test.Create<ShipClassData>();
            weapon = test.Create<BeamWeaponData>();
        }

        [TearDown]
        public void TearDown() => test.DestroyAll();

        [Test]
        public void HoldingFireForAMinuteAtStandardWeaponsNeverOverheats()
        {
            Ship ship = CreateShip(test.Preset(25, 25, 25, 25));
            float hottest = 0f;

            for (float time = 0f; time < 60f; time += Step)
            {
                ship.Step(fire: true, boost: false);
                Assert.That(ship.Reactor.IsLockedOut, Is.False, $"Locked out at {time:0.00}s");
                hottest = Mathf.Max(hottest, ship.Reactor.Heat);
            }

            Assert.That(hottest, Is.LessThanOrEqualTo(weapon.heatPerShot + 0.01f), "Heat should shed fully between shots.");
        }

        [Test]
        public void BoostedWeaponsFiringWithBoostOverheatsInSixToEightSeconds()
        {
            // Weapons at the cap; Engines takes the rest above the floors.
            Ship ship = CreateShip(test.Preset(60, 5, 30, 5));

            float seconds = RunUntilLockout(ship, fire: true, boost: true);

            Assert.That(seconds, Is.InRange(6f, 8f));
        }

        [Test]
        public void StandardFiringWithBoostAlsoOverheatsWithinEightSeconds()
        {
            Ship ship = CreateShip(test.Preset(25, 25, 25, 25));

            float seconds = RunUntilLockout(ship, fire: true, boost: true);

            Assert.That(seconds, Is.InRange(5f, 8f));
        }

        [TestCase(5, TestName = "Boosting alone never overheats at floor Weapons")]
        [TestCase(25, TestName = "Boosting alone never overheats at standard Weapons")]
        public void BoostingAloneNeverOverheats(int weaponsLevel)
        {
            int rest = (100 - weaponsLevel) / 3;
            Ship ship = CreateShip(test.Preset(weaponsLevel, rest, 100 - weaponsLevel - 2 * rest, rest));

            for (float time = 0f; time < 60f; time += Step)
            {
                ship.Step(fire: false, boost: true);
                Assert.That(ship.Reactor.IsLockedOut, Is.False, $"Locked out at {time:0.00}s");
            }
        }

        [Test]
        public void LockoutStopsTheBeamAndBoost()
        {
            Ship ship = CreateShip(test.Preset(60, 5, 30, 5));
            RunUntilLockout(ship, fire: true, boost: true);

            bool fired = ship.Step(fire: true, boost: true);

            Assert.That(fired, Is.False);
            Assert.That(ship.Flight.IsBoosting, Is.False);
        }

        [Test]
        public void LockoutEndsOnceHeatFallsBelowTheReleaseThreshold()
        {
            Ship ship = CreateShip(test.Preset(25, 25, 25, 25));
            int started = 0;
            int ended = 0;
            ship.Reactor.OverheatStarted += () => started++;
            ship.Reactor.OverheatEnded += () => ended++;
            RunUntilLockout(ship, fire: true, boost: true);

            float seconds = 0f;
            while (ship.Reactor.IsLockedOut)
            {
                ship.Step(fire: true, boost: true);
                seconds += Step;
                Assert.That(seconds, Is.LessThan(10f), "Lockout never ended.");
            }

            Assert.That(ship.Reactor.Heat, Is.LessThan(reactorData.lockoutReleaseHeat));
            Assert.That(started, Is.EqualTo(1));
            Assert.That(ended, Is.EqualTo(1));
            Assert.That(seconds, Is.EqualTo(2f).Within(0.1f), "100 → 40 heat at 30 heat/s.");
        }

        [Test]
        public void HeatNeverExceedsMax()
        {
            ReactorModel reactor = test.Reactor(reactorData);

            reactor.AddHeat(reactorData.maxHeat * 3f);

            Assert.That(reactor.Heat, Is.EqualTo(reactorData.maxHeat));
            Assert.That(reactor.IsLockedOut, Is.True);
        }

        float RunUntilLockout(Ship ship, bool fire, bool boost)
        {
            float seconds = 0f;
            while (!ship.Reactor.IsLockedOut)
            {
                ship.Step(fire, boost);
                seconds += Step;
                Assert.That(seconds, Is.LessThan(60f), "Never overheated.");
            }

            return seconds;
        }

        Ship CreateShip(ReactorPresetData preset)
        {
            Assert.That(preset.Total, Is.EqualTo(reactorData.totalOutput), "Test preset must fill the reactor.");
            ReactorModel reactor = test.Reactor(reactorData, preset);
            return new Ship(reactor, new FlightModel(shipClass, reactor), new BeamFireTimer(weapon, reactor));
        }

        /// <summary>One physics step in the same order as the game: flight, then beam, then the reactor sheds heat.</summary>
        sealed class Ship
        {
            public Ship(ReactorModel reactor, FlightModel flight, BeamFireTimer beam)
            {
                Reactor = reactor;
                Flight = flight;
                Beam = beam;
            }

            public ReactorModel Reactor { get; }
            public FlightModel Flight { get; }
            BeamFireTimer Beam { get; }

            public bool Step(bool fire, bool boost)
            {
                Flight.Step(FlightCommand.Turning(0f, 1f, boost), TestData.Step);
                bool fired = Beam.Tick(fire, TestData.Step);
                Reactor.Tick(TestData.Step);
                return fired;
            }
        }
    }
}
