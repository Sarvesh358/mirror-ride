# Mirror Ride
A mobile game to help with motion sickness by synchronising real-driving movement with in-game driving.

# Mirror Ride - Starter 3D Project (Cloud Build friendly)

This is a minimal Unity 3D starter setup for the Mirror Ride project.
Files included:
- Minimal Main.unity scene
- StarterController, PlayerCarController, GhostCarController, PathManager, GPSManager, ScoreManager, UIManager
- EditorBuildSettings configured with Main.unity
- Packages manifest

NEXT STEPS:
1. Create Player and Ghost GameObjects in the scene (simple cubes) and attach respective scripts:
   - Player: PlayerCarController
   - Ghost: GhostCarController
   - Camera: SimpleCameraFollow (assign Player)
   - PathManager: assign Ghost and optional debug waypoints
2. Create a Canvas with ScoreText and ModeText UI elements and hook UIManager
3. Commit & push to GitHub and trigger Unity Build Automation (Cloud Build)

Notes:
- GPS integration and Google Maps Directions integration are placeholders. We will implement polyline decoding and coordinate mapping later.
- This starter is intentionally minimal for fast Cloud Build iterations.
