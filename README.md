# Programming Theory Project
A simple 3D game where you fight waves of enemies and pick up health packs to stay alive. 

It was built in Unity to demonstrate the four pillars of Object-Oriented Programming (OOP): Inheritance, Polymorphism, Abstraction, and Encapsulation.

---

## 🚀 Quick Start

Want to skip the code and just play? 

1. **[Download the Repository](https://github.com/Frext/Programming-Theory-repo/archive/refs/heads/main.zip)**

3. Extract the ZIP file.

4. Open the **`BuildWindows`** folder and run the game executable named **`Programming Theory Project.exe`**.

---

## 🎮 Controls

| Key | Action |
| :---: | :--- |
| **W A S D** | Move |
| **SPACE** | Jump |
| **LEFT CLICK** | Attack |
| **V** | Change Camera View |

---

## 📸 Screenshots

| Main Menu | Controls |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/ac41a1b4-32ff-47d0-8f43-ad3f0a3bbe7e" alt="Main Menu"/> | <img src="https://github.com/user-attachments/assets/f511cbde-18f3-44c8-a5e8-b47d1d507824" alt="Controls"/> |

| First-Person View (Wave 1) | Third-Person View (Wave 1) |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/743fb90e-acd5-46eb-a997-4e62904e1ef0" alt="First-Person View"/> | <img src="https://github.com/user-attachments/assets/c4b545f1-56aa-4c5d-9be7-9cdc1fb21aeb" alt="Third View"/> |

| Multiple Skeleton Enemies (Wave 2) | Health Pickup |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/26f79955-357a-4440-81a2-077087e58704" alt="Multiple Skeleton Enemies"/> | <img src="https://github.com/user-attachments/assets/3e658c0c-7938-4414-8ddf-07cefd229318" alt="Health Pickup"/> |

| Orc Enemy (Wave 3) | Death Screen |
|:---:|:---:|
| <img src="https://github.com/user-attachments/assets/243bc4f8-3ec5-464e-bdb4-ac896b738a1a" alt="Orc Enemy"/> | <img src="https://github.com/user-attachments/assets/aae0a9a9-0c80-4069-8a10-1230c590968f" alt="Death Screen"/> |

---

## 🧠 OOP Implementation

- **Inheritance:** `PlayerHealth` and `EnemyHealth` inherit from `HealthManager`; `PlayerAttack` and `EnemyAttack` inherit from `AttackManager`
- **Polymorphism:** `PlayerAttack` and `EnemyAttack` override the virtual `Attack()` method with unique behavior; `TakeDamage()` is overridden in `PlayerHealth` to update UI
- **Abstraction:** `WaveManager` spawns waves without exposing details; `ChasePlayer` handles enemy AI logic; `MeleeAttackHandler` manages combat without exposing internals
- **Encapsulation:** `Health` property in `HealthManager` uses a private `_health` field with protected getter/setter to control access and prevent invalid states

---

## 🛠️ Built With

* **Engine:** Unity (2021.3.9f1)
* **Language:** C#
* **Course:** Unity Junior Programmer Pathway (Submission: Programming Theory in Action)

---

**2023**


