using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// Values for the placeholder starfield, which gives the eye something to judge speed against.
    /// Placeholder art: replaced by the real starfield in the Phase 2 art pass.
    /// </summary>
    [CreateAssetMenu(menuName = "Solar Storms/Starfield", fileName = "Starfield")]
    public sealed class StarfieldData : ScriptableObject
    {
        [Tooltip("How many stars to scatter.")]
        [Min(1)] public int starCount = 200;

        [Tooltip("Half the size of the area the stars fill around the camera, in world units. Must exceed the game view's half-size.")]
        public Vector2 areaHalfExtent = new Vector2(15f, 15f);

        [Tooltip("Smallest and largest star size, in world units.")]
        public Vector2 sizeRange = new Vector2(0.04f, 0.12f);

        [Tooltip("Dimmest and brightest star, 0 to 1.")]
        public Vector2 brightnessRange = new Vector2(0.3f, 1f);

        [Tooltip("Seed for the star layout, so every run looks the same.")]
        public int seed = 1;
    }
}
