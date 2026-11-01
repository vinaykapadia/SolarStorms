# Solar Storms — Phase 0 Prototype Plan

*Companion to the design doc (docs/solar-winds-successor-design-doc.md, v0.13). Phase 0 has one job: prove that flight, routing, tuning, and one fight feel right together. Nothing else is scheduled until this prototype has been played.*

## Scope in one paragraph

One ship in an empty arena, with Unity 6 and C#. Placeholder shapes for art. No station, no contracts, no save system. Everything the design calls "locked" is in: reactor routing, five-band frequencies with scanning and retuning, four-quadrant shields, and combat state. One enemy group (a Dart) patrols, notices, chases, and returns.

## Build order

Riskiest and most depended-on first. Each step has a "done when" test; do not start the next step until it passes.

| # | Step | What is built | Done when | Status |
| --- | --- | --- | --- | --- |
| 1 | **Flight and shooting** | One ship with direct turning (no drift), a beam, the camera look-ahead A/B toggle | Matches the measured numbers: standstill to full speed 0.5s, full speed to stop 0.5s, 360° turn 1.5s, a shot ship-to-corner 0.75s, 10 shots in 3.3s | Done (2026-10-31, PR #1 to develop) |
| 2 | **Reactor model** | Four systems on 100 units, D-pad quick-routing, three presets (out of combat only), heat, effects wired to engines and weapons, side-panel bars | A power change is visible on the ship and audible within a second, and no system keeps its own copy of its power level | Done (2026-11-01, PR #2 to develop) |
| 3 | **Shields** | Four quadrants, shared recharge from Shields power, slow equalisation, hull damage when a quadrant is down, the white outline | A focused pass breaks a side, and turning the broken side away measurably helps | Next |
| 4 | **Frequencies and scan** | Five bands with a switch between the three-band and five-band sets, the closer-is-stronger rule, the first-hit verdict, held-LB D-pad tuning with explicit up/down per band and buffered presses, scanning from the reticle, band tags, empty brackets, stale tags | A tester can name an enemy's bands and say how they knew | — |
| 5 | **Enemy group and combat state** | One Dart group with home, patrol radius, chase radius, wander, notice, chase, return; the first Dart with fixed bands and later Darts rolled; combat state, combat lamp, travel-mode and preset locks | A fast pilot can shake a pursuer, and the player can say why something is locked | — |
| 6 | **Playtest gates** | See below | See below | — |

## Step 1 results (done)

**Result:** all five "done when" targets pass in play, timed by the debug overlay (`FlightTimingReadout`): standstill to full speed 0.5s, full speed to stop 0.5s, 360° turn 1.5s, shot ship-to-corner 0.75s, 10 shots in 3.3s. All 11 EditMode tests pass.

**Decisions made in Step 1**

- **Steering model.** Controller: the left stick's direction is the desired heading, and the ship turns toward it at 240°/s; stick tilt is the throttle. Keyboard: A/D rotate, W thrusts. Fire is RT on the controller and Space on the keyboard.
- **No drift.** Velocity always points along the heading. Acceleration and deceleration are linear.
- **Beam along the heading.** Bolts fire along the ship's heading; twin-stick aiming (right stick) is deferred. Bolts do not inherit the ship's velocity, so every bolt flies at the same speed.
- **4:3 view and 16.67 u/s bolts.** "Screen corner" is defined in world units: a fixed 4:3 game view (left of the side panel) inside a 16:9 layout, letterboxed on other screen shapes. View half-height 7.5 gives a corner distance of 12.5 units, so bolts fly at 16.67 u/s to reach it in 0.75s. Bolt lifetime is 0.8s.
- **0.367s fire interval.** "10 shots in 3.3s" means the first shot at 0s and the tenth at 3.3s, so 3.3 ÷ 9 = 0.367s between shots. Holding repeats; tapping cannot beat the interval; releasing never banks shots.
- **Max speed 8 u/s.** The first value, 13.33 u/s (crossing the game view's width in 1.5s), left bolts only 3.3 u/s faster than the ship. Lowered to 8 u/s in play.
- **Look-ahead on by default, with an F1 toggle.** The camera leads along the ship's velocity, by up to 25% of the view's half-height at full speed, with 0.3s smoothing. Players will be able to turn it off in the settings (design doc Sections 4 and 10); in Phase 0, F1 in the debug overlay toggles it. The ship otherwise stays at the centre of the game view.
- **Engine and beam values are temporary.** They sit at the standard power level in `ShipClass_Kestrel` and `BeamWeapon_Standard` until the Step 2 reactor model supplies them.

**Open items**

- **Settings screen.** None exists yet. The look-ahead player toggle needs one, and settings are not in the MVP table, so they need a phase.
- **Template input asset.** The template's `InputSystem_Actions` is still in the project because Project Settings uses it as the project-wide actions asset. Game code reads only the `Flight` map in `SolarStorms.inputactions`. Remove it once the project-wide setting is cleared.
- **Build Settings.** The scene list still names the deleted `SampleScene`. Arena needs adding before the first build.
- **Bolt collisions.** Bolts move without physics; collisions arrive with Step 3.

## Step 2 decisions (recorded before code)

1. **Power levels.** The reactor's 100 units are shared across 4 systems. Standard is 25 per system (Balanced = 25/25/25/25), the floor is 5, and the cap is 60. Effects are piecewise linear in power: 0.6x at the floor, 1.0x at standard, 1.3x at the cap. These are starting values, held in a ScriptableObject. At 25, every Step 1 number must still pass.
2. **Overheat is a lockout.** At max heat, the beam and boost stop until heat falls below a threshold (a tuning value). The lockout has a clear HUD and audio cue.
3. **Presets are always available until Step 5.** The combat lock check is a stub that always returns "not in combat" until combat state exists.
4. **Only Engines and Weapons have real effects in Step 2.** The Shields and Sensors bars move but do nothing yet.
5. **Top speed stays below bolt speed.** Top speed, including boosted Engines plus boost, is capped below the bolt speed of 16.67 u/s. The cap is a tuning value, starting at about 14 u/s.

Approved with the class structure:

- **One shared curve.** Every effect uses the same power-to-effect curve.
- **Fire rate scales.** The fire interval is 0.367s ÷ the Weapons multiplier: 0.611s at the floor, 0.282s at the cap.
- **Heat dissipation scales with Weapons too.** Firing alone never overheats at any Weapons level, and neither does boosting alone; firing and boosting together overheats in about 6.5s. Starting values: 10 heat per shot, 18 heat per second of boost, 30 heat per second dissipation at standard Weapons, max heat 100, lockout release below 40.
- **Boost is limited by heat only.** No boost meter and no separate recovery. Boost multiplies top speed by 1.35.
- **Time to full speed stays 0.5s.** Engines scales top speed and acceleration together.
- **Hold repeat.** A routing input repeats after 0.3s, then every 0.1s.
- **Whole units.** Leftover units are taken in the fixed order Weapons, Shields, Engines, Sensors.
- **Bolts carry a snapshot.** Each bolt stores its damage and intensity when fired; this is a result of the shot, not a copy of a power level.
- **uGUI** for the placeholder side panel.
- **Presets on debug keys** F2, F3 and F4 (Balanced, Combat, Burn). The player-facing preset input is open in the design doc.
- **Input asset.** Clear the project-wide actions setting and delete the template's `InputSystem_Actions`, so `SolarStorms.inputactions` is the only input source.

## Step 2 results (done)

**Result:** the "done when" test passes in play, and all 57 EditMode tests pass.

- **A power change is visible and audible within a second.** A routing tap moves the side-panel bar, plays that system's tick, and changes the engine glow or bolt brightness on the same frame (logged by `ReactorReadout`).
- **No system keeps its own copy of its power level.** `ReactorModel` is the only owner; `FlightModel` and `BeamFireTimer` read the Engines and Weapons multipliers from it every step.
- **Step 1 numbers still pass at standard.** At Balanced, the Step 1 overlay shows all five targets passing, and the Step 1 tests run against a reactor at Balanced.
- **Heat targets hold.** Holding fire at standard Weapons never overheats (heat peaks at about 9.4). Firing with Weapons at the cap and boosting overheats in about 6.5s; firing with boost at standard, in about 6.3s. A lockout ends about 2s later.
- **Speed cap holds.** Top speed never passes 14 u/s at any Engines level with boost.

**Decisions:** as listed under "Step 2 decisions" above. The design doc records the power levels, heat and overheat, the speed cap, and the preset values (Sections 4 and 5, and the decisions table).

**What exists now**

- **Simulation:** `ReactorModel` (allocations, heat, lockout, and the PowerChanged, HeatChanged, OverheatStarted and OverheatEnded events), `QuickRouting`, `PowerCurve`, `HoldRepeater`, `PowerChange`, the `CombatState` stub, and the `ReactorSystem` enum. Data: `ReactorData`, `ReactorPresetData`, `ReactorFeedbackData`. `FlightModel` and `BeamFireTimer` now take a reactor; `CameraRigData` holds the game-view and side-panel layout maths.
- **Gameplay:** `ShipReactor`, `PlayerRoutingInput`; the `Flight` map gains Boost and the four routing actions; bolts carry damage and intensity.
- **Presentation:** `SidePanelView` (uGUI, built in code), `EngineGlowView`, `BoltTint`, `ReactorAudio` with `PlaceholderTones`.
- **DebugTools:** `ReactorReadout` (presets on F2/F3/F4, time from zero heat to lockout, power-change log).
- **Assets:** `Reactor_Starter`, `ReactorFeedback_Placeholder`, and the Balanced, Combat and Burn presets in `Tuning Data/`.

**Resolved from Step 1:** the template's project-wide `InputSystem_Actions` is removed, and Arena is in Build Settings.

**Open items**

- **Preset input.** No player-facing input applies a preset yet; the design doc marks it open, with the power panel as the likely home.
- **"Heat as frustration" risk.** The design doc's mitigation says cooling should not be "a forced pause", but the lockout stops the beam and boost for about 2s. The risk text needs reviewing against the lockout decision.
- **Floor Weapons and boost.** At floor Weapons, boost heat exactly matches dissipation, so boosting alone builds no heat there.
- **Shields and Sensors** bars move but have no effect until Steps 3 and 4.
- **Combat state** is a stub until Step 5, so presets are always available.
- **Engine glow** follows Engines power only, not boost.
- **Build Settings** still lists the deleted `SampleScene`, disabled.
- **Unsaved new fields.** `ShipClass_Kestrel` does not yet store `boostSpeedMultiplier`, `boostHeatPerSecond` or `speedCap`, and `BeamWeapon_Standard` does not store `heatPerShot` or `damageAtStandard`. Both run on the code defaults, which are the intended values; saving each asset once in the editor writes them out, so a later change to a default cannot silently change play.
- **Carried from Step 1:** the settings screen and bolt collisions.

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

This section is the single source for the project layout and assembly references. Feature folders sit under `Assets/_SolarStorms/`, with one assembly definition per layer, and EditMode tests sit in `Assets/Tests/EditMode/`:

```
Assets/
  _SolarStorms/
    Simulation/      (asmdef: SolarStorms.Simulation)
    Gameplay/        (asmdef: SolarStorms.Gameplay)
    Presentation/    (asmdef: SolarStorms.Presentation)
    DebugTools/      (asmdef: SolarStorms.DebugTools)
    Tuning Data/     (ScriptableObject .asset instances)
    Prefabs/  Scenes/  Art/  Audio/
  Tests/EditMode/    (asmdef: SolarStorms.Tests.EditMode)
  Settings/          (URP, from the template; do not change)
```

| Assembly | Holds | References |
| --- | --- | --- |
| **SolarStorms.Simulation** | Reactor, shield, and tuning models; combat state; shared events; every ScriptableObject class, including data only one layer reads (such as camera and starfield values). Plain C# classes, no MonoBehaviours. References UnityEngine (decided — not engine-free), so Unity types like Vector2 are allowed. | UnityEngine only |
| **SolarStorms.Gameplay** | Gameplay MonoBehaviours: ship, flight, weapons, Dart group AI, input actions | Simulation, Unity.InputSystem |
| **SolarStorms.Presentation** | Camera, HUD, side panel, audio, VFX. HUD, audio, and VFX listen to events only | Simulation, Gameplay, UnityEngine.UI |
| **SolarStorms.DebugTools** | Live-tuning overlay, playtest logging, timing readouts | Simulation, Gameplay, Presentation |
| **SolarStorms.Tests.EditMode** | Unit tests of the Simulation rules: flight timings, fire rate, closer-is-stronger, quick-routing math, quadrant assignment and equalisation | Simulation (plus the Unity Test Framework) |

Nothing references Presentation or DebugTools, so the "HUD, audio, and logs just listen" rule is enforced by the compiler. Never add a reference this table does not list, or one that creates a cycle. All ScriptableObject classes live in Simulation, so every layer can read every data asset; their `.asset` instances live together in `_SolarStorms/Tuning Data/`. URP settings stay in the template's `Assets/Settings/`. Folders are created only when their first file arrives, because Git does not track empty folders.

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
