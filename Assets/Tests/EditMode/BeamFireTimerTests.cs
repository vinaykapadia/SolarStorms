using System.Collections.Generic;
using NUnit.Framework;
using SolarStorms.Simulation;

namespace SolarStorms.Tests.EditMode
{
    public sealed class BeamFireTimerTests
    {
        const float Step = TestData.Step;
        const float Tolerance = TestData.Tolerance;

        TestData test;
        BeamWeaponData weapon;
        ReactorData reactorData;

        // Balanced (25/25/25/25): the Step 1 fire rate must still hold at standard power.
        ReactorModel reactor;

        [SetUp]
        public void SetUp()
        {
            test = new TestData();
            weapon = test.Create<BeamWeaponData>();
            weapon.fireInterval = 3.3f / 9f;
            reactorData = test.Create<ReactorData>();
            reactor = test.Reactor(reactorData);
        }

        [TearDown]
        public void TearDown() => test.DestroyAll();

        [Test]
        public void FirstShotFiresOnTheFirstStep()
        {
            var timer = new BeamFireTimer(weapon, reactor);

            Assert.That(Tick(timer, true), Is.True);
        }

        [Test]
        public void TenHeldShotsTakeThreePointThreeSeconds()
        {
            var timer = new BeamFireTimer(weapon, reactor);

            List<float> shotTimes = HoldFor(timer, 5f);

            Assert.That(shotTimes.Count, Is.GreaterThanOrEqualTo(10));
            Assert.That(shotTimes[9] - shotTimes[0], Is.EqualTo(3.3f).Within(Tolerance));
        }

        [Test]
        public void TappingCannotFireFasterThanHolding()
        {
            var timer = new BeamFireTimer(weapon, reactor);
            int shots = 0;
            float elapsed = 0f;
            int step = 0;

            // Press on one step and release on the next, as fast as possible.
            while (elapsed < 3.3f + Step * 0.5f)
            {
                if (Tick(timer, step % 2 == 0))
                {
                    shots++;
                }

                elapsed += Step;
                step++;
            }

            Assert.That(shots, Is.LessThanOrEqualTo(10));
        }

        [Test]
        public void ReleasingDoesNotBankShots()
        {
            var timer = new BeamFireTimer(weapon, reactor);
            for (int i = 0; i < 250; i++)
            {
                Tick(timer, false);
            }

            // One extra step of margin, because the tenth shot may land one step late.
            List<float> shotTimes = HoldFor(timer, 3.3f + Step * 1.5f);

            Assert.That(shotTimes.Count, Is.EqualTo(10));
        }

        [TestCase(5, 0.6f, TestName = "Weapons at the floor: interval ÷ 0.6")]
        [TestCase(60, 1.3f, TestName = "Weapons at the cap: interval ÷ 1.3")]
        public void WeaponsPowerScalesTheFireRate(int weaponsLevel, float multiplier)
        {
            int others = (100 - weaponsLevel) / 3;
            reactor = test.Reactor(reactorData, test.Preset(weaponsLevel, others, others, 100 - weaponsLevel - 2 * others));
            var timer = new BeamFireTimer(weapon, reactor);

            Assert.That(timer.CurrentInterval, Is.EqualTo(weapon.fireInterval / multiplier).Within(0.0001f));

            List<float> shotTimes = HoldFor(timer, 6f);
            Assert.That(shotTimes[9] - shotTimes[0], Is.EqualTo(9f * weapon.fireInterval / multiplier).Within(Tolerance));
        }

        [Test]
        public void EachShotAddsItsHeat()
        {
            var timer = new BeamFireTimer(weapon, reactor);

            timer.Tick(true, Step);

            Assert.That(reactor.Heat, Is.EqualTo(weapon.heatPerShot));
        }

        [Test]
        public void NothingFiresDuringALockout()
        {
            var timer = new BeamFireTimer(weapon, reactor);
            reactor.AddHeat(reactorData.maxHeat);

            Assert.That(timer.Tick(true, Step), Is.False);
        }

        /// <summary>One physics step: the timer, then the reactor sheds heat, as in the game.</summary>
        bool Tick(BeamFireTimer timer, bool held)
        {
            bool fired = timer.Tick(held, Step);
            reactor.Tick(Step);
            return fired;
        }

        List<float> HoldFor(BeamFireTimer timer, float seconds)
        {
            var shotTimes = new List<float>();
            float elapsed = 0f;
            while (elapsed < seconds)
            {
                if (Tick(timer, true))
                {
                    shotTimes.Add(elapsed);
                }

                elapsed += Step;
            }

            return shotTimes;
        }
    }
}
