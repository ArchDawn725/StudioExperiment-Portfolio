# **Line Wars (2024)**

A Mobile Tug-of-War Strategy Prototype • Studio Leadership Project • Cancelled

*Itch.io Page:*

Availability: Free

Line Wars was one of two experimental prototypes developed under my self-funded studio initiative in 2024. The goal was to assemble a small team, evaluate their long-term fit, and test whether a micro-budget studio could produce high-quality mobile games. Despite the team’s initial motivation, a lack of contribution from several members ultimately led to the project’s cancellation. Remaining budget was divided proportionally among contributors based on completed work.

## This prototype was developed by a small team:

-Me — Lead Programmer & Systems Architect

-Game Designer

-2D Artist

-Sound Designer

-3 Testers

*The project resulted in a fully functional and well-optimized gameplay prototype featuring clean code, structured architecture, tooling for designers, and fast-paced strategic gameplay.*

# **⭐ Project Overview**

Line Wars is a mobile, single-player tug-of-war strategy game where players build structures, spawn units, upgrade abilities, progress through a tech tree, and battle through multi-lane maps. The core focus was fast decision-making and smooth automated combat, supported by an extremely optimized tick-based simulation system designed to hold a steady 60 FPS on even low-end hardware.

*Although art assets remained low-quality due to team inexperience and budget constraints, the game systems themselves were production-ready, and the codebase represents some of my cleanest pre-2025 work.*

# **🎮 Gameplay Summary**
## *Menus & Progression*

### On launching the game, players are presented with:

**Upgrade Menu** — Spend points earned from matches to increase unit stats (damage, health, speed, spawn times, etc.)

**Research Menu** — Unlock new units via a tech-tree progression inspired by historical RTS titles, ending at the early Iron Age in this prototype

**Level Select** — Choose from multiple maps, each with escalating difficulty and unique lane configurations

## *Match Flow*

### Each level contains:

-3 branching lanes that split and merge, leading to enemy and player castles

-Buildable tiles behind each castle (2 rows)

-Buildings that auto-produce units after a countdown timer

-Units that march down lanes, fight enemies, and attack buildings and castles

## *Combat*

Units follow lane paths using a custom node-based pathfinding system

### When enemies or buildings are within range:

-Units charge to optimal attack distance

-Enter cooldown-based attack loops

-Deal damage through weapon hit detection

-Destroying the enemy castle wins the match

-Losing your own castle counts as defeat

-Players earn upgrade points whether they win or lose

## *Economy*

### Gold is earned:

-Periodically

-By killing enemy units

-By building certain structures

### Gold is then used to:

-Build new structures

-Upgrade production speed

-Spawn higher-tier units

# **🧩 Key Features**

-Mobile-ready single-player strategy design

-Highly optimized tick-based simulation system

-Custom node graph + pathfinding

-Clean, readable code with single-responsibility architecture

-Scriptable string catalog (no magic strings)

## Custom Unity tools for:

-Map creation

-Node generation

-Path connections

-Full upgrade system + research tree

-Multi-lane tug-of-war combat

-Buildings that auto-spawn units

-Gold-based resource loop

-Designer-friendly layout supporting rapid content creation

*Free on itch.io*

# **🏗️ Architecture Overview**

## The architecture of Line Wars reflects a major evolution in my engineering methodology:

### *Strengths*

-Clear folder organization

-Every script created for a single purpose (no god scripts)

-High readability for non-programmers

-Strong naming conventions

-Tick system ensures stable gameplay regardless of device speed

-Custom Unity Editor tools to avoid Play Mode for map building

-Fully event-driven upgrade and research systems

## Clean separation of:

-combat logic

-economy

-building behavior

-pathfinding

-data storage

-visuals

-*Tick System*

### The entire game (movement, spawning, combat) was controlled by a fixed tick clock:

-Ensures predictable timing

-Allows slower devices to keep simulation accuracy

-Guarantees 60 FPS flow on mobile

# **🗂️ Key Scripts to Review**

### *Core*

GameFlowController — manages menus → gameplay transitions

TickSystem — core fixed-time simulation loop

LevelController — loads maps, opponents, and difficulty

### *Systems*

BuildingSystem — countdown timers, spawn triggers

UnitCombatSystem — weapon hit detection, cooldowns, targeting

UnitPathfindingSystem — node-based path traversal

ResearchSystem — tech tree progression

UpgradeSystem — modifies player stat multipliers

EconomySystem — gold generation & rewards

### *Managers*

MapNodeManager — lane generation and editor integration

EnemySpawner — AI-controlled buildings & upgrades

UnitManager — pooling, spawning, tracking

### *UI*

UpgradeMenuUI — displays and applies upgrade points

ResearchMenuUI — unit unlocks and tree progression

LevelSelectUI — map selection and difficulty display

BuildingUI — countdown timers, readiness indicators

### *Utilities*

StringCatalog — centralized string reference store

EditorPathTool — custom inspector for lane creation

DamageHelper — stat-modified damage calculations

# **🧪 Development Notes**

## *Optimization*

-Tick-based loop → locked 60 FPS

-Strictly minimized Update usage

-Single-responsibility scripts → lower memory churn

-Unit logic grouped into batched operations

-Editor tools minimized need for runtime complexity

## *Team Leadership*

### I led:

-Engineering direction

-Architecture decisions

-Task planning and workload scaling

-Quality reviews

-Contributor payouts proportional to meaningful work

*This project demonstrates real studio management, even under heavy constraints.*

## *Gameplay*

-Core loop polished and intuitive

-Minimal tutorial required at playtesting events

-Strong retention from first-time players

-Clear decision-making trees for building placement & upgrades

# **🚧 Why This Project Matters**

## This project illustrates:

-My ability to lead a multidisciplinary team

-My skill in building mobile-optimized gameplay systems

-My capacity to write clean, readable code intended for collaborators

-Early mastery of editor tooling, tick systems, and pathfinding

-My ability to recognize when a project must be cancelled and execute that leadership decision responsibly

-Proof that you can create production-quality prototypes on micro budgets

-The foundation for my later improvements in async operations, pooling, and fluid combat

*Even though it was cancelled, Line Wars is one of the best demonstrations of my discipline, leadership, and technical consistency.*

# **📚 Lessons Learned**

-Not all team members will contribute equally

-Strong communication and accountability are essential

-Custom tick systems work well but async frameworks offer better scalability

-Fluid combat relies heavily on animation–logic syncing

-Good architecture dramatically reduces onboarding cost

-Sometimes cancellation is the most responsible decision

# **🛠️ Tech Stack**

Unity 2022.3

C#

Custom tick-based engine

Node graph + pathfinding tools

Unity Editor tool scripting
