namespace SolarStorms.Simulation
{
    /// <summary>
    /// Tap-and-hold timing for one input: one action when pressed, then repeats after a delay
    /// for as long as the input stays held.
    /// </summary>
    public sealed class HoldRepeater
    {
        bool wasHeld;
        float heldTime;
        float nextRepeatAt;

        /// <summary>Advance one frame. Returns how many actions to perform this frame.</summary>
        public int Tick(bool held, float deltaTime, float repeatDelay, float repeatInterval)
        {
            if (!held)
            {
                wasHeld = false;
                return 0;
            }

            if (!wasHeld)
            {
                wasHeld = true;
                heldTime = 0f;
                nextRepeatAt = repeatDelay;
                return 1;
            }

            heldTime += deltaTime;
            int repeats = 0;
            while (heldTime >= nextRepeatAt)
            {
                repeats++;
                nextRepeatAt += repeatInterval;
            }

            return repeats;
        }
    }
}
