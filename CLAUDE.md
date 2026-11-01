# Solar Storms: guide for Claude Code

A top-down, multidirectional space shooter. Unity 6, C#. Original work, inspired by Solar Winds (1993). Do not use any original Solar Winds characters, text, art, music, or assets.

## Source documents

* `docs/solar-winds-successor-design-doc.md` is the design source of truth. If code and doc disagree, stop and ask.
* `docs/solar-storms-phase-0-plan.md` is the build plan. Follow its build order.

## How we work

* Implement only the step I name. Build in the order in the phase plan, and do not start a step until the previous step's "done when" test passes.
* For each step, first propose the class structure in plain English: which classes exist, what each owns, which events it sends, and which assembly each file goes in. Wait for my approval before writing scripts.
* Flag design conflicts or open questions instead of deciding them. Options are welcome; decisions are mine.
* Keep scope tight. If something looks like a rabbit hole, say so and propose the smaller version.
* At the end of a step, update that step's Status and results in the phase plan. Any design change goes into the design doc (including its decisions table) in the same PR as the code.
* Before reporting a step done, run the EditMode tests and check the step's "done when" numbers in play.
* End each step with a summary in this format: results vs "done when", decisions made, what exists now, open items for later steps.

## Git

* One branch per step, named `phase-0-step-N`, cut from `develop`.
* Open a PR into `develop`.
* Never commit to `main`.
* Never force-push a shared branch unless I ask.

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

