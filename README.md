# Arena-Blasters
# Arena Shooter

A top-down twin-stick arena shooter built in C# with the MonoGame framework. Battle an AI-controlled enemy in single-player mode, or grab controllers with friends for 2-4 player local multiplayer.

Built as a final project for ICS4U1.

## Gameplay

- **Single-player**: Face off against an AI enemy that moves unpredictably and fires bullets aimed at your position.
- **Local multiplayer**: Connect 2-4 controllers and the game automatically switches to multiplayer, where it's every player for themselves. Last one standing wins.
- **Health & lives**: Each character starts with 100 HP and 3 lives. Taking a hit costs 25 HP; running out of lives means you're out.
- **Power-ups**: Spawn periodically around the map. Pick one up to heal 25 HP if you're hurt, or get a temporary 2x speed boost if you're already at full health.

## Controls

This game is built for **gamepad/controller input only** (no keyboard support):

| Input | Action |
|---|---|
| Left Stick | Move |
| Right Stick | Aim / face direction |
| Right Trigger | Shoot |
| Back Button | Quit game |

The game automatically detects how many controllers are connected at launch and sets up single-player or multiplayer accordingly.

## Tech Stack

- **Language**: C#
- **Framework**: [MonoGame](https://www.monogame.org/) (WindowsDX) targeting .NET 8
- **Platform**: Windows

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- One or more Xbox/compatible controllers (required — there's no keyboard fallback)

### Build & Run
```bash
git clone <your-repo-url>
cd <repo-folder>
dotnet restore
dotnet run
```

The first build restores the MonoGame content pipeline tools automatically, which may take a moment.

## Project Structure

- `Game1.cs` – Main game loop: initialization, update, and draw logic
- `Character.cs` – Base class for shared health/lives/movement logic
- `Player.cs` – Controller input, movement, aiming, and shooting for players
- `Enemy.cs` – AI behavior for the single-player opponent
- `Weapon.cs` / `Projectile.cs` – Bullet firing and management
- `PowerUp.cs` / `GameItem.cs` / `MovingGameItem.cs` – Pickup and item logic
- `Content.mgcb` – MonoGame content pipeline (sprites, fonts, sound)

## Credits

Created by Om Patel.
