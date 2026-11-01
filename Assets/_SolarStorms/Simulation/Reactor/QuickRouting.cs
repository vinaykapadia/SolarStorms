using UnityEngine;

namespace SolarStorms.Simulation
{
    /// <summary>
    /// The quick-routing rule from design doc Section 5. A bump moves up to 3X units to the chosen system,
    /// limited by the room left below its cap and by what the other three can give before reaching their floors.
    /// The units come from the others as evenly as possible, skipping any at their floor; leftover units are
    /// taken in the fixed ReactorSystem order. The total never changes.
    /// </summary>
    public static class QuickRouting
    {
        /// <summary>Applies one bump to <paramref name="levels"/> in place. Returns the units moved (0 if none).</summary>
        public static int Bump(int[] levels, ReactorSystem chosen, ReactorData data)
        {
            int chosenIndex = (int)chosen;
            int roomBelowCap = data.capLevel - levels[chosenIndex];

            int givable = 0;
            for (int i = 0; i < levels.Length; i++)
            {
                if (i != chosenIndex)
                {
                    givable += Mathf.Max(0, levels[i] - data.floorLevel);
                }
            }

            int amount = Mathf.Min(3 * data.quickRouteStep, Mathf.Min(roomBelowCap, givable));
            if (amount <= 0)
            {
                return 0;
            }

            // Take one unit at a time round the other systems, so each gives as evenly as it can.
            int stillToTake = amount;
            while (stillToTake > 0)
            {
                for (int i = 0; i < levels.Length && stillToTake > 0; i++)
                {
                    if (i == chosenIndex || levels[i] <= data.floorLevel)
                    {
                        continue;
                    }

                    levels[i]--;
                    stillToTake--;
                }
            }

            levels[chosenIndex] += amount;
            return amount;
        }
    }
}
