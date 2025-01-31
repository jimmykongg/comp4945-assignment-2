using System;
using UnityEngine;

namespace UnityGame.Scripts
{
    public class GameLogic
    {
        public event Action<Vector3> OnLocalCubeMoved;
        public event Action<Vector3> OnRemoteCubeMoved;
        
        private Vector3 localCubePos;
        private Vector3 remoteCubePos;

        public GameLogic()
        {
            localCubePos = new Vector3(1.0f, 1.0f, 1.0f);
            remoteCubePos = new Vector3();
        }

        public Vector3 LocalCubePos => localCubePos; // Getter
        public Vector3 RemoteCubePos => remoteCubePos; // Getter

        public void MoveLocalCube(float x, float y)
        {
            localCubePos += new Vector3(x, y, 0.0f);
            OnLocalCubeMoved?.Invoke(localCubePos);
        }

        public void UpdateRemoteCube(Vector3 newPos)
        {
            remoteCubePos = newPos;
            OnRemoteCubeMoved?.Invoke(remoteCubePos);
        }

        public bool CheckCollision()
        {
            return Vector3.Distance(localCubePos, remoteCubePos) < 2.0f;
        }
    }
}