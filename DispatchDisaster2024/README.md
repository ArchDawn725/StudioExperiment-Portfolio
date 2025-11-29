# **Dispatch Disaster (2024)**

Studio Prototype • Large Simulation • Free on Itch.io (Playable)

*Itch.io Page:*

Availability: Free

# **⭐ Project Overview**

This project is a modern reimagining of my original 2020 Dispatch Disaster prototype, rebuilt with a clean architecture, improved coding standards, and more accessible gameplay. Development began as part of an experimental studio initiative I self-funded to assess whether small teams could form the foundation of a long-term studio.

The project was developed by only two people—myself (programming, design, systems) and a contracted 3D artist as well as a couple of QA testers. The art pipeline fell significantly behind schedule and required a renegotiation of the artist’s bid, which I was unable to approve due to budget constraints. The project was cancelled before reaching mobile deployment but remains fully playable in its current state and stands as a demonstration of clean code, strong system design, and my ability to manage a studio team with limited resources.

# **🎮 Gameplay Summary**

*Dispatch Disaster retains the spirit of the original 2020 concept but introduces new systems and a streamlined single-player progression loop.*

## *Core Gameplay Loop*

-Select a level

-Assign upgrade points

-Vehicle movement

-Income generation

-Objective response time

-Choose your agency: Fire, Police, or Medical

-Select a spawn location for your home base

-Purchase your first vehicle

-Navigate a procedurally generated map using A* pathfinding

-Respond to emergencies before they expire

-Manage regional reputation (0 = lose, 100 = win)

## *Agencies & Vehicles*

**Each agency has:**

-3 vehicle types with unique stats

-A high-tier vehicle with a special ability (e.g., helicopters for Medical)

-A dedicated home base (hospital, fire station, jail)

-Transport-based rewards depending on objective type

## *Strategic Considerations*

-Highways double movement speed

-Busy streets reduce movement speed

-Different emergencies have different regional effects

-Transport objectives require returning to home base

-Regions gain or lose points based on performance

## *Progression*

-Completing a level unlocks larger and more complex levels

-Every level introduces new zones or increased map sizes

## *Public Confidence System*

-The central lose condition simulates real 911 agency performance:

*If any region’s reputation falls to zero, you lose control of that region—resulting in overall failure.*

# **🧩 Key Features**

-Fully playable 2024 reimagining of the original Dispatch Disaster

-Procedural map generation using coroutine-driven safe placement

-Custom A* pathfinding built around street intersection graphs

-Region reputation scoring with win/lose conditions

-Agency-based asymmetrical gameplay

-Upgrade system affecting every aspect of gameplay

-Clear, accessible gameplay loop (tested at public events)

-Strong coding standards and a predictable architectural layout

-Designed for mobile compatibility (though not released on mobile)

-Single-reference string system to eliminate typo errors

-Free on itch.io for public testing

# **🏗️ Architecture Overview**

## This 2024 version showcases some of your cleanest architectural work pre-2025. The project features:

*-Major Improvements Over the 2020 Version*

-No god scripts

-Strong separation of responsibilities

-Consistent naming conventions

-Clear folder organization

-Heavy use of coroutines for sequenced events

-Reduced Update usage at every layer

-All string references consolidated into a single constants file

-Modular pathfinding, map generation, and emergency systems

-Planned mobile-friendly performance constraints

## *Key Systems*

-*A** *Pathfinding System*

Pathfinding built using intersection nodes, street weights, and dynamic cost modifiers.

-*Map Generation System*

Coroutine-driven to avoid heavy frame spikes, with zone-size minimums and failsafes.

-*Emergency Spawn System*

Generates priority-based objectives with expiration timers.

-*Region Reputation System*

Enforces core win/lose conditions and drives agency performance.

-*Upgrade System*

Player-assigned upgrades that modify gameplay variables.

-*Vehicle Controller*

Handles movement, speed modifiers, fuel, return-to-base logic, and rewards.

## *Code Quality Notes*

**This project demonstrates:**

-My strongest early architecture

-My ability to write readable, maintainable code for non-technical collaborators

-Considerations for team scalability

-Extreme optimization consideration for mobile targets

# **🗂️ Key Scripts to Review**

### *Core*

GameController – orchestrates the level flow, agency selection, upgrades

LevelController – unlock logic, loading, region initialization

### *Systems*

AStarPathfindingSystem – intersection-based, highway/busy-street weighting

VehicleMovementSystem – handles movement, fuel, special abilities

EmergencySpawnSystem – timed emergencies with region effects

ReputationSystem – manages region scoring and win/lose logic

UpgradeSystem – global modifiers applied at mission start

TransportSystem – handles hospital/fire station/jail deliveries

### *Managers*

MapGenerationManager – coroutine-driven procedural map creation

VehicleManager – vehicle purchasing, initialization, and pooling

EmergencyManager – active objective tracking

### *UI*

UpgradeUIController – assigns points before each mission

HomeBasePlacementUI – interactive base placement

EmergencyUI – displays timers, zones, and warnings

### *Utilities*

StringConstants – single reference for all gameplay strings

### *MathHelpers*

CoroutineUtilities

# **🧪 Development Notes**

## *Map Generation*

-Built entirely at runtime

-Zero stalling thanks to coroutine batching

-Failsafes guarantee zone minimum sizes

-Randomization ensures variety across levels

## *Pathfinding*

-Custom A* algorithm

-Street types adjust movement cost

-Works seamlessly with player click-to-move input

## *Public Testing*

**The game was showcased at events, and:**

-New players consistently understood the mechanics

-Players showed high retention

-Gameplay loop was intuitive after a short explanation

This was a key validation of the design direction.

# **🚧 Why This Project Matters**

**This project illustrates several important strengths:**

-My ability to lead and manage a small studio team

-My skill in writing code that others can learn from with minimal onboarding

-My significant improvement in architecture, cleanliness, and readability

-My ability to build polished gameplay systems quickly

-My capacity to deliver a fully playable prototype with almost no budget

-My capability to safely rebuild and modernize older flawed systems

-My passion for optimization, consistency, and development discipline

In short, this project shows what you can accomplish when you prioritize engineering quality and player clarity.

# **📚 Lessons Learned**

-Importance of clear contracts and timelines when working with contractors

-How clean coding standards empower non-technical teammates

-How to design scalable systems even in small teams

-That reboots often offer opportunities to fix foundational problems

-How to build mobile-friendly systems even before mobile deployment

-How to craft gameplay that is immediately understandable to new players

# **🛠️ Tech Stack**

Unity 2022.3

C# (Object-oriented + Coroutine-driven sequencing)

Custom A* pathfinding

Itch.io WebGL/Desktop deployment

ScriptableObject-based agencies and upgrades
