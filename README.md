# Arena Shooter

A top-down twin-stick shooter I built in C# using MonoGame for my ICS4U1 final project. Fight a simple AI enemy on your own, or plug in a few controllers and play against friends.

## What it does

- Single player mode against an AI enemy that wanders around the map and shoots back
- Local multiplayer for 2-4 players using controllers, free-for-all style
- Health and lives system: 100 HP per life, 3 lives total, you're out once you lose them all
- Power-ups spawn on the map periodically. They heal you if you're hurt, or give you a temporary speed boost if you're already at full health

## Controls

Everything runs off a controller, there's no keyboard support:

- Left stick to move
- Right stick to aim / face a direction
- Right trigger to shoot
- Back button to quit

The game checks how many controllers are connected on startup and sets up single player or multiplayer automatically.

## Built with

C# and MonoGame (WindowsDX), targeting .NET 8. Windows only.

## Running it

You'll need the .NET 8 SDK and at least one controller.

```
git clone <your-repo-url>
cd <repo-folder>
dotnet restore
dotnet run
```

## Code overview

- `Game1.cs` - main game loop, handles updates, drawing, collisions, and power-up spawning
- `Character.cs` - shared health/lives logic used by both players and the enemy
- `Player.cs` - reads controller input and handles movement, aiming, and shooting
- `Enemy.cs` - AI opponent's movement and shooting behavior
- `Weapon.cs` / `Projectile.cs` - bullets
- `PowerUp.cs` / `GameItem.cs` / `MovingGameItem.cs` - pickups and shared item logic
- `Content.mgcb` - sprites, font, and sound assets

Made by Om Patel.
