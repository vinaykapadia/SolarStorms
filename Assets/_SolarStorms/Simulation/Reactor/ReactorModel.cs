using System;
using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// One ship's reactor. The only owner of the four power allocations, heat, and the overheat lockout:
    /// every other system reads its level or multiplier from here each time it needs one.
    /// </summary>
    public sealed class ReactorModel
    {
        public const int SystemCount = 4;

        readonly ReactorData data;
        readonly CombatState combatState;
        readonly int[] levels = new int[SystemCount];

        public ReactorModel(ReactorData data, CombatState combatState, ReactorPresetData startingPreset = null)
        {
            this.data = data;
            this.combatState = combatState;

            for (int i = 0; i < SystemCount; i++)
            {
                levels[i] = startingPreset != null && IsValid(startingPreset)
                    ? startingPreset.Level((ReactorSystem)i)
                    : data.standardLevel;
            }
        }

        /// <summary>Raised whenever any allocation changes.</summary>
        public event Action<PowerChange> PowerChanged;

        /// <summary>Raised whenever heat changes, with the new heat.</summary>
        public event Action<float> HeatChanged;

        /// <summary>Raised when heat reaches max and the beam and boost lock out.</summary>
        public event Action OverheatStarted;

        /// <summary>Raised when heat falls below the release threshold and the lockout ends.</summary>
        public event Action OverheatEnded;

        public ReactorData Data => data;
        public float Heat { get; private set; }
        public bool IsLockedOut { get; private set; }

        public int Level(ReactorSystem system) => levels[(int)system];

        public float Multiplier(ReactorSystem system) => PowerCurve.Multiplier(levels[(int)system], data);

        /// <summary>One quick-routing bump toward <paramref name="system"/>. Returns false if nothing could move.</summary>
        public bool QuickRoute(ReactorSystem system)
        {
            if (QuickRouting.Bump(levels, system, data) == 0)
            {
                return false;
            }

            PowerChanged?.Invoke(PowerChange.Routed(system));
            return true;
        }

        /// <summary>Applies a preset. Refused in combat, or if the preset does not fit this reactor.</summary>
        public bool ApplyPreset(ReactorPresetData preset)
        {
            if (combatState.InCombat)
            {
                return false;
            }

            if (!IsValid(preset))
            {
                Debug.LogWarning($"[Reactor] Preset '{preset.displayName}' does not fit this reactor " +
                                 $"(total {preset.Total}, needs {data.totalOutput}, each between {data.floorLevel} and {data.capLevel}).");
                return false;
            }

            for (int i = 0; i < SystemCount; i++)
            {
                levels[i] = preset.Level((ReactorSystem)i);
            }

            PowerChanged?.Invoke(PowerChange.FromPreset(preset));
            return true;
        }

        public void AddHeat(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            SetHeat(Mathf.Min(Heat + amount, data.maxHeat));
            if (!IsLockedOut && Heat >= data.maxHeat)
            {
                IsLockedOut = true;
                OverheatStarted?.Invoke();
            }
        }

        /// <summary>Advance one step: shed heat at a rate scaled by Weapons power, and end a lockout once cool enough.</summary>
        public void Tick(float deltaTime)
        {
            float dissipation = data.heatDissipationPerSecond * Multiplier(ReactorSystem.Weapons) * deltaTime;
            SetHeat(Mathf.Max(0f, Heat - dissipation));

            if (IsLockedOut && Heat < data.lockoutReleaseHeat)
            {
                IsLockedOut = false;
                OverheatEnded?.Invoke();
            }
        }

        bool IsValid(ReactorPresetData preset)
        {
            if (preset.Total != data.totalOutput)
            {
                return false;
            }

            for (int i = 0; i < SystemCount; i++)
            {
                int level = preset.Level((ReactorSystem)i);
                if (level < data.floorLevel || level > data.capLevel)
                {
                    return false;
                }
            }

            return true;
        }

        void SetHeat(float value)
        {
            if (value == Heat)
            {
                return;
            }

            Heat = value;
            HeatChanged?.Invoke(Heat);
        }
    }
}
