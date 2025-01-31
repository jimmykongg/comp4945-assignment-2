using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject fallingSpherePrefab;
    public int totalPlayers = 4;
    private List<GameObject> players = new List<GameObject>();
    private bool gameOver = false;

    void Start()
    {
        SpawnPlayers();
        StartCoroutine(SpawnFallingObjects());
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 0, 0);
            GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            string playerID = "Player_" + i;
            player.GetComponent<PlayerController>().Initialize(playerID);
            players.Add(player);
        }
    }

    IEnumerator SpawnFallingObjects()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(1f); // Spawn every 1s
            Vector3 spawnPos = new Vector3(Random.Range(-6f, 6f), 6, 0);
            Instantiate(fallingSpherePrefab, spawnPos, Quaternion.identity);
        }
    }
}