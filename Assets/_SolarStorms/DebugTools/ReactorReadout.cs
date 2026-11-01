using SolarStorms.Gameplay;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.DebugTools
{
    /// <summary>
    /// Step 2 reactor readout, drawn bottom-left. Shows each system's level and multiplier, heat, and lockout,
    /// applies presets on F2 / F3 / F4 (Balanced, Combat, Burn), and times how long heat takes to climb from
    /// zero to a lockout, for the heat targets. Power changes and lockouts are also written to the console.
    /// </summary>
    public sealed class ReactorReadout : MonoBehaviour
    {
        [SerializeField] ShipReactor reactor;
        [SerializeField] PlayerShip ship;
        [SerializeField] BeamWeapon beam;
        [SerializeField] ReactorPresetData balancedPreset;
        [SerializeField] ReactorPresetData combatPreset;
        [SerializeField] ReactorPresetData burnPreset;
        [SerializeField] KeyCode balancedKey = KeyCode.F2;
        [SerializeField] KeyCode combatKey = KeyCode.F3;
        [SerializeField] KeyCode burnKey = KeyCode.F4;

        float lastZeroHeatTime;
        string lastOverheatResult = "-";
        string lastPresetMessage = "";
        GUIStyle style;

        void OnEnable()
        {
            ReactorModel model = reactor.Model;
            model.PowerChanged += OnPowerChanged;
            model.HeatChanged += OnHeatChanged;
            model.OverheatStarted += OnOverheatStarted;
            model.OverheatEnded += OnOverheatEnded;
        }

        void OnDisable()
        {
            ReactorModel model = reactor.Model;
            model.PowerChanged -= OnPowerChanged;
            model.HeatChanged -= OnHeatChanged;
            model.OverheatStarted -= OnOverheatStarted;
            model.OverheatEnded -= OnOverheatEnded;
        }

        void OnPowerChanged(PowerChange change)
        {
            string cause = change.IsPreset ? $"preset {change.Preset.displayName}" : $"bump {change.System}";
            Debug.Log($"[Reactor] {cause} at frame {Time.frameCount}: {Allocations()}");
        }

        void OnHeatChanged(float heat)
        {
            if (heat <= 0f)
            {
                lastZeroHeatTime = Time.time;
            }
        }

        void OnOverheatStarted()
        {
            float seconds = Time.time - lastZeroHeatTime;
            lastOverheatResult = $"{seconds:0.0}s from zero heat";
            Debug.Log($"[Reactor] OVERHEAT after {seconds:0.00}s from zero heat. {Allocations()}");
        }

        void OnOverheatEnded()
        {
            Debug.Log($"[Reactor] Lockout ended at heat {reactor.Model.Heat:0.0}");
        }

        void OnGUI()
        {
            HandlePresetKeys(Event.current);

            style ??= new GUIStyle(GUI.skin.label) { richText = true, wordWrap = false };
            style.fontSize = Mathf.Clamp(Screen.height / 45, 11, 24);

            ReactorModel model = reactor.Model;
            GUILayout.BeginArea(new Rect(10f, 10f, Screen.width - 20f, Screen.height - 20f));
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(false));
            GUILayout.Label("<b>Step 2 reactor</b>", style);
            for (int i = 0; i < ReactorModel.SystemCount; i++)
            {
                var system = (ReactorSystem)i;
                GUILayout.Label($"{system,-8} {model.Level(system),3}   x{model.Multiplier(system):0.00}", style);
            }

            string lockout = model.IsLockedOut ? "<color=#FF5050>LOCKED OUT</color>" : "ok";
            GUILayout.Label($"Heat {model.Heat:0.0} / {model.Data.maxHeat:0}   {lockout}", style);
            GUILayout.Label($"Top speed {ship.Flight.MaxSpeed:0.00}   Boosting {ship.Flight.IsBoosting}   Fire interval {beam.CurrentInterval:0.000}s", style);
            GUILayout.Label($"Last overheat: {lastOverheatResult}", style);
            GUILayout.Label($"Presets: {balancedKey} Balanced, {combatKey} Combat, {burnKey} Burn   {lastPresetMessage}", style);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void HandlePresetKeys(Event current)
        {
            if (current.type != EventType.KeyDown)
            {
                return;
            }

            ReactorPresetData preset = current.keyCode == balancedKey ? balancedPreset
                : current.keyCode == combatKey ? combatPreset
                : current.keyCode == burnKey ? burnPreset
                : null;
            if (preset == null)
            {
                return;
            }

            bool applied = reactor.Model.ApplyPreset(preset);
            lastPresetMessage = applied ? $"applied {preset.displayName}" : $"<color=#FF5050>{preset.displayName} refused</color>";
            current.Use();
        }

        string Allocations()
        {
            ReactorModel model = reactor.Model;
            return $"W{model.Level(ReactorSystem.Weapons)} S{model.Level(ReactorSystem.Shields)} " +
                   $"E{model.Level(ReactorSystem.Engines)} Sn{model.Level(ReactorSystem.Sensors)}";
        }
    }
}
