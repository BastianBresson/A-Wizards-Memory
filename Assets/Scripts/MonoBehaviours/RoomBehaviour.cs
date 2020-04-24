using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomBehaviour : MonoBehaviour
{
    private int enemiesAlive = 0;

    private int levelsCompleted;
    [SerializeField] int roomLevel = 1;

    [Space(5)]
    public GameObject RoomStart;
    public GameObject RoomEnd;
    [SerializeField] GameObject roomBlocker = default;
    [SerializeField] bool disableBlockerFromStart  = default;

    [Space(5)]
    [SerializeField] private GameObject easyEnemyPrefap = default;
    [SerializeField] private GameObject mediumEnemyPrefap = default;
    [SerializeField] private GameObject hardEnemyPrefap = default;

    [Space(5)]
    [SerializeField] private List<GameObject> enemySpawns = new List<GameObject>();


    private void Start()
    {
        levelsCompleted = GameManager.Instance.LevelsCompleted;

        BlockExitFromStart();

        StartCoroutine(SpawnEnemies());
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (isRoomCleared())
        {
            StopBlockingExit();
            NotifyLevelManager();
        }
    }


    // Exists either block my being enabled (walls) or being disabled (bridges)
    private void BlockExitFromStart()
    {
        if (disableBlockerFromStart == true)
        {
            roomBlocker.SetActive(false);
        }
    }


    private bool isRoomCleared()
    {
        return enemiesAlive == 0;
    }


    private void StopBlockingExit()
    {
        bool disabled = disableBlockerFromStart == true ? true : false;
        roomBlocker.SetActive(disabled);
    }


    private void NotifyLevelManager()
    {
        FindObjectOfType<LevelManager>().RoomCleared();
    }

    private IEnumerator SpawnEnemies()
    {
        int numberOfEnemySpawns = NumberOfEnemiesToSpawn();

        for (int i = 0; i <= numberOfEnemySpawns; i++)
        {
            yield return null;

            Vector3 spawnPosition = NextEnemySpawnPosition();
            int enemyDifficultyLevel = NextEnemyDifficultyLevel();       
            GameObject enemyToSpawn = EnemyToSpawn(enemyDifficultyLevel);

            SpawnEnemy(enemyToSpawn, spawnPosition);
        }
    }


    private int NumberOfEnemiesToSpawn()
    {
        int lowerbound = 1 + (levelsCompleted / 5);
        int upperbound = 3 + (levelsCompleted / 5);

        lowerbound = Mathf.Clamp(lowerbound, 0, enemySpawns.Count());
        upperbound = Mathf.Clamp(upperbound, lowerbound, enemySpawns.Count());

        int enemiesToSpawns = Random.Range(lowerbound, upperbound);

        return enemiesToSpawns;
    }


    private Vector3 NextEnemySpawnPosition()
    {
        GameObject spawn = NextEnemySpawn();
        Vector3 spawnPosition = spawn.transform.position;

        enemySpawns.Remove(spawn);

        return spawnPosition;
    }


    private GameObject NextEnemySpawn()
    {
        int spawnIndex = Random.Range(0, enemySpawns.Count);
        GameObject spawn = enemySpawns[spawnIndex];
        return spawn;
    }


    private int NextEnemyDifficultyLevel()
    {
        int enemyDifficulty = Random.Range(1, GameManager.Instance.LevelsCompleted + 1);
        enemyDifficulty = Mathf.Clamp(enemyDifficulty, 1, 3);

        return enemyDifficulty;
    }


    private GameObject EnemyToSpawn(int enemyLevel)
    {
        GameObject enemyToSpawn;

        switch (enemyLevel)
        {
            case 1:
                enemyToSpawn = easyEnemyPrefap;
                break;
            case 2:
                enemyToSpawn = mediumEnemyPrefap;
                break;
            case 3:
                enemyToSpawn = hardEnemyPrefap;
                break;
            default:
                enemyToSpawn = easyEnemyPrefap;
                break;
        }

        return enemyToSpawn;
    }


    private void SpawnEnemy(GameObject enemy, Vector3 spawnPosition)
    {
        Instantiate(enemy, spawnPosition, Quaternion.identity, this.transform);

        enemiesAlive++;
    }
}
