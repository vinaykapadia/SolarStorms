namespace SolarStorms.Simulation
{
    /// <summary>
    /// One step of pilot input for the flight model.
    /// Either a heading to turn toward (controller stick) or a turn rate (keyboard), plus a throttle and boost.
    /// </summary>
    public readonly struct FlightCommand
    {
        FlightCommand(bool hasTargetHeading, float targetHeadingDegrees, float turnInput, float throttle, bool boost)
        {
            HasTargetHeading = hasTargetHeading;
            TargetHeadingDegrees = targetHeadingDegrees;
            TurnInput = turnInput;
            Throttle = throttle;
            Boost = boost;
        }

        public bool HasTargetHeading { get; }

        /// <summary>Degrees, counter-clockwise, 0 = facing +Y. Used when HasTargetHeading is true.</summary>
        public float TargetHeadingDegrees { get; }

        /// <summary>-1 to 1; positive turns counter-clockwise (left). Used when HasTargetHeading is false.</summary>
        public float TurnInput { get; }

        /// <summary>0 to 1, a fraction of top speed.</summary>
        public float Throttle { get; }

        /// <summary>Boost held. Only takes effect with some throttle and while the reactor is not locked out.</summary>
        public bool Boost { get; }

        public static FlightCommand TowardHeading(float headingDegrees, float throttle, bool boost = false) =>
            new FlightCommand(true, headingDegrees, 0f, throttle, boost);

        public static FlightCommand Turning(float turnInput, float throttle, bool boost = false) =>
            new FlightCommand(false, 0f, turnInput, throttle, boost);
    }
}
