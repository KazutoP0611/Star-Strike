# 🚀 Star Strike (3D Rail Shooter Prototype)

### 🚀 Star Strike
**Star Strike** is a 3D rail-shooter prototype inspired by classic space combat games such as Star Fox 64.  
The project focuses on **smooth player flight control, responsive aiming mechanics, and cinematic movement behavior** while maintaining a structured gameplay foundation for future enemy encounters and boss battles.

The game features **automatic forward rail movement** while allowing the player to control the ship’s position within the combat space using mouse or keyboard input.

---

## ⚙️ Technical Highlights

- Engine: Unity 6000.3.6f1  
- Programming Language: C#  
- 3D Rail Shooter Movement System  
- Single and Dual Crosshair Targeting System  
- Particle-based Laser Shooting with Collision Detection  
- Smooth Damped Player Movement and Rotation  
- Unity Input Action System (Mouse Delta Input)  
- Boss AI foundation with state-based behavior (planned)

---

## ✈️ Player Flight System

The player ship moves automatically forward along a **rail path**, while the player controls lateral movement within the combat space.

### Core Movement Design

Player control is implemented using a **two-object movement system**:

1. **Invisible Target Object**
   - Controlled by player input (Mouse's Delta)

2. **Player Ship**
   - Smoothly follows the invisible target
   - Creates a delayed “lag follow” feeling similar to classic arcade flight controls

This approach creates **responsive but cinematic movement** where the ship naturally trails behind player input.

---

## 🎮 Dynamic Ship Rotation

To enhance visual feedback:

- **X/Y Axis**  
  The ship constantly **looks toward the invisible control object**

- **Z Axis (Roll)**  
  The ship rotates based on the **movement volume and direction**, creating a natural banking effect when turning.

This produces a flight style similar to classic arcade space shooters.

---

## 🧩 Flight Control Architecture (Invisible Target System)

One of the core technical implementations in this project is the **two-layer flight control architecture**, designed to create smooth and responsive ship movement while avoiding jittery input behavior.

### Problem

Directly applying player input to the ship transform often results in:

- Sudden snapping movement
- Unnatural rotations
- Poor visual weight for the aircraft

This is especially noticeable in **rail shooters**, where the ship should feel like it has **mass and inertia**.

### Solution

To solve this, the control system separates **input control** from **visual ship movement**.

```
Player Input
     │
     ▼
Invisible Control Target
     │
     ▼
Player Ship (Smooth Follow)
```

### Step 1 — Input Target

Player input (mouse delta) moves an **invisible control object** inside the movement bounds.

This object represents the **intended forward position of the ship**.

### Step 2 — Smooth Follow

The ship does not move directly to the target.  
Instead it **smoothly interpolates toward it**, creating a slight delay.

This produces:

- Natural movement lag
- Better visual weight
- Smoother flight feel

### Step 3 — Ship Orientation

The ship's rotation is calculated using two behaviors:

**Look Direction (X/Y Axis)**  
The ship continuously rotates to **look toward the invisible control target**.

**Banking Rotation (Z Axis)**  
Banking is calculated based on the **volume and direction of movement**, allowing the ship to tilt naturally when turning.

### Advantages of This System

- Smooth cinematic flight movement
- Reduced input jitter
- Natural aircraft-like banking
- Clean separation between **input logic and visual behavior**
- Easily expandable for future mechanics

### Future Expansion Possibilities

This architecture allows easy addition of:

- Barrel roll maneuvers
- Boost and brake mechanics
- Camera motion linked to ship velocity
- Dynamic combat movement during boss encounters

## 🎯 Single and Dual Crosshair(s) Targeting Systems

The aiming system uses **two crosshair systems (single and dual crosshair)** based on the ship’s forward direction.

The system works by projecting the ship's forward vector in front of the player.

With Dual Crosshairs, system use **two different distances** for both crosshairs.

This results in:

- A **near crosshair**
- A **far crosshair**

Because both are calculated from the ship’s forward transform, they **lag slightly when the ship rotates**, producing a natural aiming delay effect that enhances the feeling of momentum.

---

## 🔫 Laser Shooting System

Player shooting is implemented using:

- **Mouse Left Click** input
- Forward-firing laser particles
- Collision detection using **particle collision messages**

### Shooting Behavior

- The ship fires **forward lasers**
- Laser particles detect collisions and return hit information
- This approach allows fast projectile effects while keeping the system lightweight.

---

## 🎮 Controls

| Input | Action |
|------|------|
| Mouse Movement | Control ship position |
| A | Blank Left |
| D | Blank Right |
| Q | Barrel Roll Left |
| A | Barrel Roll Right |
| Spacebar | Boost and Brake |
| Left Click | Fire laser |

---

## 🤖 Enemy Systems (In Progress)

Enemy systems are currently under development.

Planned systems include:

- Enemy spawning
- AI movement patterns
- Boss enemy encounters
- Projectile combat
- Damage and health systems

### Boss AI (Early Setup)

The boss enemy will include a **state-based AI system** responsible for:

- Tracking player position
- Rotating toward the player
- Attacking at appropriate intervals

---

## 🎨 Visual Style

The project uses **stylized 3D assets** from Unity’s free asset collections.

The visual direction aims for:

- Clean stylized environments
- Arcade-style readability
- Smooth gameplay clarity during combat.

---

## 📌 Current Development Status

Current implemented features:

✔ Player rail movement system  
✔ Smooth damped flight control  
✔ Ship rotation and banking system  
✔ Two crosshair aiming systems  
✔ Particle-based laser shooting  
✔ Enemy and boss models added to the scene  

Still in development:

⬜ Enemy movement and AI  
⬜ Level design  
⬜ Boss battle mechanics  
⬜ Boost / brake system  
⬜ Barrel roll maneuver  
⬜ Camera motion system  

---

## 🧠 Development Focus

This project emphasizes:

- Responsive player flight control
- Clear shooting mechanics
- Expandable combat architecture
- Smooth and cinematic ship behavior

The goal is to create a **solid gameplay foundation** before implementing more complex enemy and level systems.

---

## 🚧 Project Status

**Active Prototype**

This project is currently focused on **core gameplay systems and flight control mechanics** before expanding into full level and enemy design.

---

## 🎮 Design Inspirations & Gameplay Philosophy

The gameplay direction of **Star Strike** is inspired by classic rail shooters such as Star Fox 64, focusing on **tight controls, readable combat space, and satisfying flight behavior**.

However, rather than directly copying mechanics, this project focuses on understanding **why these games feel good to play** and rebuilding those ideas through modern development tools.

### 🧠 Development Insight

The flight control system was not the first implementation.

The initial version directly applied player input to the plane transform.  
While functional, the movement and rotation felt too static and robotic.

After studying the movement style of classic rail shooters such as Star Fox, the system was redesigned using a **two-layer control architecture**:

Input → Invisible Target → Ship Follow

This allowed the ship to have a slight delay and inertia, producing a much more natural flight feeling.

The final result is not intended to perfectly replicate existing games, but to capture the **responsive yet weighted flight behavior** common in arcade space shooters.

---

### Core Design Goals

#### Responsive but Weighted Flight

One of the key goals is to balance:

- **Responsiveness** (player input should feel immediate)
- **Weight** (the ship should feel like a physical object)

The invisible target movement system was designed specifically to achieve this balance.

---

#### Readable Combat Space

Rail shooters require the player to quickly read:

- Enemy positions
- Projectile directions
- Player movement boundaries

Because of this, the project uses:

- Clear aiming indicators (dual crosshair system)
- Forward-focused combat
- Limited movement area within the rail path

This keeps the gameplay readable even during high-speed movement.

---

#### Momentum-Based Aiming

Unlike static crosshair shooters, aiming in Star Strike reflects the **momentum of the ship**.

Because the crosshair system is derived from the ship's forward transform, turning the ship creates a **slight aiming delay**, reinforcing the sense of speed and movement.

---

#### Modular Combat Systems

The project architecture is designed so that future gameplay elements can be added without rewriting core systems.

Examples include:

- Enemy waves
- Boss behavior patterns
- Boost mechanics
- Barrel roll evasions
- Dynamic camera motion

---

## 🚀 Long-Term Vision

Star Strike is currently focused on **core player mechanics**, but the long-term goal is to expand into a full rail-shooter experience including:

- Cinematic level progression
- Large-scale boss battles
- Environmental obstacles
- Dynamic combat encounters

The project serves as both a **technical exploration of flight control systems** and a **foundation for building arcade-style shooter gameplay in Unity**.
