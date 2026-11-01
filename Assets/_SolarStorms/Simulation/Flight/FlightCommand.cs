namespace SolarStorms.Simulation
{
    /// <summary>
    /// One step of pilot input for the flight model.
    /// Either a heading to turn toward (controller stick) or a turn rate (keyboard), plus a throttle.
    /// </summary>
    public readonly struct FlightCommand
    {
        FlightCommand(bool hasTargetHeading, float targetHeadingDegrees, float turnInput, float throttle)
        {
            HasTargetHeading = hasTargetHeading;
            TargetHeadingDegrees = targetHeadingDegrees;
            TurnInput = turnInput;
            Throttle = throttle;
        }

        public bool HasTargetHeading { get; }

        /// <summary>Degrees, counter-clockwise, 0 = facing +Y. Used when HasTargetHeading is true.</summary>
        public float TargetHeadingDegrees { get; }

        /// <summary>-1 to 1; positive turns counter-clockwise (left). Used when HasTargetHeading is false.</summary>
        public float TurnInput { get; }

        /// <summary>0 to 1, a fraction of top speed.</summary>
        public float Throttle { get; }

        public static FlightCommand TowardHeading(float headingDegrees, float throttle) =>
            new FlightCommand(true, headingDegrees, 0f, throttle);

        public static FlightCommand Turning(float turnInput, float throttle) =>
            new FlightCommand(false, 0f, turnInput, throttle);
    }
}
