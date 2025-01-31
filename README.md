Game is a dodging game - spheres fall from the sky, and you control a cube. If your cube gets hit you die.

Put Scripts + Prefabs into Assest folder. I think it should run - if it doesn't, copy steps below.

Create Scripts and Prefabs folder inside Assets

PlayerCube:
Hierarchy → Right-click → 3D Object → Cube
Rename to "PlayerCube".
Inspector Settings:
Transform → Position: (0, 0, 0)
Tag: "Player" (Create if missing).
Add Components:
BoxCollider →  Enable "Is Trigger".
Rigidbody → Is Kinematic = true, Use Gravity = false.
Make Prefab:
Drag PlayerCube to Assets/Prefabs/.
Delete it from the Hierarchy.

FallingSphere:
Hierarchy → Right-click → 3D Object → Sphere
Rename to "FallingSphere".
Inspector Settings:
Transform → Position: (0, 6, 0)
Tag: "FallingObject" (Create if missing).
Add Components:
SphereCollider →  Enable "Is Trigger".
Rigidbody → Is Kinematic = false, Use Gravity = false, Collision Detection = Continuous.
Make Prefab:
Drag FallingSphere to Assets/Prefabs/.
Delete it from the Hierarchy.

GameManager:
Hierarchy → Right-click → Create Empty
Rename to "GameManager".
Inspector → Click "Add Component" → Add "GameManager (Script)" (after creating it).
Assign Prefabs:
Drag PlayerCube.prefab into "Player Prefab" slot.
Drag FallingSphere.prefab into "Falling Sphere Prefab" slot.

Attach PlayerController.cs to GameManager
Attach PlayerController to cube
Attach FallingObject to Sphere
