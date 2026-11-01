using System.Linq;
using NUnit.Framework;
using SolarStorms.Simulation;

namespace SolarStorms.Tests.EditMode
{
    public sealed class QuickRoutingTests
    {
        TestData test;
        ReactorData data;

        [SetUp]
        public void SetUp()
        {
            test = new TestData();
            data = test.Create<ReactorData>();
        }

        [TearDown]
        public void TearDown() => test.DestroyAll();

        // Levels are in ReactorSystem order: Weapons, Shields, Engines, Sensors.

        [Test]
        public void BumpFromBalancedMovesThreeToTheChosenSystemAndOneFromEachOther()
        {
            int[] levels = { 25, 25, 25, 25 };

            int moved = QuickRouting.Bump(levels, ReactorSystem.Weapons, data);

            Assert.That(moved, Is.EqualTo(3));
            Assert.That(levels, Is.EqualTo(new[] { 28, 24, 24, 24 }));
        }

        [Test]
        public void LargerStepMovesThreeXAndTakesXFromEachOther()
        {
            data.quickRouteStep = 2;
            int[] levels = { 25, 25, 25, 25 };

            QuickRouting.Bump(levels, ReactorSystem.Engines, data);

            Assert.That(levels, Is.EqualTo(new[] { 23, 23, 31, 23 }));
        }

        [Test]
        public void SystemsAtTheirFloorAreSkipped()
        {
            int[] levels = { 60, 5, 30, 5 };

            int moved = QuickRouting.Bump(levels, ReactorSystem.Engines, data);

            Assert.That(moved, Is.EqualTo(3));
            Assert.That(levels, Is.EqualTo(new[] { 57, 5, 33, 5 }), "Shields and Sensors are at the floor, so all 3 come from Weapons.");
        }

        [Test]
        public void TakeIsUnevenOnceSomeSystemsReachTheirFloor()
        {
            int[] levels = { 50, 6, 6, 38 };

            QuickRouting.Bump(levels, ReactorSystem.Sensors, data);
            Assert.That(levels, Is.EqualTo(new[] { 49, 5, 5, 41 }), "First bump: one from each.");

            QuickRouting.Bump(levels, ReactorSystem.Sensors, data);
            Assert.That(levels, Is.EqualTo(new[] { 46, 5, 5, 44 }), "Second bump: Shields and Engines are at the floor.");
        }

        [Test]
        public void CapLimitsHowMuchTheChosenSystemGains()
        {
            int[] levels = { 59, 14, 14, 13 };

            int moved = QuickRouting.Bump(levels, ReactorSystem.Weapons, data);

            Assert.That(moved, Is.EqualTo(1));
            Assert.That(levels[0], Is.EqualTo(60));
            Assert.That(levels.Sum(), Is.EqualTo(100));
        }

        [Test]
        public void NothingMovesWhenTheChosenSystemIsAtItsCap()
        {
            int[] levels = { 60, 15, 15, 10 };

            int moved = QuickRouting.Bump(levels, ReactorSystem.Weapons, data);

            Assert.That(moved, Is.EqualTo(0));
            Assert.That(levels, Is.EqualTo(new[] { 60, 15, 15, 10 }));
        }

        [Test]
        public void GiftIsLimitedByWhatTheOthersCanGiveAboveTheirFloors()
        {
            // With the default floor and cap, the others can always give enough, so raise the floor to reach this case.
            data.floorLevel = 20;
            int[] nothingToGive = { 40, 20, 20, 20 };
            int[] oneToGive = { 40, 21, 20, 19 };

            Assert.That(QuickRouting.Bump(nothingToGive, ReactorSystem.Weapons, data), Is.EqualTo(0));
            Assert.That(nothingToGive, Is.EqualTo(new[] { 40, 20, 20, 20 }));

            Assert.That(QuickRouting.Bump(oneToGive, ReactorSystem.Weapons, data), Is.EqualTo(1), "Only Shields has a unit above the floor.");
            Assert.That(oneToGive, Is.EqualTo(new[] { 41, 20, 20, 19 }));
        }

        [Test]
        public void EverySequenceOfBumpsKeepsTheTotalAndStaysWithinFloorAndCap()
        {
            // Every sequence of 6 bumps from Balanced: 4^6 = 4096 sequences.
            const int length = 6;
            int sequences = 1 << (2 * length);
            for (int sequence = 0; sequence < sequences; sequence++)
            {
                int[] levels = { 25, 25, 25, 25 };
                for (int step = 0; step < length; step++)
                {
                    var system = (ReactorSystem)((sequence >> (2 * step)) & 3);
                    QuickRouting.Bump(levels, system, data);

                    Assert.That(levels.Sum(), Is.EqualTo(data.totalOutput), $"Sequence {sequence}, step {step}");
                    Assert.That(levels.Min(), Is.GreaterThanOrEqualTo(data.floorLevel), $"Sequence {sequence}, step {step}");
                    Assert.That(levels.Max(), Is.LessThanOrEqualTo(data.capLevel), $"Sequence {sequence}, step {step}");
                }
            }
        }

        [Test]
        public void HoldingOneSystemSettlesAtTheCapWithTheTotalKept()
        {
            int[] levels = { 25, 25, 25, 25 };
            for (int i = 0; i < 50; i++)
            {
                QuickRouting.Bump(levels, ReactorSystem.Shields, data);
            }

            Assert.That(levels[(int)ReactorSystem.Shields], Is.EqualTo(data.capLevel));
            Assert.That(levels.Sum(), Is.EqualTo(data.totalOutput));
        }

        [Test]
        public void ReactorModelRaisesPowerChangedOnlyWhenSomethingMoved()
        {
            ReactorModel reactor = test.Reactor(data, test.Preset(60, 15, 15, 10));
            int changes = 0;
            reactor.PowerChanged += change => changes++;

            Assert.That(reactor.QuickRoute(ReactorSystem.Weapons), Is.False, "Weapons is at its cap.");
            Assert.That(reactor.QuickRoute(ReactorSystem.Engines), Is.True);
            Assert.That(changes, Is.EqualTo(1));
        }
    }
}
