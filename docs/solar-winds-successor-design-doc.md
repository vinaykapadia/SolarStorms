# GAME DESIGN DOCUMENT

## Solar Storms: A Modern Spiritual Successor to Solar Winds

*"Solar Storms" is the game's chosen title. The game itself is an original title inspired by the feel and structure of Solar Winds (1993), not a remake, sequel, or licensed product.*

| Item | Current direction |
| --- | --- |
| **Genre** | Top-down, multidirectional space shooter with mission-based progression |
| **Fantasy** | Independent pilot taking risky jobs on the edge of a system that is hiding something |
| **Platforms** | PC first; Xbox Series X\|S as the natural second platform |
| **Engine / language** | Unity / C# — proposed, to be confirmed in prototyping |
| **Players** | Single-player for the MVP and first release scope |
| **Session length** | 15–40 minutes — one to three jobs plus time at the station |
| **Core system (required)** | Reactor energy management — routing a finite reactor output between ship systems, in flight |
| **Core system (required)** | Shield and weapon frequencies — five readable bands on a line (Infra, Low, Mid, High, Ultra), closer-is-stronger, rolled per ship with the contract and scanned and tuned in flight, never unknowable; the outer two bands unlock with the first Tuner upgrade |
| **Shields (required)** | Four quadrants — fore, aft, left, right — on the single Shields reactor system; each takes its own damage, recharges from Shields power, and slowly equalises |
| **Damage Control** | Not a routable system — a passive upgrade and station repair function only; hull and system repair is not a reactor routing choice in flight |
| **Status** | Design draft v0.13 — pre-prototype; reactor routing and frequencies are locked requirements; Damage Control is not a routable system (passive upgrade and station repair only), frequencies use five bands on a line named Infra, Low, Mid, High, and Ultra with a closer-is-stronger rule, the player starts with Low, Mid, and High and the first Tuner upgrade unlocks Infra and Ultra, shields have four quadrants on one Shields system, and the power panel opens on a press and slows the action; retuning uses a held-LB D-pad layer with an explicit up and down input for each band; contracts are parameterised templates plus a few authored story jobs; every enemy ship has its own randomly rolled weapon and shield bands (fixed on the two tutorial fights), so every ship is scanned, and a scanned ship shows its shield band on a small tag beside it; a single combat state (hostile within a fixed radius, or alert) locks travel mode and presets; enemies are placed as groups with a home point, a patrol radius, and a chase radius |

> **Design intent in one line:** If the loop — fly, fight, scan, tune, route power, get paid, upgrade, take a bigger job — does not feel good in one small star system, more content will not save it. Prove the loop, and the reactor and frequencies at its centre, first.

---

# 1. Vision and high concept

You are a freelance pilot with a modest ship, a thin credit balance, and a station bulletin board full of people willing to pay for work they cannot — or will not — do themselves. Some jobs are honest hauling. Some are bounty hunts. Some look routine until the cargo, the target, or the person who hired you turns out to be part of something larger.

That is the heart of the original inspiration: a small pilot, a working ship, and a mystery that widens one job at a time. Solar Storms keeps that structure and rebuilds everything around it — its hero, factions, star systems, ships, dialogue, art, and story are original to this project.

The game should feel immediate in the first sixty seconds. The player launches, turns, boosts, and fires without a tutorial wall. Depth arrives through loadout choices, tougher contracts, and story jobs that test skills the early game taught quietly.

One system sits at the centre of that depth, and it is a requirement, not an optional feature: reactor energy management. The ship's reactor never produces enough power to run everything at its best at once, so the player is always deciding — in flight, in the middle of a fight — what the ship is good at right now. That constant, hands-on trade-off is the most distinctive part of the original game's feel, and Solar Storms is built around a modernised version of it (see Section 5). If power routing were removed, this would no longer be the game this document describes.

A second requirement sits alongside it: shield and weapon frequencies. In the original, picking a frequency felt like a crapshoot — the right choice was hidden, so success looked like luck. Solar Storms keeps frequencies and removes the guessing: five readable bands, one closer-is-stronger rule, and a sensor scan that tells the player each ship's bands before they have to commit (see Section 6). Every ship carries its own bands, so the scan is something the player does to every ship, not a lesson learned once. If frequencies were removed, or left as a hidden guess, this would also no longer be the game this document describes.

## The experience we are selling

**Freedom with a job to do.** Open space to explore, but always a clear next contract, destination marker, and payoff.

**Skill you can feel.** Fights are won by positioning, burst discipline, missile timing, power routing, and tuning — not by stat checks alone.

**A reactor you actually run.** Survival often turns on a routing call made in seconds: power to shields to weather a pass, to engines to break away, or to weapons to finish a target before it finishes you.

**Frequencies you can read.** A scan names each enemy's shield and weapon bands, you retune your beam onto their shield band and your shield away from their beam — and the first hit tells you, in words, how close you got.

**A ship that becomes yours.** Upgrades visibly change how the starter ship flies and fights — and a bigger, more efficient reactor changes every routing decision, and a better tuner opens the outer bands — before a second hull is ever offered.

**A mystery, not lore dumps.** Short briefings, intercepted messages, and contradictory orders reveal the conspiracy in playable pieces.

# 2. Design pillars

Every feature is judged against these seven pillars. If a feature weakens one pillar and strengthens none, it is cut or deferred. The reactor and frequency pillars are locked: neither can be traded away to satisfy another pillar.

| Pillar | What it means | Test in play |
| --- | --- | --- |
| **Direct flight** | Turn and you go; minimal drift | Matches the original's measured feel: full speed to stop in 0.5s, standstill to full speed in 0.5s, a 360° turn in 1.5s, a shot from ship to screen corner in 0.75s, and 10 shots in 3.3s |
| **Power is a decision** | A finite reactor forces live trade-offs between four systems, each worth routing power to or from | Player re-routes power mid-fight, unprompted, in the first session |
| **Tuned, not guessed** | Every ship's frequency bands can be found out in flight — scanned, signed, and shown — never hidden or unknowable | Player can name the target's shield band and say how they found it and why a hit was penetrating, partial, or blocked |
| **Earned progression** | Pay, parts, and reputation come from jobs | Player can explain what the next upgrade will change |
| **Readable combat** | Threats, damage, and intent are clear on screen | Player knows why they took damage after every hit |
| **Unfolding conspiracy** | Story advances through contracts | Each story job ends with a new question, not just a payout |
| **Looks like Solar Winds** | Visual style stays unmistakably the original's — metal-framed side panel, pixel font, sprite ships, sparse starfield — with higher-resolution sprites as the main upgrade | Someone who knows the original recognises a screenshot as Solar Winds at a glance |

## What we keep, modernise, and leave behind

**Keep:** ship-centred top-down view, immediate steering matched to the original's measured flight feel, the original's visual style (metal frame, pixel font, sprite ships), beam burst options, limited tracking missiles, mission variety, interlocking systems — and, above all, managing reactor energy and choosing which systems that energy feeds, and tuning shield and weapon frequencies.

**Modernise:** the reactor itself (four systems instead of the original's longer list, each worth routing power to or from; out-of-combat presets and one-tap quick-routing, with a power panel that slows the action — see Section 5), pacing (a denser galaxy and a faster travel mode that runs independently of the reactor and cannot be engaged in combat — see Section 4), and the frequencies themselves (five named bands on a line instead of an opaque dial, a sensor scan on every ship instead of a guess, and bands rolled once with the contract instead of hidden), twin-stick and controller support, save system, quest log and star map, difficulty options, accessibility settings, and clear combat feedback. The routable systems are a new set chosen for modern play, not a copy of the original game's list.

**Leave behind:** DOS-era guesswork — above all, frequency choices with no way to learn the right answer — punishing lack of saves, opaque systems with no explanation, pausing the action to drag sliders (the Section 5 power panel slows the action rather than pausing it), and any direct reuse of the original game's characters, story, art, or assets.

# 3. Core loop

The core loop is deliberately small enough to prototype in full:

Accept a contract at the station (and read its enemy intel) → Set a power preset and hedging starting bands for the job → Launch and travel → Scan each target, tune your beam onto its shield and your shield away from its weapon, fight, evade, or protect, re-routing power as the situation changes → Complete the objective → Return, get paid, repair and upgrade (the reactor first) → Choose a harder or stranger job.

## Moment-to-moment (seconds)

Steer, boost in short bursts, route reactor power between engines, shields, weapons, and sensors & targeting, scan each new ship until its bands are named, retune your weapon and shield bands, choose a burst mode for the target in front of you, and save missiles for targets that matter. Enemies telegraph attacks so the player is always making a decision, not just holding fire — and the most frequent decisions are where the power goes next, and whether the ship is tuned for the ship it is facing.

## Session loop (minutes)

Pick one to three contracts that share a region of the system when possible, chain them efficiently, and come home with enough credits to feel a choice: repair now, upgrade now, or save for the bigger part. Different jobs reward different presets — a cargo run wants engines and sensors, an escort wants shields and weapons — so planning a session is partly planning power. It is also partly planning for the scan: a contract that expects three ships is a different launch from one that expects one, even at the same pay, because every extra ship is another scan, another tune, and another chance that no shield band blocks everything. That choice is the session's closing beat.

## Long-term loop (hours)

Raise reputation with factions, unlock gated contracts and equipment, grow the reactor so hard trade-offs become wider choices rather than disappearing, open the outer frequency bands with the Tuner, piece together who is manipulating the job board, and prepare the ship — and the pilot — for leaving the starter system.

# 4. Flight and combat

## Flight model

**Ship-centred camera.** The player's ship stays near the centre of the screen and the world scrolls around it, matching the original feel. Slight look-ahead in the direction of travel keeps fast flight readable. Decided in the Phase 0 play test: look-ahead is on by default, and players can turn it off in the settings (Section 10). The camera leads along the ship's velocity, by up to a quarter of the game view's half-height at full speed; that distance and its smoothing are tuning values.

**Immediate turning.** Heading follows input directly, in an arcade style with tight, direct control. There is no inertial drift: the ship starts and stops quickly. Targets, matched to the original's measured feel: from standstill to full speed in 0.5s, from full speed to a stop in 0.5s, and a full 360° turn in 1.5s.

**Shooting feel.** A shot travels from the ship (screen centre) to the screen corner in 0.75s, and firing 10 shots takes 3.3s, matching the original's measured feel. Hit feedback in the original was sound, and it stays a core channel (see Section 10).

**Speed layers.** Cruise for combat, boost for repositioning and escape (limited by the power routed to engines and by reactor heat), and a faster travel mode for crossing empty distance. Travel mode runs independently of the reactor (it does not draw reactor power) and cannot be engaged in combat state (below). If a hostile triggers combat state while the ship is in travel mode — an ambush on a long cargo leg, for instance — the ship drops out of travel mode immediately and slows to cruise, with a warning tone and a flash on the combat lamp. A ship starved of engine power is visibly slower; routing power there is how the player runs, chases, or closes in. Top speed, even with Engines at its cap and boost held, is capped below beam bolt speed, so the player's own shots always pull ahead of the ship (the cap is a tuning value; it starts at 14 u/s against 16.67 u/s bolts).

**Combat state.** The game tracks one simple state, used by travel mode, presets, and anything else that needs to know whether the player is in a fight. The player is in combat state when at least one hostile ship is either within a fixed engagement radius or alert, meaning it has noticed the player.

- **The radius is fixed.** It does not scale with Sensors power, so a Burn preset cannot widen or shrink it. It is set a little beyond the edge of the game view, so a hostile the player can see always counts.
- **Becoming alert.** A hostile becomes alert when the player comes within its notice range (a per-class tuning value), when it fires on the player, or when the player attacks it.
- **Ceasing to be alert.** An alert hostile stays alert only while the player is inside its group's chase radius (see Enemy groups, below). The moment the player is outside it, the group disengages and starts back toward its home, so a pilot who runs far enough on Engines can actually get away.
- **What does not matter.** Whether the player has scanned a ship makes no difference: the state follows what the ship is doing, not what the player knows. Neutral and friendly ships, including an escorted ship, never count.
- **Entering and leaving.** Combat state starts the moment a hostile qualifies and ends only after about five seconds with no qualifying hostile, so it does not flicker at the edge of the radius.
- **What it locks.** Travel mode cannot be engaged, and presets cannot be applied (Section 5). Manual routing, quick-shifts, the power panel, retuning, and docking are never locked by it.

A small combat lamp on the side panel shows the state, so the player always knows why travel mode or a preset is unavailable (Section 10). [Open: the engagement radius, per-class notice ranges, the patrol and chase radii of enemy groups, and the five-second calm period are all starting values for the prototype; they are the first things to tune.]

**Collision that teaches.** Bumping asteroids and debris costs a little shield and momentum, not a run. Ramming at boost speed costs more, so the risk is legible.

## Weapons

| System | How it works | Player decision |
| --- | --- | --- |
| **Beam — single burst** | Low heat, precise, best vs. small / fast targets | Accuracy and heat control |
| **Beam — double burst** | Balanced damage and heat; the default workhorse | General-purpose fighting |
| **Beam — triple burst** | Highest damage, fastest heat build-up, wider spread | Close, committed attacks |
| **Tracking missiles** | Limited ammunition, lock-on after a short hold, strong vs. armoured targets | When is this target worth a missile? |
| **Defensive slot (post-MVP)** | Flare, shield pulse, or mine — one slot, swappable at station | Build for the job ahead |

Weapons do not run on their own. Beam damage, burst recovery, and heat dissipation all scale with the power routed to Weapons (Section 5): a starved beam is weak and slow to recover, a well-fed beam is the ship's main argument. Every beam also fires on a frequency band, and every shield is tuned to one (Section 6): power decides how hard a beam hits, the band decides how much of that hit the target's shield lets through. Tracking missiles are physical, not energy, so frequencies do not affect them — which is what makes a missile the reliable answer to a target the player has not tuned for yet, at the cost of a scarce round. Reactor heat is the pacing system — firing and boosting both build it, and shedding it is part of routing, not a separate mini-game.

The combat HUD therefore shows, at all times: the four reactor bars, reactor heat, the four shield quadrants (fore, aft, left, right) shown around the ship, hull, and missile count — plus the player's own weapon and shield bands, and, for the locked or scanned target, its two bands and the current effectiveness word (Penetrating, Partial, or Blocked, Section 6). Nothing about power or tuning is hidden in a menu during a fight.

## Enemies and damage

Every enemy ship carries its own weapon band and its own shield band, rolled when the contract is built (Section 6), so two Darts in the same fight can be tuned to different bands. The class defines how the ship flies and fights; the scan tells the player how to hurt it.

**Dart (MVP).** Fast, fragile, attacks in short strafing passes. Teaches leading targets and single-burst discipline. The first Dart in the game is authored with fixed bands (weapon Low, shield Mid, Section 6), so the player can learn the frequency rule on a ship whose answer is clean: a beam set to Mid — matching its shield — and a shield set to High — two or more steps from its weapon, and the only such band the player can reach at the start (Ultra also works once unlocked) — solves it. Every Dart after that carries its own rolled bands.

**Mauler (MVP).** Slow, armoured, fires heavy bolts. Teaches circling, double/triple bursts, and missile value. The first Mauler in the game is also authored with fixed bands (weapon High, shield Low), the opposite problem from the first Dart: the attacking beam must be Low to penetrate, not the Mid that penetrated the Dart, and a shield on Low (or Infra once unlocked) cuts its bolts to a quarter damage. That contrast is deliberate: it is the fight that teaches tuning. Every Mauler after that carries its own rolled bands.

**Post-MVP additions.** A carrier that launches Darts, a sniper that fires a telegraphed long-range beam, and a faction ace with player-like boost and burst behaviour — added one at a time, each teaching one new response.

**Damage model — shield quadrants.** Every ship's shield is divided into four quadrants — fore, aft, left (port), and right (starboard) — and each quadrant is its own damage pool. A hit is assigned to the quadrant facing the incoming fire, and only that quadrant takes the damage; a hit on a depleted quadrant passes through to the hull. All four quadrants belong to the single Shields system on the reactor — they are not separate routable systems and have no separate power allocations. Shields recharge after a few seconds out of fire at a rate set by the one power allocation routed to Shields, shared across all four quadrants, and the quadrants also slowly equalise: strength bleeds from the strongest quadrant toward the weakest over seconds, so a battered side recovers partly at the expense of the others. Equalisation is a slow trickle, not a rescue — fast enough that a side is not dead forever, slow enough that focused fire can still break a quadrant before it equalises away — and its exact rate is a tuning value for the prototype. Facing therefore becomes a combat decision: after a bad pass the pilot turns the damaged quadrant away and presents a fresh side while it recharges and equalises. Routing away from Shields to run or shoot is a real, visible gamble on all four sides at once.

Hull does not recharge on its own in flight, and hull or system damage is not repaired in flight: it lasts until the station, where it is repaired fully and fast. Damage Control is not a reactor system — it is a passive upgrade and station-side function (see Section 5) that improves station repair, not a routing choice made in flight. Damage in a fight is therefore one-way pressure: the in-flight answers are shields, facing, and getting out, not feeding a repair system. Destruction returns the player to the station with a repair bill and lost contract progress — never lost upgrades or lost story progress.

## Shield quadrants — one system, four sides

**Four pools, one system.** Fore, aft, left, and right are damage pools, not reactor systems. There is one Shields allocation, one recharge rate, and one shield band for the whole ship; no input routes power to an individual quadrant.

**Facing is defence.** A hit lands on the quadrant facing the shooter, so turning is a defensive action: present a strong quadrant, protect a weak one, and circle an enemy to keep hitting the side already broken.

**Equalisation, not instant repair.** Quadrants slowly equalise toward each other on top of recharge. Equalisation never creates shield strength — it only spreads what the shield has — so total strength still depends entirely on the power routed to Shields. Repair stays separate: hull and system repair happens at the station, not in flight, and station repair does not refill quadrants mid-fight.

**A broken side is an opening.** When a quadrant reaches zero, hits from that direction bypass the shield and damage the hull until the quadrant has recharged above zero. The frequency rule does not apply on that side while it is down, because there is no shield there to penetrate or to block with.

Enemy ships run reactors too, and their routing is readable: a Dart glows at the engines and fires in quick, light bursts (engines and weapons favoured, shields thin), while a Mauler sits behind bright, fast-recovering shield quadrants and hits hard but turns slowly. Enemy quadrants work exactly as the player's do, and a depleted enemy quadrant shows as a missing section of its shield outline once the enemy has been scanned — an opening to attack through. Learning to read an opponent's routing — and to punish the system they starved — is a core combat skill. The same applies to their tuning, with an asymmetry: an enemy's beam colour always truthfully shows its weapon band the moment it fires, so what is coming at the player is readable at a glance, but its shield band is shown only after a scan, as a small band tag beside the ship (Sections 6 and 10). The player sees what a ship shoots; they have to scan what it wears.

## Enemy groups — home, patrol, chase

Enemies are placed as groups of one to three ships (the MVP cap on ships in a fight). Each group has three pieces of data, all set by the contract (Section 7):

- **Home.** A point in space.
- **Patrol radius.** The circle around home inside which the group wanders, flying between random points. This is where the player can expect to find it.
- **Chase radius.** A larger circle around home. While the group is alert and the player is inside it, the group chases. When the player is outside it, the group disengages, flies back toward home, and resumes patrol.

Ships in a group share the home and both radii, with small offsets so they do not stack. The chase radius is the same one that ends alertness in combat state (above), so there is one leash rule, not two.

**Rules that keep the leash honest**

- **Damage is kept.** A returning group recharges its shields normally, but its hull does not recover. Luring a group out and letting it reset is never a free heal.
- **Hunters are different.** Ambushers and escort attackers go after a target (the cargo ship, the escorted ship) instead of guarding a point. They are marked as hunters in their data, and their chase radius covers the whole route.
- **Fleeing targets.** A bounty target with the flee twist ignores the leash and heads for an exit point.
- **Ranged enemies.** A post-MVP sniper must have a chase radius larger than its weapon range, or a player could shoot it from outside its own leash.
- **Search areas.** A bounty's briefing marks the target's patrol circle on the star map. Confirmed intel draws it accurately; an unconfirmed report can draw it offset or too large (Section 7). Patrol circles the player has learned about stay drawn on the star map and minimap, so they can be avoided or sought out.
- **Scouting.** The aim, as a tuning goal and not a promise, is that a scan on boosted Sensors can finish from outside a ship's notice range, so a sensors-heavy approach lets the player look before being seen.

**The MVP version.** Random wander inside the patrol circle, noticing the player at the class notice range, chasing up to the chase radius, and returning home. Waypoint patrol routes, schedules, sleeping groups, and reinforcements are post-MVP.

[Open: the ratio between patrol and chase radii, per-class notice ranges, and the wander timing are all starting values to tune in the prototype.]

# 5. Reactor and power routing — the signature system

> **Requirement — locked, not open:** Reactor energy management is a core, non-negotiable system in Solar Storms. It is in the prototype and the MVP, it cannot be cut for scope, and no other system may be designed in a way that makes routing irrelevant. What is open to change is the exact routable-system list beyond the MVP set and the tuning — never whether the system exists.

In the original game, the lasting pleasure was not only shooting — it was running the ship: deciding, moment by moment, where a limited supply of energy should go. Solar Storms keeps that pleasure and rebuilds the machinery for a modern controller, a modern HUD, and a player who should never have to pause a fight to drag a slider.

## How it works

The reactor produces a finite output (100 units on the starter reactor). The player distributes that output between the ship's four systems, each shown as its own bar on the side panel (Section 10). The total routed can never exceed the output, so feeding one system means starving another — every gain is bought with a named loss. Routing can be changed at any time in flight, takes effect within a second, and has no cooldown and no change cost: the skill is in the timing and the call, not in menu speed.

Each system has a floor, a standard level, and a boosted level. At the floor the system barely functions; at standard it behaves as the base stats elsewhere in this document describe; boosted, it performs noticeably beyond base. Exact numbers are a tuning task for the prototype, but the differences must be feelable without reading a tooltip — speed, shield recovery, beam strength, and lock speed the player can see change. Engines at the standard level reproduces the original's measured flight feel (Section 4): below it the ship is slower and less responsive, above it faster and snappier.

**Starting values (decided for the prototype).** The 100 units start Balanced at 25 per system, which is the standard level. Each system's floor is 5 and its cap is 60. Effects are piecewise linear in power: 0.6x at the floor, 1.0x at standard, and 1.3x at the cap, on one curve shared by every effect. Engines power scales top speed, acceleration, and turn rate together, so the time to reach full speed is the same at any Engines level. Weapons power scales the fire rate (the interval is divided by the multiplier), heat dissipation, and beam damage. All of these are tuning values.

**Heat and overheat.** Firing builds heat per shot and boosting builds heat per second; heat sheds continuously, faster with more Weapons power. At maximum heat the reactor locks out: the beam and boost stop until heat falls below a release threshold (a tuning value), with a clear HUD and audio cue for the lockout and its end. Starting values are tuned so that holding fire at standard Weapons never overheats, while firing with Weapons at its cap and boosting overheats in roughly 6 to 8 seconds. Boost has no separate meter or recovery: heat is its only limit.

## Routable systems — the modernised set

This set is deliberately not the original game's list. It is chosen so that every system answers a question the player is already asking in flight:

| System | More power gives | Starving it costs |
| --- | --- | --- |
| **Engines** | Top speed, turn response, boost strength and recovery | You cannot chase, flee, or reposition — you are a target |
| **Shields** | Faster recharge of all four shield quadrants (fore, aft, left, right) and faster equalisation between them | Quadrants stay down longer and equalise slowly; a broken side stays open and hits reach the hull sooner |
| **Weapons** | Beam damage, burst recovery, and heat dissipation | Weak, slow beams; long, dangerous pauses between bursts |
| **Sensors & targeting** | Scanning radius (how far scanned ships stay live), missile lock speed, aim assistance, frequency scan speed and range | A small scanning radius (scanned ships go stale), slow locks, slow or short-range band scans, ambushes you never saw coming |

Sensors stay relevant after the first scan. Every ship is scanned once, and its data stays live while it is inside the scanning radius (Section 10). Fights with several enemies at different distances reward a wide radius, because a ship that slips outside it goes stale, and missile lock speed and aim assistance also scale with Sensors power. The player decides how much power to keep on Sensors once the scans are done.

The MVP ships with these four systems, and only these four are routable. Each maps to one direction on the controller's D-pad (see Section 11), with no diagonal input. Damage Control was considered as a fifth routable system and cut from routing: as a slow repair trickle it was a solved choice — starved in battle to feed combat systems, fed outside battle when there was no real opportunity cost — so it did not earn a reactor slot. One further system is designed now and added after the MVP, once the four-system game is proven: Cooling / radiators, which sheds reactor heat faster and lets the ship sustain firing and boosting longer. Adding it later widens the decision; it does not change its nature.

## Damage Control — passive, not routed

Damage Control remains in the game, but not on the reactor. It is a passive upgrade and a station repair function: at the station it governs how fully, how fast, and how cheaply hull and damaged systems are restored, and its upgrade track (Section 9) improves that station-side repair and the ship's resilience fittings. It has no power allocation, no preset share, no quick-shift input, and no effect that repairs hull or systems in flight. A damaged ship fights damaged, breaks off, or limps home on Engines — repair begins when it docks.

## Routing without the fiddle — presets and quick-routing

The DOS-era failure mode to avoid is management as homework: long slider sessions, optimal settings solved once and never touched again. Three modern layers prevent it:

**Presets are a preparation tool.** A preset sets the sliders to a saved configuration in one action, so the player does not have to adjust each system by hand before a job. Presets are available only out of combat, meaning when the player is not in combat state (Section 4); in combat the pilot manages power by hand, and the preset control shows that it is locked and why. Three presets ship in the MVP — Balanced (safe all-round, the launch default), Combat (weapons and shields favoured), and Burn (engines and sensors favoured, for chasing, fleeing, and hauling against the clock). Starting values: Balanced 25/25/25/25; Combat Weapons 35, Shields 35, Engines 15, Sensors 15; Burn Engines 35, Sensors 35, Weapons 15, Shields 15. Post-MVP, players can save their own presets at the station. [Open: the input that applies a preset. The controller and keyboard tables (Section 11) have none yet; the power panel is the likely home. The prototype uses debug keys.]

**Quick-routing is the in-fight tool.** Tapping a D-pad direction bumps that system. Each bump takes X points from each of the other three systems (X starts at 1 and is tuned in testing) and adds 3X to the chosen system, so the total never changes. The amount moved is the smallest of 3X, the room left in the chosen system, and the points the other three can give up before reaching their floors; it is taken from the other systems as evenly as possible, skipping any at their floor, and which system supplies any leftover points does not matter, so the implementation can use random or fixed order. Holding the direction repeats the bump. Every routable system has its own cardinal direction — there is no diagonal quick-shift. A short audio tick and the shifting bars on the side panel confirm the change, so routing is done by feel, eyes on the fight. While the tuning modifier (LB on a controller) is held, the same D-pad directions tune frequencies instead of routing power (Sections 6 and 11).

**The power panel is for full configuration.** Pressing the power button opens a radial panel and slows the action; it does not pause it. The slowdown starts at 50% speed, and the final value is left for playtesting. There the player can set an exact distribution and inspect what each level does. Because the panel takes the player's eyes off the fight, opening it mid-combat is a real cost and not a free pause. It is an option in combat, not a requirement.

## What routing must never do

**Never strand the player.** Any distribution can be escaped in seconds, presets always include a safe option, and no mission is failed purely because the player launched with the wrong preset.

**Never be solved once.** Contracts, enemies, and damage keep changing which system matters. If playtesting finds one dominant distribution for all situations, that is a tuning bug to fix, not a discovery to celebrate.

**Never punish accessibility.** An optional Auto-route assist starts from the player's own setup and, when the hull is threatened, applies shield bumps using the quick-routing rule, then returns to the player's setup when the threat passes. It makes the game playable for players who cannot manage four systems at speed, without removing the system for anyone else — see Section 10.

## How routing and frequencies fit together

Routing and tuning (Section 6) are two different decisions that multiply. The reactor decides how much power a system has; the frequency decides how well that power lands or blocks. Routed sensor power sets how fast and how far the frequency scan works, routed weapons power sets how fast the beam can be retuned, and routed shield power sets how fast the shield can be retuned and how fast all four quadrants recharge and equalise — so a pilot who wants to identify a target quickly approaches with sensors-heavy power, then shifts power back to weapons and shields once the bands are known and set. Because every ship has to be scanned, Sensors power is never a one-time cost: a fight with several ships keeps pulling power back toward the scan. Neither system replaces the other: a boosted beam on a non-matching band is still mostly blocked, and a perfectly matched beam with no power behind it still hits weakly. There is no repair interaction to add: a ship that has just been penetrated cannot buy its systems or hull back in flight — it must survive on shields, facing, and engines until it can dock.

# 6. Shield and weapon frequencies — tuning, not guessing

> **Requirement — locked, not open:** Shield and weapon frequencies are a core, non-negotiable system in Solar Storms, alongside reactor routing. They are in the prototype and the MVP, and they cannot be cut for scope. What makes this version different from the original is not the presence of frequencies — it is that no frequency choice in Solar Storms is ever a guess. What is open to change is the tuning values (damage fractions, recalibration times) and what is added beyond the MVP set — never whether frequencies exist, and never whether they can be found out.

In the original game, frequencies carried real power — the right shield frequency blunted an attacker, the wrong weapon frequency wasted a volley — but the player had no reliable way to learn what 'right' was for the ship in front of them. Choosing felt like a crapshoot because, from the player's seat, it was one: the information was hidden and the feedback arrived too late to teach anything. Solar Storms keeps the power and removes the crapshoot, with three commitments: the bands are few and named, the rule is one — the closer the match, the more damage gets through — and the answer is always knowable in flight before it is expensive. Every ship carries its own bands, so the answer is something the player scans for on each ship, not something memorised once.

## The five bands

Every beam weapon fires on one band, and every shield is tuned to one band — one band for the whole ship, covering all four shield quadrants together, never a separate band per quadrant. There are five bands, arranged on a line from lowest to highest: Infra, Low, Mid, High, and Ultra. The player starts able to tune to the middle three — Low, Mid, and High. Infra and Ultra are the outer bands, locked until the first Tuner upgrade (Section 9), so the unlock is something the player can see coming on their band strip. Five is enough that a wrong guess costs real damage, and few enough to learn in one fight:

| Band | Position | Colour | Shape | Pips | Sounds as |
| --- | --- | --- | --- | --- | --- |
| **Infra** (outer, locked at start) | 1 | Deep red | Horizontal bar | 1 | Lowest note, with an added harmonic so it carries on small speakers |
| **Low** | 2 | Amber | Circle | 2 | Second note |
| **Mid** | 3 | Cyan | Diamond | 3 | Third note |
| **High** | 4 | Magenta | Triangle | 4 | Fourth note |
| **Ultra** (outer, locked at start) | 5 | White, with a glint | Four-point star | 5 | Highest note, bright but not shrill |

Bands are never encoded by colour alone: each band has a name, a colour, a shape, a pip count (its position on the line, one to five pips), and a pitch, and any one of them is enough to identify it. The five pitches are five ascending notes of a pentatonic scale, so stepping up the line sounds like climbing. The player's own two bands — weapon and shield — are shown on the combat HUD at all times, and a ship's beam colour always truthfully shows its weapon band; its shield band is shown once it has been scanned. [Open — verify before locking: the colours and shapes should be checked in a colour-blindness simulator (deep red sits close to amber for some players, which is why Infra also leans on its shape and pip count), and the pitches, Infra's especially, should be checked on small speakers.]

## The one rule: closer is stronger

There is a single rule, stated from the attacker's side — the defensive mirror follows from it:

| Situation | Result | What the player wants |
| --- | --- | --- |
| **Weapon band matches the target's shield band (0 steps apart)** | Penetrating — the beam passes through the shield's own frequency and deals full damage (100%) | As the attacker: match the target's shield band exactly |
| **Weapon band is one step from the target's shield band** | Partial — the shield stops part of the beam (50% damage) | As the attacker: close enough to hurt while you retune, not enough to win the fight |
| **Weapon band is two or more steps from the target's shield band** | Blocked — the shield's tuning stops most of the beam (25% damage) | As the defender: keep your shield two or more steps from the attacker's weapon band |

Distance is counted along the line of five bands, one step per band. The closer a beam's band is to the shield's band, the more of it gets through, so offence is 'tune your weapon onto their shield' and defence is the mirror image, 'tune your shield away from their weapon'. The damage values in the table are starting values for the prototype, and the two-or-more-steps row can be split into further rows if testing calls for it. The rule itself — closer is stronger, with no hidden states — is not open. Missiles ignore frequencies entirely, as Section 4 describes.

## Three bands, then five

Because the bands sit on a line, which shield bands fully block an attacker depends on where the attacker's band is:

| Attacker's weapon band | Shield bands that block it (two or more steps away) | Of those, available before the unlock |
| --- | --- | --- |
| **Infra** | Mid, High, Ultra | Mid, High |
| **Low** | High, Ultra | High |
| **Mid** | Infra, Ultra | None |
| **High** | Infra, Low | Low |
| **Ultra** | Infra, Low, Mid | Low, Mid |

Before the unlock, the player's range is Low, Mid, and High, and the contract board rolls enemy bands from that same range (Section 7), so Infra and Ultra attackers do not yet appear. In that three-band start, the only fully blocking pairing is Low against High, and a Mid weapon cannot be fully blocked at all: the best the player's shield can do against it is a Partial. After the first Tuner upgrade, a Mid weapon is blocked only by an Infra or Ultra shield. The unlock therefore widens the player's answers — it does not simply add power.

## How the player knows — never a guess

A band choice is only fair if the player can find out the answer. Because every ship's bands are its own, the scan is the main tool and the other cues back it up. Solar Storms gives three overlapping ways, so no single missed cue can turn a fight back into a gamble:

**The beam, immediately.** A ship's weapon band is the colour of its beam, visible the moment it fires. The player always sees what is coming at them, so the defensive half of tuning needs no scan to start.

**The sensor scan, in flight.** Keeping a target in the targeting reticle scans it — no extra button is needed. Every ship is scanned separately; a scan of one ship says nothing about the next. When the scan completes, the target panel names its shield band and its weapon band, with name, colour, shape, and pips, and the ship gets a band tag beside it showing its shield band (Section 10). Scan time and range scale with the power routed to Sensors & targeting (Section 5): about a second on boosted sensors, a few seconds on standard power, and slow or out of range when sensors are starved. Once a ship has been scanned, its data stays live while it is inside the scanning radius, with no need to scan it again (Section 10). Feeding the sensors is how a careful pilot buys certainty quickly; skipping a scan and relying on the beam colour and the first hit is how a confident one saves the power.

**First-hit feedback, in words.** The first beam hit on any shield always reports its result plainly — a PENETRATING marker, a resonant shield flare, and a sharper crack for an exact match; a PARTIAL marker for a band one step away; and a BLOCKED marker, a solid shield thud, and a duller impact for two or more steps. One hit tells the player how close the band is and does not name it, so even an unscanned target teaches how far off the band is from the very first shot, and the second shot narrows it further.

**Briefing intel and the codex support the scan but do not replace it.** A contract's briefing names the expected classes, how many of them there are, and who flies them, when intel is available, so the player can plan a launch (Section 7). It never names bands, because bands belong to individual ships. The codex records each class's stats, behaviour, and how many ships of it the player has scanned. It does not record a band for the class, because a class does not have one.

## Retuning in flight

The player steps the weapon band and the shield band along the line, independently, one band at a time, with a separate up and down input for each (Section 11). A band stops at the ends of the player's range: pressing up at the top band (High before the unlock, Ultra after) or down at the bottom band (Low before the unlock, Infra after) does nothing but play a dull denied tick, and bands never wrap round. Presses are buffered: recalibration starts about 0.3 seconds after the last press, so moving three bands is three taps and one recalibration, not three. A retune is not instant: the beam is offline for about a second while it recalibrates, and the shield stops recharging — in all four quadrants — for about a second and a half while it retunes. Those recalibration times shorten as more power is routed to Weapons or Shields respectively, so retuning between attack passes — not in the middle of one — is the skill, and routing power to the system being retuned is how a pilot retunes faster. Retuning costs no consumable and has no cooldown beyond the recalibration itself, and starting bands can always be set free at the station before launch; because the bands in a fight are not known in advance, the starting bands are a hedge until the scan, not an answer. Recalibration time is flat per retune, however many bands were crossed; that and the 0.3 second buffer are starting values for the prototype. [Open: whether recalibration should instead grow with the number of steps moved, which would make crossing the line cost more time and could add a further cost to reaching the outer bands.]

A worked pass shows how the two locked systems compose. The player approaches a ship with sensors-heavy power and scans it: shield Mid, weapon High (both rolled for this ship). They step their beam to Mid (an exact match with its shield, so it penetrates) and their shield to Low (the only band they can reach at the start that is two or more steps from a High weapon band, so it cuts the ship's beam to a quarter damage; Infra would also work once unlocked). They then quick-route power back from sensors to weapons and shields and make their pass. Scan, tune, route, fight — in that order, for every new ship, faster as the player gets better at it and as the Tuner and Sensors improve.

## Enemy bands are rolled with the contract, then fixed

**Each ship has its own two bands.** Every enemy ship's weapon band and shield band are each drawn from the bands the player's Tuner can reach at the time the contract is built — Low, Mid, and High before the unlock, all five after it — using a seeded random draw. A contract keeps the pool it was built with, so a job built before the unlock stays a three-band job even if the player unlocks the outer bands before flying it. The draws are stored with the contract's data, so the ships a contract places are fixed from the moment it is built: nothing is rolled at spawn, nothing is re-rolled during a fight, and retrying a contract after a destruction meets the same ships. Weighting within a pool is a tuning option, not an MVP feature: it starts uniform, but in the three-band pool one weapon in three is Mid and cannot be fully blocked, so Mid weapons may be weighted lower early.

**The first two fights are authored.** The first Dart in the game has weapon Low and shield Mid, and the first Mauler has weapon High and shield Low (Section 4). They exist to teach the rule on clean examples, and all later ships carry rolled bands.

**No silent changes in the MVP.** MVP enemies hold their bands for the whole fight. Post-MVP, advanced enemies may retune once after their shield breaks — and only with a two-second telegraphed recalibration, flashing the incoming band's colour and shape before it locks, while their beam or shield is offline. A change the player can see coming, with time to answer it, is a decision; an instant, silent change would be the crapshoot returning, and it is ruled out.

**Enemy fire is honest.** An enemy's beam colour and its retune flash always show its true band, and a scanned ship's shield band is always true. The game hides a ship's shield band until it is scanned, but it never shows a false one. A ship's shield band is not shown on screen before a scan; after a scan it shows on the ship's band tag (Section 10).

## What frequencies must never do

**Never be unknowable.** Every band in a fight can be found out in flight. A weapon band is visible from its beam, and a ship's two bands are named by its scan; at least two cues are always available, including the first-hit verdict.

**Never be rolled during play.** Bands are rolled once, when the contract is built, never at spawn and never re-rolled mid-fight. A fight's outcome follows the rule, never a hidden die. If a playtester describes a frequency outcome as luck, the likely cause is a scan that was too slow, too short-range, or too hard to hold — and that is a bug to fix, not a system working as intended.

**Never be an unreasonable scan.** A ship can always be scanned in a few seconds on standard Sensors power and about one on boosted power. MVP fights have no more than three ships, so scanning each one stays a decision and never becomes homework.

**Never guarantee a safe shield.** Random weapon bands can leave no shield band that blocks every attacker. Two attackers on Low and High weapon bands, for example, have no common safe shield band, because a shield must be two or more steps from both. In the three-band start, even a single Mid weapon cannot be fully blocked. That is intended: the answer is who to kill first, which side to face, and when a missile is worth it.

**Never be brute-forced or mandatory.** Extra weapon power cannot punch through a distant shield — a beam two or more steps from the target's shield band is cut to a quarter however strong it is, so tuning is not optional homework for damage, it is the damage decision — and a player who cannot or will not tune can still fall back on missiles and on keeping their own shield two or more steps from their attacker's band where one exists, which the assist below supports.

**Never punish accessibility.** An optional Auto-tune assist can set the shield band two or more steps from the last scanned attacker's weapon band and prompt — not force — the weapon change that matches the target's shield. Bands are identified by name, shape, pips, and pitch as well as colour, and every tuning input is remappable (see Section 10).

## Beyond the MVP

Designed now, added only after the five-band game is proven: telegraphed enemy recalibration, as above; a hardened shield sidegrade that still blocks a matched beam at half damage, in exchange for a weaker block against mismatched beams; and tuner upgrades that shorten recalibration and add a spectrum analyser that scans faster, at longer range, and warns before an enemy retune locks. None of these change the one rule — they widen it, exactly as reactor upgrades widen routing without removing it.

# 7. Missions and progression

| Mission type | Objective | Twist it can carry |
| --- | --- | --- |
| **Bounty hunt** | Find and destroy a named target and its escorts | Target surrenders, flees, or offers a counter-bribe |
| **Cargo run** | Carry cargo to a destination, intact and on time | Cargo is contraband, fragile, or wanted by someone else |
| **Escort** | Keep a slow friendly ship alive to its destination | Escorted pilot panics, lies about the route, or is the real target |
| **Story contract (post-MVP cadence)** | A hand-authored job advancing the conspiracy | Recontextualises earlier jobs and factions |
| **Salvage / survey (post-MVP)** | Recover an object or scan sites under time or threat pressure | What you find is not what the client described |

## How contracts are made

The contract board is built from templates plus a small number of authored story jobs. This keeps the session loop supplied with jobs without hand-writing every one, and it keeps the data a designer can tune and a playtest log can read.

**Templates for bounty, cargo, and escort.** Each template takes the same five parameters, and the board builder fills them in:

| Parameter | What it sets |
| --- | --- |
| **Mission type** | Bounty, cargo, or escort |
| **Enemy mix** | Which classes (Dart, Mauler, or both) and how many, up to three ships in the MVP — this decides the intel and most of the risk. Each ship's own weapon and shield bands are rolled here from the bands the player's Tuner can reach, and stored with the contract |
| **Route** | The destination and region of the Kestrel Reach, which decides travel time and which jobs chain well together, and where the enemy groups' home points are placed |
| **Client faction** | Who pays and whose reputation moves (Section 8) |
| **Intel quality** | Confirmed or an unconfirmed report of which classes are on the job, how many, and where they are |

A contract may also roll one optional twist from its mission type's list above. Risk rating, pay, expiry, and the recommended power preset are derived from the five parameters, not set freely, so pay follows risk and the recommended preset follows the mission type and enemy mix. Each enemy group's patrol and chase radii (Section 4) are likewise derived from the enemy mix and Route, not added as further parameters; ambushers and escort attackers are marked as hunters.

**The board.** The board shows three to five contracts at a time. It always includes a low-risk cargo run (see No fail-state spiral below) and a spread of mission types, and it favours contracts that share a region so the player can chain them. The board is built from the player's current Tuner tier, so the first contracts roll enemy bands from Low, Mid, and High only. These counts are starting values for the prototype.

**Authored story contracts.** Story jobs are hand-written assets, pinned to the board by story flags rather than generated. They use the same contract fields as templates. The MVP has one.

**Beyond the MVP.** Once the templates have proved the board feels varied, a procedural generator for side missions may be added. Story contracts stay authored.

## What a contract shows

Contracts show, before acceptance: destination, pay, recommended loadout, a recommended power preset, known enemy intel (the expected enemy classes and how many, marked as confirmed or as unconfirmed reports), where the enemy is expected (a bounty's briefing marks the target's patrol circle on the star map), faction, expiry, and a plain-language risk rating. Hidden information is allowed — a client can lie, and unconfirmed intel can be wrong about which classes or how many, or can draw a patrol circle offset or too large, and both are labelled as such — but the game never hides its own rules from the player, and confirmed intel is never wrong. Briefing intel never names bands: every ship's bands are rolled individually and are found out in flight by the scan (Section 6), so every contract expects the player to scan.

## Failure and expiry

A contract that expires unaccepted simply leaves the board. A contract the player fails (the cargo is lost or late, the escorted ship is destroyed, the bounty target escapes) pays nothing and costs some reputation with the client faction. Destruction follows Section 4: a repair bill and lost contract progress, never lost upgrades or story progress. The exact reputation costs are tuning values.

## How the missions stress the ship

Each mission type stresses the reactor differently, which is what keeps routing fresh across a session: a bounty hunt swings between Burn (to run the target down) and Combat (to finish it); a cargo run lives in engines and sensors, until an ambush forces a hard choice between running and shielding the cargo; an escort asks for shields and weapons sustained longer than the reactor comfortably allows, so heat management and quick-routing decide whether the escorted ship gets home. The same jobs stress tuning differently too: a bounty names its target's class, but the target's own bands are still rolled and still have to be scanned; a cargo ambush is an unannounced group of hunters the player must scan under pressure, ship by ship; and an escort can put two attackers with different weapon bands on the player at once, where the beams are visible at a glance and the shield has to sit two or more steps from both. Sometimes no such band exists — attackers on Low and High weapon bands leave none — and then the answer is who to shoot first, which side to face, and when a missile is worth spending. In an escort that pressure is sharpest: damage taken is damage carried for the rest of the job, because there is no in-flight repair to route to — protecting the escorted ship and the player's own hull is a shields, facing, and positioning decision until the station.

## Progression

**Credits** pay for repairs, missiles, and upgrades. The economy is tuned so one good session always buys something meaningful.

**Reputation** with each faction opens gated contracts and stock, and closes others. Reputation changes are shown on the contract completion screen, with the reason.

**Licences** gate the most powerful equipment behind story beats rather than pure grinding, so progression and narrative stay linked.

**Knowledge** is progression too. Every class scanned goes into the codex with its stats and behaviour, which makes later intel on that class confirmed more often and makes the player faster at reading a ship. What knowledge does not buy is the bands: those are scanned on every ship, and the Sensors and Tuner upgrades are how that scan gets faster.

**The outer bands** are progression too. The first Tuner upgrade unlocks Infra and Ultra, which widens the player's answers (a Mid weapon becomes blockable) and widens the enemy pool the board draws from (Section 6). The first Tuner tier is priced so most players reach it within their first few sessions; the exact price is a tuning value.

**No fail-state spiral.** A bad run costs credits and time, never the ability to recover: the station always offers at least one low-risk cargo run scaled to a damaged starter ship.

# 8. World, story and factions

All names, characters, and events in this section are original placeholders for this project, created for Solar Storms. They are starting points to be developed in writing, not final canon.

## Setting

A single inhabited system on the edge of administered space — the Kestrel Reach — where one orbital station, Verra Dock, serves miners, haulers, and a small patrol force. Officially, things are quiet. In practice, ships have started disappearing on routine routes, and someone is quietly buying up salvage rights to every wreck before families can claim them.

## Player character

An unnamed-at-design-stage freelance pilot, defined by the player through job choices rather than a fixed backstory monologue: whose contracts they take, who they spare, and who they believe. Dialogue options stay short and practical — this is a working pilot, not a speech-maker.

## Factions (starter set)

| Faction | What they want | What they offer |
| --- | --- | --- |
| **Verra Dock Authority** | Order, fees paid, incidents kept quiet | Safe harbour, licences, official contracts |
| **Free Haulers' Compact** | Fair pay and mutual protection for independents | Cargo work, discounts, early warnings |
| **Meridian Salvage Group** | Exclusive control of wreck and salvage rights | High pay, few questions — at a reputation cost |

## Story shape

Three acts, told through contracts. Act 1: honest work and odd details that do not add up. Act 2: competing versions of the disappearances, and a job the player should refuse — with real reasons to take it. Act 3: leaving the starter system with proof, a price on the player's head, or both. The MVP contains only the opening: one story contract that introduces the disappearances and ends on an unanswered question.

# 9. Ships, weapons and upgrades

The MVP has exactly one ship. Depth comes from fitting it, not from collecting hulls.

**Starter ship — Kestrel-class light freighter-fighter.** Mediocre at everything, which is the point: every upgrade is felt. Cargo space for small runs, one beam mount, one missile rack, one defensive slot (locked in the MVP) — and a 100-unit starter reactor that is deliberately a little too small for the ship, so routing matters from the first launch.

**Upgrade tracks.** Reactor (total output, efficiency), Engine (speed, boost), Shield (capacity, recharge), Beam (damage, heat efficiency), Missile rack (capacity, lock speed), Cargo (capacity, shielding), Damage Control (a passive, station-side track only — station repair cost, speed, and completeness, plus resilience fittings; never routable and never an in-flight repair rate), and Tuner (weapon and shield recalibration speed, scan range, and — at its first tier — the outer bands Infra and Ultra). Each track has three tiers in the full game; the MVP ships with two tiers on Reactor, Beam, and Shield, and one Tuner tier, so retuning visibly speeds up inside the MVP and the outer bands unlock. A post-MVP spectrum analyser caps the Tuner track: faster, longer-range scans and a warning before an enemy retune locks.

**The reactor track comes first.** A reactor upgrade does not remove the routing trade-off — it widens it. More output means the player can boost two systems at once instead of one; better efficiency means less heat for the same performance. Playtesting must confirm that a maxed reactor still forces choices in hard fights; if it does not, high-end enemy pressure rises to restore them.

**Sidegrades, not just upgrades.** At tier 2 and above, some parts trade one strength for another — faster recharge for lower capacity, for example — so two maxed ships need not be identical. The post-MVP hardened shield (Section 6) is the signature tuner sidegrade: it still blocks a beam matched to its own band at half damage, in exchange for blocking mismatched beams more weakly than a standard shield.

A second, specialised hull (a fast interceptor with little cargo, or a gunship that cannot run light cargo profitably) is the first major post-MVP addition, gated behind a licence and a story beat.

# 10. Presentation, audio and interface

**Art direction.** Faithful to the original's look: sprite-based ships in a sparse starfield, framed in riveted metal, with a pixel font in yellow on dark. Sprites may be higher resolution than the original, but the game must look unmistakably like Solar Winds at a glance. Ships read by silhouette first, faction colour second. Space is dark but never murky — threats, pickups, and objectives each own a distinct colour and shape language.

**Screen layout.** The game view fills the left of the screen with the ship at its centre, and a side panel sits on the right, both inside a riveted metal frame. The panel has the minimap at the top, with the following below it: the four reactor bars (Weapons, Engines, Sensors, Shields), hull, missile count, the player's weapon and shield frequency, a small combat lamp, and information on the targeted ship. A message bar runs along the bottom of the game view. Shield quadrants are not shown in the panel; they are drawn on the ship (see below).

**Combat feedback.** Hit flashes, a directional damage indicator, heat warning that escalates from HUD to audio cue, and a brief hit-pause on missile kills — plus the frequency verdict on every first hit on a shield: the word PENETRATING, PARTIAL, or BLOCKED, in text, with its own shield effect and sound (Section 6). Shield state is shown by the outline described below. The combat lamp on the side panel lights while the player is in combat state (Section 4), flashes when a hostile forces the ship out of travel mode, and goes dark after the calm period. When the player tries something that combat state locks — engaging travel mode or applying a preset — the message bar says why. If the player cannot tell what happened, that is a bug, not a style choice.

**Shields on the ship.** A white outline surrounds the ship, split into four segments — fore, aft, left, and right. Each segment's brightness follows its quadrant's remaining strength: it fades as the quadrant is damaged and disappears completely when the quadrant is depleted. The outline carries strength only; it never changes colour to show a band. Enemy ships show the same outline only after they have been scanned. A scanned enemy's outline stays live while the enemy is inside the scanning radius; if it leaves the radius, the last known values stay on screen as stale data until it returns, with no need to scan again.

**Band tags.** A scanned ship carries a small tag beside it showing its shield band as that band's glyph: its shape, in its colour, with its pips. When a scan completes, the ship's outline flashes once in the band's colour and then returns to white. A ship inside the scanning radius that has not yet been scanned shows an empty bracket where its tag will appear, so the player can see at a glance what is left to scan. If a scanned ship leaves the scanning radius, its tag goes hollow and dim, like the rest of its stale data. The tag shows the shield band only: the weapon band shows on the beam, and both bands show on the target panel for the locked target. Because the glyph carries shape and pips as well as colour, an Ultra tag stays distinct from the white outline. The tag is a small set of sprites (one glyph per band, plus the bracket), cheap to make and in keeping with the pixel style.

**Scanning radius.** A ring shows the scanning radius around the player's ship, in the main view and on the minimap. Its size follows the power routed to Sensors. Any scanned ship inside the ring is up to date; any ship outside it is showing stale data.

**Power you can see and hear.** Routing is written on the ship itself: engine glow and trail length follow engine power, beam colour intensity follows weapon power, and the scanning radius ring grows with sensor power. Shield strength shows in the outline around the ship, quadrant by quadrant. A routing change also has a short, distinct audio tick per system, so experienced players route by ear without looking down. The side panel shows four reactor bars in matching colours — Weapons, Engines, Sensors, Shields.

**Tuning you can see and hear.** A beam's colour is its band's colour and is visible the instant the ship fires, and a retune plays its band's pitch as it locks — the five bands are five ascending notes, so a retune sounds like moving up or down the line. The player's weapon and shield bands appear on the side panel as a five-position strip, with the current band lit; before the unlock, Infra and Ultra are shown greyed and locked, so the player can see what the Tuner upgrade will add. The target panel shows a scanned target's two bands as named, shaped chips with their pips, and a scanning progress arc around the reticle shows how close certainty is. An unscanned ship's shield shows no band at all; only a scan names it, and then on its band tag. Colour, shape, name, pips, and pitch always agree, so no player has to rely on any single channel. While the tuning modifier is held, the weapon and shield band rows on the side panel are highlighted and the reactor bars dim, so the player always knows which layer the D-pad is on.

**Station interface.** One screen, four tabs: Contracts, Outfitter, Repair, and Star Map / Journal. Everything the session loop needs is reachable in two inputs from docking.

**In flight.** Quest log entry pinned to the current contract, objective marker with distance, and a star map that pauses the game. Patrol circles the player has learned about (from a briefing or by meeting a group) stay drawn as faint circles on the star map and the minimap, so they can be avoided or sought out; how accurately a briefing draws one depends on its intel quality. No information needed in combat lives only in a menu.

**Audio.** Beam, missile, and engine sounds pitched by burst mode and speed so experienced players can fly partly by ear. Music is sparse in open space and gains layers in combat; station music carries the setting's character.

**Accessibility.** Remappable controls (including every routing and tuning input), aim assist strength, an optional Auto-route assist (see Section 5) and an optional Auto-tune assist (see Section 6), screen-shake and flash toggles (including the scan-complete flash and the combat lamp flash), a camera look-ahead toggle (see Section 4), subtitle and UI scale options, and difficulty settings that adjust enemy damage and aggression — not player damage alone.

# 11. Controls and platforms

Controller is the reference control scheme; keyboard and mouse must be equally viable on PC, not an afterthought port of it.

| Action | Controller (Xbox layout) | Keyboard / mouse |
| --- | --- | --- |
| **Steer / aim** | Left stick steers; right stick aims (twin-stick) | Mouse aims; A/D or mouse turn — both schemes supported |
| **Fire beam** | Right trigger | Left mouse button / Space |
| **Change burst mode** | Right bumper (cycles 1–2–3) | Mouse wheel / 1–3 keys |
| **Fire missile** | Left trigger (hold to lock) | Right mouse button (hold to lock) |
| **Boost** | Left stick click | Shift |
| **Route power — quick-shift** | D-pad: Up Weapons, Right Shields, Down Engines, Left Sensors (tap = 1 step, hold = more) | Arrow keys in the same layout / Numpad 2-4-6-8 |
| **Tuning layer (modifier)** | Hold left bumper — the D-pad switches from routing to tuning while held | None needed — tuning has dedicated keys |
| **Weapon band up / down** | Left bumper held + D-pad Up / Down (tap = one band, no hold-repeat) | E / Q |
| **Shield band up / down** | Left bumper held + D-pad Right / Left (tap = one band, no hold-repeat) | C / Z |
| **Power panel (slows the action)** | Right stick click (press), then choose with the right stick | R (press) |
| **Scan target's bands** | Automatic — keep target in reticle; no button | Automatic — keep target in reticle; no button |
| **Interact / dock** | A | F |
| **Star map / journal** | View button | M / Tab |
| **Pause** | Menu button | Esc |

Routing is designed controller-first, because that is the hard case: if power can be re-routed with a single D-pad tap, mid-fight, without a menu, the system works everywhere. Any routing scheme that needs a mouse, a pause, or more than one input for a quick-shift fails the prototype gate. With Damage Control cut from routing there is no diagonal exception: every routable system has a cardinal D-pad direction, and no preset or input routes power to repair.

Tuning shares the D-pad through a held layer. Holding the left bumper turns the four D-pad directions into band steps: Up and Down for the weapon band, Right and Left for the shield band (right is up, so the direction matches the line from Infra to Ultra). Releasing the bumper returns the D-pad to routing. The layer is a hold, not a toggle, so the player can never be stuck in the wrong mode, and routing inputs are ignored while the bumper is held. Each tap moves one band within the player's unlocked range; the buffer described in Section 6 means several quick taps cost one recalibration. The scan needs no button at all. Tuning is held to the same gate as routing: any scheme that needs a menu, a wheel, or a pause fails. Because routing and tuning share a pad, accidental routing from a half-pressed bumper is the first thing the prototype should test. The power panel moves to the right stick click, since selecting a distribution in a radial panel is a natural use of the right stick, and Interact stays on A.

Platform plan: PC (Windows, Steam-ready) first, because iteration and playtesting are fastest there. Xbox Series X|S second, using the same controller-first design and Unity build pipeline. No platform-specific content is planned; saves do not need to transfer between platforms for the first release.

# 12. Technical direction

**Engine: Unity, language: C#.** This plays directly to an existing C# / .NET background, keeps the learning curve on game design rather than a new language, and provides a proven path to both PC and Xbox builds. To be confirmed by the first prototype — if flight feel cannot be achieved cleanly, that is discovered in week one, not month three.

**Architecture for the prototype.** Data-driven contracts, ships, and weapons (ScriptableObjects or equivalent data assets) so balancing and new missions do not require code changes. A single scene for the starter system in the MVP, with the station as a UI state rather than a separate level.

**Contracts are templates plus parameters.** Each mission type is a template asset that takes the five parameters in Section 7. A board builder fills three to five slots from them, derives risk, pay, expiry, and the recommended preset from the parameters, and rolls each enemy ship's weapon and shield bands with a seeded draw that is stored in the contract data. The pool of bands for that draw is set by which bands the player has unlocked, which is a save flag set by the first Tuner tier. Story contracts are separate authored assets pinned to the board by story flags; the two tutorial fights use authored fixed bands. Because every contract is a parameter set with its rolled bands stored, playtest logs can record exactly which contracts and which bands were offered and taken.

**Enemy groups are data.** A group asset holds its ships and their small offsets, one home point, a patrol radius, a chase radius, and a hunter flag. The board builder places groups from the Route and Enemy mix parameters and stores them in the contract data. A simple group controller handles wander, notice, chase, and return; it sets and clears each ship's alert flag, which the combat state reads. Returning groups recharge shields but never restore hull.

**The reactor is a simulation, not a UI trick.** One central reactor model owns output, routing, and heat, and engines, shields, weapons, and sensors & targeting read their effective stats from it every frame — for the player and for enemy ships alike. No system may cache a private copy of its power level, because a routing change that any system ignores would quietly break the game's core promise. Routing events are logged in playtest builds, so tuning can see which presets and shifts players actually use.

**Frequencies are data, not dice.** One central tuning model owns every ship's weapon and shield bands, scan state, and recalibration timers, for the player and for enemies alike, and knows which bands the player has unlocked. An enemy ship's bands are read from its contract data, where the board builder rolled them once, so there is no random roll at spawn or during a fight anywhere in the code path. The band tag reads its glyph from the same scan state, so it can never show a band the scan has not reported. Playtest logging records the seed, scans, retunes, and penetrating/partial/blocked outcomes so a designer can prove from the log that an outcome followed the rule, never chance.

**Shields are four pools, one system.** One central shield model owns the four quadrant pools, the shared recharge rate from routed Shields power, and the slow equalisation between quadrants — for the player and for enemies alike. Hit assignment is by incoming direction relative to the ship's facing, a depleted quadrant passes damage to the hull, and the single shield band applies to all four quadrants. Playtest logging records quadrant breaks, so a designer can check that focused fire — not chance — opened a side.

**Combat state is one flag.** One central state, computed from hostile positions and alert flags, is read by travel mode, presets, and the combat lamp. No system keeps its own definition of "in combat". The engagement radius, per-class notice ranges, and calm period live in one data asset so they can be tuned together during playtests; the patrol and chase radii live on the enemy groups.

**Saves.** Autosave on docking and on contract completion, plus a manual save at the station. One profile with three save slots for the first release.

**Testing habit.** Every build is played with a controller from a fresh save. The MVP is not done when the code works; it is done when a fresh player completes the loop unaided.

**AI-assisted development.** Coding assistance is used for boilerplate, tooling, and iteration speed, with all generated code reviewed and playtested like any other code. Design decisions in this document remain human decisions.

# 13. MVP scope — the first buildable game

The minimum viable product (MVP) is a complete, playable loop — not a tech demo. It is the smallest version that can answer the only question that matters at this stage: is this fun?

| In the MVP | Explicitly out of the MVP |
| --- | --- |
| **One star system (Kestrel Reach) with Verra Dock** | Multiple systems, jump travel, planetary landing |
| **One station with Contracts, Outfitter, Repair, Map** | Station walking, NPC conversations beyond briefings |
| **One starter ship, Reactor + Beam + Shield upgrades (2 tiers), Tuner (1 tier, which also unlocks the outer bands Infra and Ultra)** | Second hull, defensive slot, sidegrade parts |
| **REQUIRED: reactor routing — 4 systems (Engines, Shields, Weapons, Sensors & targeting), 3 presets (locked in combat state), D-pad quick-shift, power panel (slows the action)** | Damage Control routing (Damage Control is passive / station-only), Cooling routing, custom presets |
| **REQUIRED: frequencies — 5 bands on a line (Infra, Low, Mid, High, Ultra; the player starts with Low, Mid, High and the first Tuner tier unlocks the outer two) with a closer-is-stronger damage curve, per-ship scan with a band tag on each scanned ship, held-LB D-pad retuning with explicit up and down per band, per-ship rolled bands drawn from the unlocked set (fixed on the two tutorial fights), class intel + codex** | Enemy mid-fight recalibration, hardened shield, class weighting of bands |
| **REQUIRED: shield quadrants — fore, aft, left, right on the single Shields system; per-quadrant damage, shared recharge, slow equalisation, one band for the whole ship** | Separate reactor systems or separate frequency bands per quadrant |
| **Three mission types as parameterised templates: bounty, cargo, escort** | Salvage / survey, faction war missions, procedural side-mission generator |
| **Two enemy types: Dart and Mauler, placed as groups of up to three ships with a home point, patrol radius, and chase radius (wander, notice, chase, return)** | Carriers, snipers, aces, capital ships; waypoint patrol routes, schedules, sleeping groups, reinforcements |
| **One authored story contract introducing the disappearances** | Full three-act campaign |
| **Controller + keyboard/mouse, PC build** | Xbox build, multiplayer, mod support |
| **Autosave on docking, one save slot** | Cloud saves, multiple profiles |

> **MVP success test:** A first-time player, given no coaching, docks, takes a job, flies it, re-routes power at least twice during the fight without opening a menu, scans a target and retunes their weapon to match its shield after a BLOCKED result, and — after taking damage — turns a depleted shield quadrant away while it recharges, fights, returns, buys an upgrade they chose themselves, and asks for the next job. If that happens, build more. If it does not, fix flight, routing feel, frequency readability, combat readability, or payoff — in that order — before adding content.

# 14. Roadmap beyond the MVP

| Phase | Goal | Adds |
| --- | --- | --- |
| **0 — Prototype** | Prove flight, routing, tuning, and one fight feel right | Ship movement, camera, 4-system reactor with presets + quick-shift, combat state, one enemy group with patrol, chase and return, beam + heat, four-quadrant shields, one Dart with fixed bands on the first fight and rolled bands after, scan + band tags + retune, and a switch between the three-band and five-band sets so both can be played |
| **1 — MVP** | Prove the full session loop | Station, 3 mission-type templates and the board builder, reactor + tuner upgrades (the first Tuner tier unlocks Infra and Ultra), class intel + codex, Mauler with rolled bands (fixed on the first), save, 1 authored story job |
| **2 — Vertical slice** | Prove production quality for 30–45 min | Polished art/audio pass incl. power and band visuals, 2nd enemy polish, routing and tuning onboarding |
| **3 — Early content** | Prove variety and story cadence | Cooling routing, custom presets, telegraphed enemy recalibration, salvage/survey, defensive slot, sidegrades incl. hardened shield, waypoint patrol routes and reinforcements, Act 1 complete, optional procedural side-mission generator |
| **4 — Expansion** | Grow only what players use | Second system + jump travel, second hull, Act 2 |
| **5 — Release prep** | Ship it | Xbox build, difficulty/accessibility pass, localisation check |

Phases are gates, not dates. A phase starts when the previous phase's success test passes, and any phase can send the project back a step if playtesting says the loop has broken.

# 15. Risks and open decisions

## Risks

**Scope creep — the classic killer.** A second star system, a second hull, or multiplayer will always sound essential. The MVP table in Section 13 is the contract; new ideas go in a parking list, not in the build. Two exceptions are already written into that contract: reactor routing and frequencies are in the MVP, and cutting either to save scope is not an option — other content is cut first.

**Frequencies as a crapshoot — the frequency system's own risk.** This is the original game's failure mode, named as the thing to avoid: if a player cannot find out a ship's bands before the choice costs them, tuning degenerates into guessing and resentment. With every ship's bands rolled individually, the whole weight now rests on the scan. Mitigation: a scan whose speed the player controls with sensor power, a beam colour that shows the weapon band instantly, a band tag that keeps every scanned ship's shield band on screen, and a PENETRATING/PARTIAL/BLOCKED verdict on the very first hit — plus a prototype gate that specifically asks playtesters, after each fight, to name the bands they faced and how they knew. An answer of 'I scanned it' or 'the beam colour' passes; 'I guessed' fails that gate.

**Scanning as homework.** If every ship must be scanned, a fight with several ships can become a chore. Mitigation: a scan that takes about a second on boosted Sensors, scans that stay live inside the scanning radius, empty brackets that show what is left to scan, a cap of three ships per fight in the MVP, the optional Auto-tune assist, and a prototype gate that watches whether players skip scans out of boredom rather than choice.

**The three-band start.** Before the first Tuner upgrade the player has only Low, Mid, and High, so one weapon in three (Mid) cannot be fully blocked and the closer-is-stronger rule has less room to matter. Mitigation: the first Tuner tier is priced to be reached within the first few sessions, Mid weapons can be weighted lower in the three-band pool, the prototype can switch between three and five bands, and the prototype gate checks that the first sessions still feel like tuning rather than luck.

**Combat state feeling arbitrary.** If travel mode or presets lock for reasons the player cannot see, or the state flickers at the edge of the radius, it will read as the game being unfair. Mitigation: the combat lamp and message-bar reason, the calm period before the state ends, a radius set just beyond the visible game view so anything the player can see counts, and a chase radius that lets a fast pilot shake a pursuer. The prototype gate watches for players who cannot tell why something is locked.

**Leash exploits and cheesy enemy behaviour.** A patrol-and-leash model invites kiting and lure-and-reset play, and a group that snaps home the instant the player crosses a line can look silly. Mitigation: hull damage is kept when a group resets, shields recharge only at the normal rate, ranged enemies are given chase radii larger than their weapon range, hunters ignore the leash, and the prototype gate watches for fights that are won by dancing on the boundary instead of by routing and tuning.

**Flight feel.** If immediate turning feels twitchy at modern frame rates and screen sizes, the whole game suffers. Mitigation: prototype flight first, tune turn rate, camera look-ahead, and boost before any other system.

**Routing as micromanagement — the reactor's own risk.** If routing feels like babysitting four bars, players will set it once and resent it. Mitigation: presets that set up each job before a fight, one-tap quick-shifts, instant effect with no cooldown, power changes the player can see on the ship, Auto-route assist as an option, and a prototype gate that specifically tests whether players re-route voluntarily in a second fight.

**Routing and tuning sharing the D-pad.** Because tuning is a held layer on the same pad as routing, a player who does not hold the left bumper firmly could route when they meant to tune, or the reverse. Mitigation: the layer is a hold, not a toggle; the HUD highlights the active layer; and the prototype gate watches for accidental routing during retunes.

**Quadrant equalisation mistuned.** If quadrants equalise too fast, facing stops mattering and quadrants become decoration; too slow, and one unlucky pass becomes a death spiral. Mitigation: equalisation is a slow trickle by design, its rate is a named tuning value in the prototype, and the prototype gate checks that focused fire can break a side and that a pilot who turns a broken side away survives longer than one who does not.

**Heat as frustration.** Reactor heat can read as the game saying "stop playing." Mitigation: generous efficiency at standard power levels, clear heat feedback, heat that only bites during sustained boosted firing, and cooling that is part of routing rather than a forced pause.

**Samey contracts.** Templated jobs can read as repeats. Mitigation: vary the enemy mix, intel quality, and route across the five parameters, use the optional twists, and watch playtest logs for repeats.

**IP separation.** This is a spiritual successor. No original Solar Winds characters, story text, art, music, or assets may be used, and the final title, hero, and factions must be checked for trademark conflicts before any public release.

**Solo-developer bandwidth.** Art and audio are the most likely bottlenecks. Mitigation: a simple, consistent art style chosen for producibility, and licensed or commissioned audio rather than a bespoke score at MVP stage.

## Open decisions — to be made by the creators

| Decision | Options on the table | Needed by |
| --- | --- | --- |
| **Final title** | Solar Storms — chosen; check for trademark conflicts before public release | Decided |
| **Contract generation** | Decided: parameterised templates for bounty, cargo, and escort plus authored story contracts; a procedural side-mission generator may be added later | Decided |
| **Retune controls** | Decided: hold left bumper to switch the D-pad to tuning, explicit up and down per band, clamped at the ends of the player's range; keyboard uses dedicated keys | Decided |
| **Enemy bands** | Decided: each ship's weapon and shield bands are each randomly drawn from the bands the player's Tuner can reach when the contract is built and stored with it; the first Dart and first Mauler are authored with fixed bands; weighting within a pool is a possible later tuning option | Decided |
| **Shield band before a scan** | Decided: a ship's shield band is not shown before a scan; its weapon band shows from the beam colour | Decided |
| **How a scanned ship's shield band shows** | Decided: a small band tag beside the ship (glyph in the band's colour with its pips), shield band only; the outline stays white and carries strength only, with a one-shot colour flash when the scan completes; unscanned ships in range show an empty bracket; stale ships show a hollow, dim tag | Decided |
| **"Hostiles nearby" / combat state** | Decided in outline: combat state while any hostile is within a fixed engagement radius (set just beyond the game view, not scaled by Sensors) or is alert; it ends after about five seconds with none; an alert group disengages when the player leaves its chase radius; it locks travel mode and presets, and nothing else. All distances and times are starting values to tune in the prototype | Phase 0 (values) |
| **Enemy group behaviour** | Decided: groups of one to three ships share a home point, a patrol radius (wander), and a chase radius (leash); damage is kept on reset; hunters ignore the leash; fleeing targets ignore it; ranged enemies get a chase radius beyond their weapon range; bounty briefings mark the patrol circle, with accuracy set by intel quality. MVP is wander, notice, chase, and return; waypoint routes, schedules, sleeping groups, and reinforcements come later. Radii and ratios are tuning values | Phase 0 (values) |
| **Band names and end bands** | Decided: Infra, Low, Mid, High, Ultra; the player starts with Low, Mid, High and the first Tuner upgrade unlocks Infra and Ultra; further end-band specials (extra reactor cost, recalibration that grows with distance) remain optional for Phase 3 | Decided |
| **Band colours, shapes, pips, pitches** | Decided: deep red bar, amber circle, cyan diamond, magenta triangle, white star, with 1 to 5 pips and five ascending pentatonic notes; to be verified in a colour-blindness simulator and on small speakers before locking | Phase 0 |
| **Mid-weapon weighting in the three-band pool** | Uniform to start; lower weighting if the three-band start feels too punishing | Phase 0 |
| **First Tuner tier price** | Priced so most players reach it within their first few sessions | Phase 1 |
| **Art style detail** | Flat illustrated vs. lightly shaded / textured 2D | Phase 2 |
| **Campaign length target** | Focused ~6–8 hrs vs. larger ~12–15 hrs | Phase 3 |
| **Difficulty default** | Story / Standard / Hard naming and tuning | Phase 2 |
| **Monetisation (if any)** | Premium only is the working assumption; no decision taken | Before any public announcement |
| **Voice acting** | Text-only (working assumption) vs. partial VO for story jobs | Phase 3 |
| **Routable systems beyond the MVP four** | Cooling is designed in Section 5; order and tuning to confirm. Damage Control is decided — it is not routable; it stays a passive upgrade and station repair function | Phase 3 — existence of routing itself is decided, not open |
| **Camera look-ahead** | Decided: on by default, with a player setting to turn it off; leads along velocity, distance and smoothing are tuning values | Decided |
| **Reactor power levels** | Decided for the prototype: 100 units, Balanced 25 each, floor 5, cap 60; one shared piecewise-linear effect curve (0.6x / 1.0x / 1.3x); preset values as in Section 5. All are tuning values | Decided |
| **Overheat behaviour** | Decided: a lockout — at max heat the beam and boost stop until heat falls below a release threshold, with a HUD and audio cue; boost has no separate meter | Decided |
| **Top speed vs bolt speed** | Decided: top speed, including capped Engines plus boost, stays below bolt speed; the cap is a tuning value (starts at 14 u/s) | Decided |
| **Preset input** | Open — no controller or keyboard input applies a preset yet; debug keys in the prototype; the power panel is the likely home | Phase 1 |
| **Auto-route assist default** | Off by default with a first-session prompt vs. on by default | Phase 2 |
| **Auto-tune assist default** | Off by default with a first-session prompt vs. on by default | Phase 2 |
| **Shield quadrant equalisation rate** | Slow trickle rate and recharge split across quadrants to tune in the prototype; the quadrant model itself is decided, not open | Phase 0 |
| **Custom preset slots** | How many player-saved presets, and whether they can be shared | Phase 3 |

Scope, platform order beyond PC-first, and tech stack were framed in early discussion but are treated in this draft as proposals to lock at the prototype gate — Unity / C# and PC-first are the recommended defaults, not irreversible commitments.

# 16. Success criteria

**Prototype:** a player who has never seen the game can hit a moving Dart within one minute of picking up the controller — and, in a second fight against a Mauler, re-routes power without prompting, turns a damaged shield quadrant away to protect it, and can say what they gave up to get it. In a third fight, against a Dart with rolled bands, the same player scans it, sets the correct bands within a few seconds of contact, and can say how they knew them — from the beam colour, the scan, or the first-hit verdict — without using the word 'guessed'.

**MVP:** the uncoached full-loop test in Section 13 passes with three different first-time players, including its routing and tuning requirements.

**Vertical slice:** players finish a 30-minute session and can retell the story setup — the disappearances — in their own words.

**Release:** a complete campaign that respects the pillars, runs cleanly on PC and Xbox, and contains no material from the original game it honours.

*Next step after this document: build Phase 0 — one ship, one empty arena, one Dart with its bands, and the four-system reactor with four-quadrant shields, scanning and retuning — and tune flight, routing, tuning, and reactor heat until they feel right together. Nothing else is scheduled until that prototype is played.*

---

*Game Design Document • Working draft — decisions marked "Open" are not yet locked • Original work; not affiliated with Solar Winds or Epic Games*
