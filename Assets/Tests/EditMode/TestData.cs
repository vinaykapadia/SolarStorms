using System.Collections.Generic;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Tests.EditMode
{
    /// <summary>
    /// Creates data objects with their code defaults (the starting values) for tests, and destroys them afterwards.
    /// </summary>
    public sealed class TestData
    {
        // Unity's default physics step. A timing result may land one step either side of its target.
        public const float Step = 0.02f;
        public const float Tolerance = Step + 0.001f;

        readonly List<Object> created = new List<Object>();

        public T Create<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            created.Add(asset);
            return asset;
        }

        public ReactorPresetData Preset(int weapons, int shields, int engines, int sensors)
        {
            var preset = Create<ReactorPresetData>();
            preset.displayName = $"Test {weapons}/{shields}/{engines}/{sensors}";
            preset.weapons = weapons;
            preset.shields = shields;
            preset.engines = engines;
            preset.sensors = sensors;
            return preset;
        }

        public ReactorModel Reactor(ReactorData data, ReactorPresetData preset = null) =>
            new ReactorModel(data, new CombatState(), preset);

        public void DestroyAll()
        {
            foreach (Object asset in created)
            {
                Object.DestroyImmediate(asset);
            }

            created.Clear();
        }
    }
}
