using NUnit.Framework;
using SolarStorms.Simulation;

namespace SolarStorms.Tests.EditMode
{
    public sealed class PowerCurveTests
    {
        const float Precision = 0.0001f;

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

        [TestCase(5, 0.6f, TestName = "Floor (5) gives 0.6")]
        [TestCase(25, 1.0f, TestName = "Standard (25) gives 1.0")]
        [TestCase(60, 1.3f, TestName = "Cap (60) gives 1.3")]
        [TestCase(15, 0.8f, TestName = "Halfway from floor to standard (15) gives 0.8")]
        [TestCase(32, 1.06f, TestName = "A fifth of the way from standard to cap (32) gives 1.06")]
        [TestCase(0, 0.6f, TestName = "Below the floor clamps to 0.6")]
        [TestCase(80, 1.3f, TestName = "Above the cap clamps to 1.3")]
        public void MultiplierIsPiecewiseLinear(int level, float expected)
        {
            Assert.That(PowerCurve.Multiplier(level, data), Is.EqualTo(expected).Within(Precision));
        }

        [Test]
        public void MultiplierNeverDecreasesAsPowerRises()
        {
            float previous = PowerCurve.Multiplier(data.floorLevel, data);
            for (int level = data.floorLevel + 1; level <= data.capLevel; level++)
            {
                float multiplier = PowerCurve.Multiplier(level, data);
                Assert.That(multiplier, Is.GreaterThan(previous), $"Level {level}");
                previous = multiplier;
            }
        }
    }
}
