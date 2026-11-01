using NUnit.Framework;
using SolarStorms.Simulation;

namespace SolarStorms.Tests.EditMode
{
    public sealed class HoldRepeaterTests
    {
        const float Delay = 0.3f;
        const float Interval = 0.1f;
        const float Frame = 0.01f;

        [Test]
        public void PressGivesOneActionImmediately()
        {
            var repeater = new HoldRepeater();

            Assert.That(repeater.Tick(true, Frame, Delay, Interval), Is.EqualTo(1));
        }

        [Test]
        public void HoldingRepeatsAfterTheDelayThenEveryInterval()
        {
            var repeater = new HoldRepeater();
            int actions = repeater.Tick(true, Frame, Delay, Interval);

            // Hold for 1.05 s after the press (clear of any repeat boundary): repeats at 0.3, 0.4, ... 1.0 = 8 repeats.
            for (int frame = 0; frame < 105; frame++)
            {
                actions += repeater.Tick(true, Frame, Delay, Interval);
            }

            Assert.That(actions, Is.EqualTo(1 + 8));
        }

        [Test]
        public void NoRepeatBeforeTheDelay()
        {
            var repeater = new HoldRepeater();
            repeater.Tick(true, Frame, Delay, Interval);

            int repeats = 0;
            for (int frame = 0; frame < 29; frame++)
            {
                repeats += repeater.Tick(true, Frame, Delay, Interval);
            }

            Assert.That(repeats, Is.EqualTo(0));
        }

        [Test]
        public void EachSeparateTapGivesExactlyOneAction()
        {
            var repeater = new HoldRepeater();
            int actions = 0;
            for (int tap = 0; tap < 5; tap++)
            {
                actions += repeater.Tick(true, Frame, Delay, Interval);
                actions += repeater.Tick(false, Frame, Delay, Interval);
            }

            Assert.That(actions, Is.EqualTo(5));
        }
    }
}
