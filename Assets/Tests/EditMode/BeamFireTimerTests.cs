using System.Collections.Generic;
using NUnit.Framework;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Tests.EditMode
{
    public sealed class BeamFireTimerTests
    {
        // Unity's default physics step. A result may land one step either side of its target.
        const float Step = 0.02f;
        const float Tolerance = Step + 0.001f;

        BeamWeaponData weapon;

        [SetUp]
        public void SetUp()
        {
            weapon = ScriptableObject.CreateInstance<BeamWeaponData>();
            weapon.fireInterval = 3.3f / 9f;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(weapon);
        }

        [Test]
        public void FirstShotFiresOnTheFirstStep()
        {
            var timer = new BeamFireTimer(weapon);

            Assert.That(timer.Tick(true, Step), Is.True);
        }

        [Test]
        public void TenHeldShotsTakeThreePointThreeSeconds()
        {
            var timer = new BeamFireTimer(weapon);

            List<float> shotTimes = HoldFor(timer, 5f);

            Assert.That(shotTimes.Count, Is.GreaterThanOrEqualTo(10));
            Assert.That(shotTimes[9] - shotTimes[0], Is.EqualTo(3.3f).Within(Tolerance));
        }

        [Test]
        public void TappingCannotFireFasterThanHolding()
        {
            var timer = new BeamFireTimer(weapon);
            int shots = 0;
            float elapsed = 0f;
            int step = 0;

            // Press on one step and release on the next, as fast as possible.
            while (elapsed < 3.3f + Step * 0.5f)
            {
                if (timer.Tick(step % 2 == 0, Step))
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
            var timer = new BeamFireTimer(weapon);
            for (int i = 0; i < 250; i++)
            {
                timer.Tick(false, Step);
            }

            // One extra step of margin, because the tenth shot may land one step late.
            List<float> shotTimes = HoldFor(timer, 3.3f + Step * 1.5f);

            Assert.That(shotTimes.Count, Is.EqualTo(10));
        }

        static List<float> HoldFor(BeamFireTimer timer, float seconds)
        {
            var shotTimes = new List<float>();
            float elapsed = 0f;
            while (elapsed < seconds)
            {
                if (timer.Tick(true, Step))
                {
                    shotTimes.Add(elapsed);
                }

                elapsed += Step;
            }

            return shotTimes;
        }
    }
}
