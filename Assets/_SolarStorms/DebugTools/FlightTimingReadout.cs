using System.Collections.Generic;
using SolarStorms.Gameplay;
using SolarStorms.Presentation;
using UnityEngine;

namespace SolarStorms.DebugTools
{
    /// <summary>
    /// Times the five Step 1 flight targets in play and shows them on screen, with a pass or fail.
    /// F1 toggles camera look-ahead for A/B comparison. Every result is also written to the console.
    /// Runs after the ship each physics step, so it sees the state the ship just produced.
    /// </summary>
    [DefaultExecutionOrder(100)]
    public sealed class FlightTimingReadout : MonoBehaviour
    {
        // Targets from the design doc, Section 2 "Direct flight". Not tunables: these are the test.
        const float FullSpeedTarget = 0.5f;
        const float StopTarget = 0.5f;
        const float FullTurnTarget = 1.5f;
        const float CornerTarget = 0.75f;
        const float TenShotTarget = 3.3f;
        const int ShotsInTest = 10;
        const float SpeedEpsilon = 0.001f;
        const float TurnEpsilon = 0.001f;

        [SerializeField] PlayerShip ship;
        [SerializeField] BeamWeapon beam;
        [SerializeField] PlayerInputReader input;
        [SerializeField] CameraRig cameraRig;
        [SerializeField] KeyCode lookAheadToggleKey = KeyCode.F1;

        readonly Timing fullSpeed = new Timing("Standstill to full speed", FullSpeedTarget);
        readonly Timing stop = new Timing("Full speed to stop", StopTarget);
        readonly Timing fullTurn = new Timing("360° turn", FullTurnTarget);
        readonly Timing corner = new Timing("Shot to screen corner", CornerTarget);
        readonly Timing tenShots = new Timing("10 shots", TenShotTarget);
        readonly List<BeamBolt> trackedBolts = new List<BeamBolt>();

        float previousSpeed;
        float previousHeading;

        bool fullSpeedRunning;
        float fullSpeedElapsed;
        bool stopRunning;
        float stopElapsed;
        bool turnRunning;
        float turnElapsed;
        float turnAccumulated;
        float turnDirection;

        int seenShotCount;
        int burstShots;
        float burstFirstShotTime;
        float burstLastShotTime;

        GUIStyle style;

        void Start()
        {
            previousSpeed = ship.Flight.Speed;
            previousHeading = ship.Flight.HeadingDegrees;
        }

        void FixedUpdate()
        {
            float step = Time.fixedDeltaTime;
            float tolerance = step + 0.001f;
            TrackFullSpeed(step, tolerance);
            TrackStop(step, tolerance);
            TrackTurn(step, tolerance);
            TrackShots(tolerance);

            previousSpeed = ship.Flight.Speed;
            previousHeading = ship.Flight.HeadingDegrees;
        }

        void Update()
        {
            TrackBoltsToCorner(Time.fixedDeltaTime + 0.001f);
        }

        void TrackFullSpeed(float step, float tolerance)
        {
            float speed = ship.Flight.Speed;
            if (previousSpeed <= 0f && speed > 0f)
            {
                fullSpeedRunning = true;
                fullSpeedElapsed = step;
            }
            else if (fullSpeedRunning)
            {
                // A real run gains speed every step; levelling off or slowing cancels it.
                if (speed <= previousSpeed)
                {
                    fullSpeedRunning = false;
                    return;
                }

                fullSpeedElapsed += step;
            }

            if (fullSpeedRunning && speed >= ship.Flight.MaxSpeed - SpeedEpsilon)
            {
                fullSpeedRunning = false;
                fullSpeed.Record(fullSpeedElapsed, tolerance);
            }
        }

        void TrackStop(float step, float tolerance)
        {
            float speed = ship.Flight.Speed;
            if (previousSpeed >= ship.Flight.MaxSpeed - SpeedEpsilon && speed < previousSpeed)
            {
                stopRunning = true;
                stopElapsed = step;
            }
            else if (stopRunning)
            {
                // A real run loses speed every step until it reaches zero; levelling off or speeding up cancels it.
                if (speed >= previousSpeed && speed > 0f)
                {
                    stopRunning = false;
                    return;
                }

                stopElapsed += step;
            }

            if (stopRunning && speed <= 0f)
            {
                stopRunning = false;
                stop.Record(stopElapsed, tolerance);
            }
        }

        void TrackTurn(float step, float tolerance)
        {
            float delta = ship.Flight.HeadingDegrees - previousHeading;
            float direction = Mathf.Sign(delta);
            if (Mathf.Approximately(delta, 0f))
            {
                turnRunning = false;
                return;
            }

            if (turnRunning && direction == turnDirection)
            {
                turnAccumulated += Mathf.Abs(delta);
                turnElapsed += step;
            }
            else
            {
                turnRunning = true;
                turnDirection = direction;
                turnAccumulated = Mathf.Abs(delta);
                turnElapsed = step;
            }

            if (turnAccumulated >= 360f - TurnEpsilon)
            {
                fullTurn.Record(turnElapsed, tolerance);
                turnRunning = false;
            }
        }

        void TrackShots(float tolerance)
        {
            if (beam.ShotCount == seenShotCount)
            {
                return;
            }

            seenShotCount = beam.ShotCount;
            float shotTime = beam.LastShotTime;
            trackedBolts.Add(beam.LastBolt);

            // A gap well over the fire interval means the trigger was released: start a new burst.
            bool continuesBurst = burstShots > 0 && shotTime - burstLastShotTime <= beam.Weapon.fireInterval * 1.5f;
            if (!continuesBurst)
            {
                burstShots = 0;
                burstFirstShotTime = shotTime;
            }

            burstShots++;
            burstLastShotTime = shotTime;

            if (burstShots == ShotsInTest)
            {
                tenShots.Record(shotTime - burstFirstShotTime, tolerance);
                burstShots = 0;
            }
        }

        void TrackBoltsToCorner(float tolerance)
        {
            float cornerDistance = cameraRig.Data.CornerDistance;
            for (int i = trackedBolts.Count - 1; i >= 0; i--)
            {
                BeamBolt bolt = trackedBolts[i];
                if (!bolt.gameObject.activeInHierarchy)
                {
                    Debug.LogWarning("[FlightTiming] A bolt expired before reaching the screen corner. Check boltLifetime.");
                    trackedBolts.RemoveAt(i);
                    continue;
                }

                if (bolt.DistanceTravelled >= cornerDistance)
                {
                    // Step back from the overshoot to the moment the bolt crossed the corner distance.
                    float crossedAt = bolt.Age - (bolt.DistanceTravelled - cornerDistance) / bolt.Speed;
                    corner.Record(crossedAt, tolerance);
                    trackedBolts.RemoveAt(i);
                }
            }
        }

        void OnGUI()
        {
            Event current = Event.current;
            if (current.type == EventType.KeyDown && current.keyCode == lookAheadToggleKey)
            {
                cameraRig.LookAheadEnabled = !cameraRig.LookAheadEnabled;
                Debug.Log($"[FlightTiming] Look-ahead {(cameraRig.LookAheadEnabled ? "on" : "off")}");
                current.Use();
            }

            // Scale the text to the window, and never wrap: the box grows to fit its lines instead.
            style ??= new GUIStyle(GUI.skin.label) { richText = true, wordWrap = false };
            style.fontSize = Mathf.Clamp(Screen.height / 45, 11, 24);

            GUILayout.BeginArea(new Rect(10f, 10f, Screen.width - 20f, Screen.height - 20f));
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(false));
            GUILayout.Label("<b>Step 1 flight timing</b>", style);
            GUILayout.Label(fullSpeed.Describe(), style);
            GUILayout.Label(stop.Describe(), style);
            GUILayout.Label(fullTurn.Describe(), style);
            GUILayout.Label(corner.Describe(), style);
            GUILayout.Label(tenShots.Describe(), style);
            GUILayout.Label(
                $"Speed {ship.Flight.Speed:0.00} / {ship.Flight.MaxSpeed:0.00}   " +
                $"Heading {Mathf.Repeat(ship.Flight.HeadingDegrees, 360f):0}°",
                style);
            GUILayout.Label(
                $"Look-ahead {(cameraRig.LookAheadEnabled ? "ON" : "OFF")} ({lookAheadToggleKey})   " +
                $"Device: {input.LastDeviceName}",
                style);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        sealed class Timing
        {
            readonly string name;
            readonly float target;
            float last = -1f;
            bool passed;
            int runs;

            public Timing(string name, float target)
            {
                this.name = name;
                this.target = target;
            }

            public void Record(float seconds, float tolerance)
            {
                last = seconds;
                passed = Mathf.Abs(seconds - target) <= tolerance;
                runs++;
                Debug.Log($"[FlightTiming] {name}: {seconds:0.000}s (target {target:0.00}s ±{tolerance:0.000}) {(passed ? "PASS" : "FAIL")}");
            }

            public string Describe()
            {
                if (runs == 0)
                {
                    return $"{name}: -  (target {target:0.00}s)";
                }

                string verdict = passed ? "<color=#7CFC00>PASS</color>" : "<color=#FF5050>FAIL</color>";
                return $"{name}: {last:0.000}s  (target {target:0.00}s)  {verdict}  runs {runs}";
            }
        }
    }
}
