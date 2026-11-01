namespace SolarStorms.Simulation
{
    /// <summary>What caused a PowerChanged event: a quick-route bump to one system, or a preset.</summary>
    public readonly struct PowerChange
    {
        PowerChange(bool isPreset, ReactorSystem system, ReactorPresetData preset)
        {
            IsPreset = isPreset;
            System = system;
            Preset = preset;
        }

        public bool IsPreset { get; }

        /// <summary>The system that was bumped. Only meaningful when IsPreset is false.</summary>
        public ReactorSystem System { get; }

        /// <summary>The preset that was applied. Only set when IsPreset is true.</summary>
        public ReactorPresetData Preset { get; }

        public static PowerChange Routed(ReactorSystem system) => new PowerChange(false, system, null);
        public static PowerChange FromPreset(ReactorPresetData preset) => new PowerChange(true, default, preset);
    }
}
