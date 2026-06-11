# Programming-Theory-repo
This is a basic Unity project that uses all 4 OOP pillars: Inheritance, Polymorphism, Abstraction, and Encapsulation.

---

**Unity Junior Programmer Pathway / Submission: Programming theory in action**

**2023**

## 🧠 OOP Implementation

- **Inheritance:** `PlayerHealth` and `EnemyHealth` inherit from `HealthManager`; `PlayerAttack` and `EnemyAttack` inherit from `AttackManager`
- **Polymorphism:** `PlayerAttack` and `EnemyAttack` override the virtual `Attack()` method with unique behavior; `TakeDamage()` is overridden in `PlayerHealth` to update UI
- **Abstraction:** `WaveManager` spawns waves without exposing details; `ChasePlayer` handles enemy AI logic; `MeleeAttackHandler` manages combat without exposing internals
- **Encapsulation:** `Health` property in `HealthManager` uses a private `_health` field with protected getter/setter to control access and prevent invalid states
