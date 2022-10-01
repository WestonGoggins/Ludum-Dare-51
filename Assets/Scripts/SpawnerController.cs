using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Transform spawner1;
    public Transform spawner2;
    public Transform spawner3;
    public Round[] rounds;
    public int currentRound = 0;

    public GameController gameController;

    private int roundIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
    }

    void Update()
    {
        if (gameController.roundTimer >= roundIndex)
        {
            SpawnEnemy(rounds[currentRound].spawnAtIndex[roundIndex]);
            roundIndex++;
        }
    }

    public void IncrementRound()
    {
        currentRound += 1;
        roundIndex = 0;
    }

    private void SpawnEnemy(EnemyController enemy)
    {
        int rand = Random.Range(1, 4);
        Transform spawner;
        if (rand == 1) spawner = spawner1;
        else if (rand == 2) spawner = spawner2;
        else if (rand == 3) spawner = spawner3;
        else
        {
            spawner = spawner1;
            Debug.LogWarning("Invalid Spawner Number!");
        }
        Instantiate(enemy.gameObject, spawner);
    }
}
