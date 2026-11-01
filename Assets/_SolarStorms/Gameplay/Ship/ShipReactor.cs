using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Gameplay
{
    /// <summary>
    /// Holds and ticks one ship's ReactorModel. Everything on the ship reads power and heat through Model.
    /// Runs after the ship's flight and beam each physics step, so heat from this step is shed in the same step.
    /// </summary>
    [DefaultExecutionOrder(50)]
    public sealed class ShipReactor : MonoBehaviour
    {
        [SerializeField] ReactorData reactorData;
        [SerializeField] ReactorPresetData startingPreset;

        ReactorModel model;

        /// <summary>Created on first use, so other components can read it from their own Awake.</summary>
        public ReactorModel Model => model ??= new ReactorModel(reactorData, new CombatState(), startingPreset);

        void FixedUpdate()
        {
            Model.Tick(Time.fixedDeltaTime);
        }
    }
}
