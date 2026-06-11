# Programming-Theory-repo
A simple 3D game where you fight waves of enemies and pick up health packs to stay alive. 

It was built in Unity to demonstrate the four pillars of Object-Oriented Programming (OOP): Inheritance, Polymorphism, Abstraction, and Encapsulation.

---

## 🚀 Quick Start

Want to skip the code and just play? 

1. **[Download the Repository](https://github.com/Frext/Programming-Theory-repo/archive/refs/heads/main.zip)**

3. Extract the ZIP file.

4. Open the **`BuildWindows`** folder and run the game executable named **`Programming Theory Project.exe`**.

---

## 🧠 OOP Implementation

- **Inheritance:** `PlayerHealth` and `EnemyHealth` inherit from `HealthManager`; `PlayerAttack` and `EnemyAttack` inherit from `AttackManager`
- **Polymorphism:** `PlayerAttack` and `EnemyAttack` override the virtual `Attack()` method with unique behavior; `TakeDamage()` is overridden in `PlayerHealth` to update UI
- **Abstraction:** `WaveManager` spawns waves without exposing details; `ChasePlayer` handles enemy AI logic; `MeleeAttackHandler` manages combat without exposing internals
- **Encapsulation:** `Health` property in `HealthManager` uses a private `_health` field with protected getter/setter to control access and prevent invalid states

---

**Unity Junior Programmer Pathway / Submission: Programming theory in action**

**2023**


