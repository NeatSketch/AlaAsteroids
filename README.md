# AlaAsteroids

A simple implementation of a game similar to Atari's Asteroids made with Unity.

This is NOT a faithful recreation of the original game.

It has only the most basic features to show how I approach prototyping games.

## Prerequisites
- Unity 2020.1.x

## Runtime Platform
The game is currently meant for Desktop Standalone platforms.

Build the game to play in windowed mode.

## Player Controls
- **Left/Right arrows:** rotate the ship
- **Up arrow:** accelerate
- **Space:** fire the on-board weapon

## Gameplay
Rotate and accelerate the ship to move it in the play area. Avoid getting hit by the asteroids.

If you get hit you lose a life. You have 3 lives in the beggining of each game session.

Shoot the asteroids to destroy them & gain score points. Get 50, 100 or 150 depending on the size of the asteroid.

When the asteroids are hit hard or shot, they will break and sometimes produce debris.

## Project Structure
### No visual assets
The project imitates the original Asteroids' visual appearance where all objects are represented by a bunch of line segments.
Everything in this implementation is drawn using LineRenderer.
That's why there are no visual assets in the project.

### Asmdefs
The project uses 2 asmdefs:
- **NeatSketch.AlaAsteroids:** defines the main assembly
- **NeatSketch.UnityUtils:** defines the assembly for cross-project reusable utils & stuff

### Asset Folders
The assets are separated by their purpose, not by their type.
- **Scenes:** the scene
- **Core:** the game manager
- **Utils:** contains the singleton class
- **Asteroids:** everything related to asteroids operation
- **Ship:** everything related to the player's ship operation
- **UI:** user interface classes

### Classes
- **GameManager:** starts & ends the game, respawns the ship, counts the score & lives, notifies other parts of the code about game events
- **Asteroid:** operates an asteroid, creates its geometry, spawns debris, notifies the game manager about getting shot
- **AsteroidGenerator:** spawns new asteroids around the play area & sends them to the player's demise
- **OutOfBoundsAsteroidCleaner:** destroys asteroids that go out of bounds
- **Ship:** operates the ship, spawns bullets
- **Bullet:** propels the bullet forward & destructs it
- **UIManager:** decides when to show the 'Game Over' screen
- **ScoreCounter:** monitors & shows the score
- **LifeCounter:** monitors & shows the lives
- **SingletonMonoBehaviour:** I use this class across multiple projects to implement singletons
