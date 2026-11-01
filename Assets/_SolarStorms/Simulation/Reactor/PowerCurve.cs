using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Turns a power level into an effect multiplier: piecewise linear from the floor multiplier
    /// at the floor level, to 1.0 at standard, to the cap multiplier at the cap. Clamped outside that range.
    /// One curve is shared by every effect.
    /// </summary>
    public static class PowerCurve
    {
        public static float Multiplier(int level, ReactorData data)
        {
            if (level <= data.standardLevel)
            {
                float belowStandard = Mathf.InverseLerp(data.floorLevel, data.standardLevel, level);
                return Mathf.Lerp(data.floorMultiplier, 1f, belowStandard);
            }

            float aboveStandard = Mathf.InverseLerp(data.standardLevel, data.capLevel, level);
            return Mathf.Lerp(1f, data.capMultiplier, aboveStandard);
        }
    }
}
