using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerController : MonoBehaviour
{
    public Transform spawner1;
    public Transform spawner2;
    public Transform spawner3;
    public Round[] rounds;
    public int currentRound = 0;

    public GameController gameController;
    public List<EnemyController> activeEnemies;

    private int roundIndex = 0;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        if (objs.Length > 0) currentRound = objs[0].GetComponent<DontDestroy>().roundCounter;
        else currentRound = 1;
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
    }

    void Update()
    {
        if (gameController.levelTimeStart - gameController.roundTimer >= roundIndex)
        {
            //if (roundIndex > rounds[currentRound].spawnAtIndex.Length - 1)
            //{
            //    if (currentRound > rounds.Length)
            //    {
            //        SceneManager.LoadScene("Win Scene");
            //    }
            //    {
            //        EndRound();
            //    }
            //}
            if (rounds[currentRound].spawnAtIndex[roundIndex] != null)
            {
                SpawnEnemy(rounds[currentRound].spawnAtIndex[roundIndex]);
            }
            roundIndex++;
        }
    }

    public void EndRound()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        objs[0].GetComponent<DontDestroy>().NextRound();
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
        GameObject obj = Instantiate(enemy.gameObject, spawner);
        obj.GetComponent<EnemyController>().lane = rand;
        activeEnemies.Add(obj.GetComponent<EnemyController>());
    }
}
