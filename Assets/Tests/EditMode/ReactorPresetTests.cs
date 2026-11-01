using NUnit.Framework;
using SolarStorms.Simulation;
using UnityEditor;

namespace SolarStorms.Tests.EditMode
{
    public sealed class ReactorPresetTests
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

        [Test]
        public void ApplyingAPresetSetsEveryLevelAndRaisesOnePowerChange()
        {
            ReactorModel reactor = test.Reactor(data);
            ReactorPresetData combat = test.Preset(35, 35, 15, 15);
            PowerChange? raised = null;
            reactor.PowerChanged += change => raised = change;

            Assert.That(reactor.ApplyPreset(combat), Is.True);

            Assert.That(reactor.Level(ReactorSystem.Weapons), Is.EqualTo(35));
            Assert.That(reactor.Level(ReactorSystem.Shields), Is.EqualTo(35));
            Assert.That(reactor.Level(ReactorSystem.Engines), Is.EqualTo(15));
            Assert.That(reactor.Level(ReactorSystem.Sensors), Is.EqualTo(15));
            Assert.That(raised.HasValue && raised.Value.IsPreset && raised.Value.Preset == combat, Is.True);
        }

        [Test]
        public void APresetThatDoesNotFillTheReactorIsRefused()
        {
            ReactorModel reactor = test.Reactor(data);
            UnityEngine.TestTools.LogAssert.Expect(UnityEngine.LogType.Warning, new System.Text.RegularExpressions.Regex("does not fit"));

            Assert.That(reactor.ApplyPreset(test.Preset(30, 30, 30, 30)), Is.False);
            Assert.That(reactor.Level(ReactorSystem.Weapons), Is.EqualTo(data.standardLevel));
        }

        [Test]
        public void APresetOutsideTheFloorOrCapIsRefused()
        {
            ReactorModel reactor = test.Reactor(data);
            UnityEngine.TestTools.LogAssert.Expect(UnityEngine.LogType.Warning, new System.Text.RegularExpressions.Regex("does not fit"));

            Assert.That(reactor.ApplyPreset(test.Preset(70, 10, 10, 10)), Is.False);
        }

        [Test]
        public void PresetsAreAvailableUntilCombatStateExists()
        {
            Assert.That(new CombatState().InCombat, Is.False, "Step 2 stub: presets are always available until Step 5.");
        }

        [Test]
        public void EveryPresetAssetFitsEveryReactorAsset()
        {
            string[] presetGuids = AssetDatabase.FindAssets("t:ReactorPresetData", new[] { "Assets/_SolarStorms" });
            string[] reactorGuids = AssetDatabase.FindAssets("t:ReactorData", new[] { "Assets/_SolarStorms" });
            if (presetGuids.Length == 0 || reactorGuids.Length == 0)
            {
                Assert.Ignore("No reactor or preset assets created yet.");
            }

            foreach (string reactorGuid in reactorGuids)
            {
                var reactor = AssetDatabase.LoadAssetAtPath<ReactorData>(AssetDatabase.GUIDToAssetPath(reactorGuid));
                foreach (string presetGuid in presetGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(presetGuid);
                    var preset = AssetDatabase.LoadAssetAtPath<ReactorPresetData>(path);

                    Assert.That(preset.Total, Is.EqualTo(reactor.totalOutput), $"{path} total vs {reactor.name}");
                    foreach (ReactorSystem system in System.Enum.GetValues(typeof(ReactorSystem)))
                    {
                        Assert.That(preset.Level(system), Is.InRange(reactor.floorLevel, reactor.capLevel), $"{path} {system} vs {reactor.name}");
                    }
                }
            }
        }
    }
}
