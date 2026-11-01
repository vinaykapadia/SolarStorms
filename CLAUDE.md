# Solar Storms: guide for Claude Code

A top-down, multidirectional space shooter. Unity 6, C#. Original work, inspired by Solar Winds (1993). Do not use any original Solar Winds characters, text, art, music, or assets.

## Source documents

* `docs/solar-winds-successor-design-doc.md` is the design source of truth (v0.13). If code and doc disagree, stop and ask.
* `docs/solar-storms-phase-0-plan.md` is the build plan. Follow its build order.

## How we work

* Implement only the step I name. Build in the order in the phase plan, and do not start a step until the previous step's "done when" test passes.
* For each step, first propose the class structure in plain English: which classes exist, what each owns, which events it sends, and which assembly each file goes in. Wait for my approval before writing scripts.
* Flag design conflicts or open questions instead of deciding them. Options are welcome; decisions are mine.
* Keep scope tight. If something looks like a rabbit hole, say so and propose the smaller version.

## Architecture

* Reactor, shield, and tuning models are plain C# classes in `SolarStorms.Simulation`. They are not MonoBehaviours.
* MonoBehaviours live in `SolarStorms.Gameplay`.
* HUD, audio, and VFX live in `SolarStorms.Presentation` and only listen to events.
* Assembly references: Simulation references UnityEngine. Gameplay references Simulation and Unity.InputSystem. Presentation references Simulation and Gameplay. DebugTools references all three. Tests.EditMode references Simulation. Never add a reference that creates a cycle.
* No system keeps its own copy of a power level. Read it from the reactor model.
* Tunable values live in ScriptableObjects under `Assets/_SolarStorms/Tuning Data/`. Do not hard-code a tunable number in code unless I have said it is temporary.
* Events are broadcast for: power changed, scan complete, hit result, combat state changed.

## Layout

```
Assets/
  _SolarStorms/
    Simulation/      (asmdef: SolarStorms.Simulation)
    Gameplay/        (asmdef: SolarStorms.Gameplay)
    Presentation/    (asmdef: SolarStorms.Presentation)
    DebugTools/      (asmdef: SolarStorms.DebugTools)
    Tuning Data/
    Prefabs/  Scenes/  Art/  Audio/
  Tests/EditMode/    (asmdef: SolarStorms.Tests.EditMode)
  Settings/          (URP, do not change)
```

Create a folder only when its first file goes in. Git does not track empty folders.

## Conventions

* Unity 6 with the new Input System package. Input is read through one action map.
* Use EditMode tests for Simulation rules (closer-is-stronger, quick-routing math, quadrant assignment and equalisation).
* Do not edit `.meta` files by hand.
* `Library/`, `Temp/`, and `Logs/` must never be committed.
* Use clear names over clever ones. Keep each class to one job.

