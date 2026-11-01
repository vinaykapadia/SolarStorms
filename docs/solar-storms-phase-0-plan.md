# Solar Storms — Phase 0 Prototype Plan

*Companion to the design doc (claude/solar-winds-successor-design-doc.md, v0.13). Phase 0 has one job: prove that flight, routing, tuning, and one fight feel right together. Nothing else is scheduled until this prototype has been played.*

## Scope in one paragraph

One ship in an empty arena, with Unity 6 and C#. Placeholder shapes for art. No station, no contracts, no save system. Everything the design calls "locked" is in: reactor routing, five-band frequencies with scanning and retuning, four-quadrant shields, and combat state. One enemy group (a Dart) patrols, notices, chases, and returns.

## Build order

Riskiest and most depended-on first. Each step has a "done when" test; do not start the next step until it passes.

| # | Step | What is built | Done when |
| --- | --- | --- | --- |
| 1 | **Flight and shooting** | One ship with direct turning (no drift), a beam, the camera look-ahead A/B toggle | Matches the measured numbers: standstill to full speed 0.5s, full speed to stop 0.5s, 360° turn 1.5s, a shot ship-to-corner 0.75s, 10 shots in 3.3s |
| 2 | **Reactor model** | Four systems on 100 units, D-pad quick-routing, three presets (out of combat only), heat, effects wired to engines and weapons, side-panel bars | A power change is visible on the ship and audible within a second, and no system keeps its own copy of its power level |
| 3 | **Shields** | Four quadrants, shared recharge from Shields power, slow equalisation, hull damage when a quadrant is down, the white outline | A focused pass breaks a side, and turning the broken side away measurably helps |
| 4 | **Frequencies and scan** | Five bands with a switch between the three-band and five-band sets, the closer-is-stronger rule, the first-hit verdict, held-LB D-pad tuning with explicit up/down per band and buffered presses, scanning from the reticle, band tags, empty brackets, stale tags | A tester can name an enemy's bands and say how they knew |
| 5 | **Enemy group and combat state** | One Dart group with home, patrol radius, chase radius, wander, notice, chase, return; the first Dart with fixed bands and later Darts rolled; combat state, combat lamp, travel-mode and preset locks | A fast pilot can shake a pursuer, and the player can say why something is locked |
| 6 | **Playtest gates** | See below | See below |

## Structure, in plain English

**Data assets** (no code changes to tune): ship class, weapon, band damage and recalibration values, combat state values (engagement radius, notice ranges, calm period), and enemy groups (ships and offsets, home, patrol radius, chase radius, hunter flag).

**Per-ship models**, used by the player and by enemies alike:

- **Reactor model:** owns output, routing, and heat. Engines, shields, weapons, and sensors read their effective stats from it every frame.
- **Shield model:** owns the four quadrant pools, the shared recharge, and equalisation. Hit assignment is by incoming direction relative to the ship's facing.
- **Tuning model:** owns the ship's weapon and shield bands, scan state, and recalibration timers, and knows which bands are unlocked.

**One shared combat state**, computed from hostile positions and alert flags, read by travel mode, presets, and the combat lamp.

**Events:** power changes, scan complete, hit result (penetrating, partial, blocked), and combat state changes are broadcast, so the HUD, audio, and logs just listen.

**Input:** one action map for flight, with the tuning layer switched on while the left bumper is held.

## Project layout (decided)

Feature folders under `Assets/_SolarStorms/`, with four layer assembly definitions plus tests:

| Assembly | Holds | References |
| --- | --- | --- |
| **SolarStorms.Simulation** | Reactor, shield, and tuning models; combat state; shared events; ScriptableObject classes. Plain C# classes, no MonoBehaviours. References UnityEngine (decided — not engine-free), so Unity types like Vector2 are allowed. | — |
| **SolarStorms.Gameplay** | MonoBehaviours: ship, flight, weapons, Dart group AI, input actions | Simulation |
| **SolarStorms.Presentation** | HUD, side panel, audio, VFX — listen to events only | Simulation (and Gameplay if needed) |
| **SolarStorms.DebugTools** | Live-tuning overlay, playtest logging | All of the above |
| **SolarStorms.Tests.EditMode** | Unit tests of the rules: closer-is-stronger, quick-routing math, quadrant assignment and equalisation | Simulation |

Nothing references Presentation or DebugTools, so the "HUD, audio, and logs just listen" rule is enforced by the compiler. Tuning `.asset` instances live together in `_SolarStorms/Tuning Data/`; their classes live with their feature. URP settings stay in the template's `Assets/Settings/`. Folders are created only when their first file arrives, because Git does not track empty folders.

## Debug and logging

- A live-tuning overlay for the starting values (damage fractions, recalibration times, buffer, radii, notice ranges, calm period, quadrant equalisation rate).
- A switch between the three-band and five-band sets.
- Logs of routing changes, scans, retunes, hit outcomes, the band seed, quadrant breaks, combat state changes, and how long the left bumper was held.

## 80/20 cuts

Placeholder shapes for ships, no station or contract board, no music, and audio limited to the routing ticks, the five band pitches, and basic hit sounds. If a step threatens to take much longer than planned, cut polish and keep the rule.

## Playtest gates (step 6)

From the design doc:

- A new player hits a moving Dart within one minute.
- In a second fight, the player re-routes power unprompted and can say what they gave up.
- The player turns a damaged quadrant away to protect it.
- Against a Dart with rolled bands, the player scans, tunes within a few seconds, and can say how they knew, without saying "guessed".

Added during planning:

- **Accidental routing:** watch for routing inputs sent when the player meant to tune.
- **Scan homework:** watch for players skipping scans out of boredom rather than choice.
- **Combat lamp:** watch for players who cannot tell why travel mode or presets are locked.
- **Leash dancing:** watch for fights won by dancing on the chase boundary instead of by routing and tuning.
- **Three-band start:** check that the first sessions still feel like tuning rather than luck (Mid weapons cannot be fully blocked there).

## Values to tune first

Equalisation rate, the 0.3s tuning buffer, recalibration times, the closer-is-stronger damage fractions (100/50/25), the engagement radius, notice ranges, the patrol-to-chase ratio, the five-second calm period, scan times on standard and boosted Sensors, and Mid-weapon weighting in the three-band pool.

## Next step before any code

Agree the class structure with the designer in plain English (which classes exist, what each owns, and what events they send), then write scripts one step at a time in the order above.
