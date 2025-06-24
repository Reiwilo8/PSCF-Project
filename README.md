# PSC-F Project

# 🎮 Tic-Tac-Toe (TTT) 5x5 with Q-Learning AI

A cross-platform Unity game that reimagines the classic Tic-Tac-Toe on a 5x5 board with a twist: the first player to align 4 symbols in a row wins. Supports both PvP and AI (Q-Learning) modes, including difficulty levels and custom AI behavior.

## 🧠 Features

- ✅ **Two Game Modes**: Player vs Player (PvP) and Player vs AI (PvE)
- 🧠 **Q-Learning AI** with 4 difficulty levels:
  - Easy, Medium, Hard — each with different learning parameters
  - Custom — user-defined alpha, gamma, and epsilon (0–1 range)
- 📈 **Persistent Stats**: Tracks total play time, wins, draws, and rounds across sessions
- 💾 **Q-table Persistence**: AI learns and stores strategies between games
- 🛠️ **Resettable Data**: Stats and learned AI can be reset via settings
- 📱 **Multi-platform**: Tested on Windows and Android; should work on Mac, Linux, iOS
- 🖱️🎯 Supports **mouse input** and **touch screens**

---

## 🚀 Getting Started

### 📦 Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Reiwilo8/PSCF-Project.git
   
2. **Open with Unity Hub**:
   - Add the project folder using the "Open" or "Add" option
   - Open the project using **Unity version 2021.3.x** or newer

3. **Set Target Platform** (optional):
   - Go to `File > Build Settings`
   - Select your desired platform (e.g. Windows, Android, iOS)
   - Click **Switch Platform** to apply changes

---

### 🧰 Compile Settings

- You can choose the scripting backend under:
  - `Edit > Project Settings > Player > Other Settings > Configuration`
- Available options:
  - **Mono** – faster builds, great for development
  - **IL2CPP** – better performance, required for iOS or release builds

> ⚠️ For mobile builds (especially iOS), use **IL2CPP** and ensure proper SDKs are installed.

---

## 🕹️ Controls

- **Mouse input** (Windows/Mac/Linux)
- **Touch support** (Android/iOS)
- UI buttons allow:
  - Game reset
  - Mode and difficulty selection
  - Access to statistics and Q-learning parameters (in Custom mode)

---

## 📊 Stats & AI Learning

### 🎓 Q-Learning AI

- AI plays as either **X** or **O** depending on player order
- Learns over time using a persistent Q-table
- Board state is serialized into strings like `"XO_OX___..."` for lookup

### 💡 Epsilon-Greedy Policy

AI chooses moves based on the following strategy:
- **Exploration** (random moves) with probability ε
- **Exploitation** (best-known move) with probability 1−ε

### 🔧 Difficulty Parameters

| Difficulty | Epsilon (ε) | Alpha (α) | Gamma (γ) |
|------------|-------------|-----------|-----------|
| Easy       | 0.9         | 0.3       | 0.5       |
| Medium     | 0.5         | 0.5       | 0.7       |
| Hard       | 0.1         | 0.7       | 0.9       |
| Custom     | User-defined (range 0.0 – 1.0) for each parameter |

### 💾 Data Persistence

| File          | Description                        | Location                                     |
|---------------|------------------------------------|----------------------------------------------|
| `qtable.json` | Stores Q-learning state data       | `Application.persistentDataPath/qtable.json` |
| `stats.json`  | Stores win/draw/time statistics    | `Application.persistentDataPath/stats.json`  |

You can reset both via the in-game **Settings** screen.

---

## 📂 Project Structure

```
Assets/
├── Materials/                 # Optional materials used in scenes
├── Prefabs/
│   ├── Scripts/               # Script-based prefab logic
│   └── UI/                    # UI prefab elements (buttons, panels)
├── Resources/                 # Stored assets and data files
├── Scenes/                    # Main Unity scenes
├── Scripts/
│   ├── AI/                    # Q-Learning logic and AI behavior
│   ├── Managers/              # StatsManager, GameManager, etc.
│   └── UI/                    # UI controllers and logic
├── Settings/                  # ScriptableObjects or config assets
├── Sprites/                   # 2D sprite graphics
├── TextMesh Pro/              # Fonts and TMP assets
Packages/                      # Unity package manifest and dependencies
```

---

## 📜 License

This project is private.

All rights are reserved by the author. No part of this project, including source code, assets, or binaries, may be copied, distributed, modified, or used in any form without explicit written permission from the author.

This repository is intended for educational and portfolio purposes only.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED.

---

## 🙌 Credits

Developed as a **university project**  
All scripts written in **C#** using **Unity**  
Created by Oliwier Oblicki and Zoreslav Sushko
