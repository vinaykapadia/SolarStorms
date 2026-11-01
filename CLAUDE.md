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

* The folder layout, the assemblies, what each holds, and their references are defined in the phase plan's "Project layout" section, and the broadcast events in its "Structure" section. Follow them, and change them there, not here.
* No system keeps its own copy of a power level. Read it from the reactor model.
* Do not hard-code a tunable number in code unless I have said it is temporary. Tunable values go in ScriptableObject assets.

## Conventions

* Unity 6 with the new Input System package. Input is read through one action map.
* Use EditMode tests for Simulation rules (closer-is-stronger, quick-routing math, quadrant assignment and equalisation).
* Do not edit `.meta` files by hand.
* `Library/`, `Temp/`, and `Logs/` must never be committed.
* Use clear names over clever ones. Keep each class to one job.

