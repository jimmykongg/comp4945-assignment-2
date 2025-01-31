namespace TagGame.Scripts
{
    using System;
    using UnityEngine;

    public class GameLogic
    {
        public event Action<int> OnFlagCaptured; // Event for scoring
        public event Action<Vector3> OnLocalCubeMoved;
        public event Action<Vector3> OnRemoteCubeMoved;
        public event Action<Vector3> OnFlagMoved; // Event for updating the flag position
        public event Action<int> OnPlayerTagged; // Event for tagging

        private Vector3 localCubePos;
        private Vector3 remoteCubePos;
        private Vector3 flagPos;
        private Vector3 localBasePos = new Vector3(-20f, 0f, 0f);
        private Vector3 remoteBasePos = new Vector3(20f, 0f, 0f);

        private int flagHolder = -1; // -1 means flag is not held, 0 = local player, 1 = remote player
        private int itPlayerId = 0; // 0 = local player is "It", 1 = remote player is "It"

        public GameLogic()
        {
            localCubePos = new Vector3(-3.0f, 0.0f, 0.0f);
            remoteCubePos = new Vector3(3.0f, 0.0f, 0.0f);
            flagPos = new Vector3(0.0f, 0.0f, 0.0f); // Start flag in the middle
        }

        public Vector3 LocalCubePos => localCubePos;
        public Vector3 RemoteCubePos => remoteCubePos;
        public Vector3 FlagPos => flagPos;

        public void MoveLocalCube(float x, float y)
        {
            localCubePos += new Vector3(x, y, 0.0f);
            OnLocalCubeMoved?.Invoke(localCubePos);
            UpdateFlagPosition();
            CheckTagging();
            CheckFlagInteraction();
        }

        public void UpdateRemoteCube(Vector3 newPos)
        {
            remoteCubePos = newPos;
            OnRemoteCubeMoved?.Invoke(remoteCubePos);
            UpdateFlagPosition();
            CheckTagging();
            CheckFlagInteraction();
        }

        private void UpdateFlagPosition()
        {
            if (flagHolder == 0) // If local player has the flag
            {
                flagPos = localCubePos + new Vector3(0.5f, 0.5f, 0); // Slight offset for visibility
                OnFlagMoved?.Invoke(flagPos);
            }
            else if (flagHolder == 1) // If remote player has the flag
            {
                flagPos = remoteCubePos + new Vector3(0.5f, 0.5f, 0);
                OnFlagMoved?.Invoke(flagPos);
            }
        }

        private void CheckTagging()
        {
            // If local player is "It" and tags the remote player
            if (Vector3.Distance(localCubePos, remoteCubePos) < 1.5f && itPlayerId == 0)
            {
                TagPlayer(1);
            }
            // If remote player is "It" and tags the local player
            else if (Vector3.Distance(localCubePos, remoteCubePos) < 1.5f && itPlayerId == 1)
            {
                TagPlayer(0);
            }
        }

        private void TagPlayer(int newItPlayerId)
        {
            Debug.Log($"Player {newItPlayerId} is now 'It'");

            // If the tagged player was holding the flag, transfer it to the tagger
            if (flagHolder == newItPlayerId)
            {
                flagHolder = itPlayerId; // The previous "It" now has the flag
                Debug.Log($"Player {itPlayerId} stole the flag!");
            }

            itPlayerId = newItPlayerId; // Switch who is "It"
            OnPlayerTagged?.Invoke(itPlayerId); // Notify UI
        }

        private void CheckFlagInteraction()
        {
            // Pick up the flag if no one holds it
            if (flagHolder == -1)
            {
                if (Vector3.Distance(localCubePos, flagPos) < 1.0f)
                {
                    flagHolder = 0;
                    Debug.Log("Local player picked up the flag!");
                }
                else if (Vector3.Distance(remoteCubePos, flagPos) < 1.0f)
                {
                    flagHolder = 1;
                    Debug.Log("Remote player picked up the flag!");
                }
            }

            // Score if player reaches their base with the flag
            if (flagHolder == 0 && Vector3.Distance(localCubePos, localBasePos) < 1.0f)
            {
                ScorePoint(0);
            }
            else if (flagHolder == 1 && Vector3.Distance(remoteCubePos, remoteBasePos) < 1.0f)
            {
                ScorePoint(1);
            }
        }

        private void ScorePoint(int playerId)
        {
            flagHolder = -1;
            flagPos = new Vector3(0.0f, 0.0f, 0.0f); // Reset flag to center
            OnFlagCaptured?.Invoke(playerId);
            OnFlagMoved?.Invoke(flagPos); // Reset flag position
            Debug.Log($"Player {playerId} scored!");
        }
    }
}
