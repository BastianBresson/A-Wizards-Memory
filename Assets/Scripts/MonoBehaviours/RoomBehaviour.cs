using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] int roomLevel = 1;

    [Space(5)]
    public GameObject RoomStart;
    public GameObject RoomEnd;

    [Space(5)]
    [SerializeField] private GameObject easyEnemyPrefap;
    [SerializeField] private GameObject mediumEnemyPrefap;
    [SerializeField] private GameObject hardEnemyPrefap;

    [Space(5)]
    [SerializeField] private List<GameObject> enemySpawns = new List<GameObject>();

    
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        if (enemySpawns.Count == 0)
        {
            Debug.LogError("There is no Enemy spawns!");
        }
        else if (gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }

        StartCoroutine(SpawnEnemies());
        
    }

    private IEnumerator SpawnEnemies()
    {
        int numberOfSpaws = Random.Range(roomLevel, enemySpawns.Count);

        for (int i = 0; i <= numberOfSpaws; i++)
        {
            yield return null;

            int spawnIndex = Random.Range(0, enemySpawns.Count);
            int enemyDifficulty = Random.Range(1, gameManager.LevelsCompleted + 1);
            enemyDifficulty = Mathf.Clamp(enemyDifficulty, 1, 3);
            GameObject spawn = enemySpawns[spawnIndex];
            Vector3 spawnPosition = spawn.transform.position;
            //spawnPosition.y = 3;

            if (enemyDifficulty == 1)
            {
                Instantiate(easyEnemyPrefap, spawnPosition, Quaternion.identity);
            }
            else if (enemyDifficulty == 2)
            {
                Instantiate(mediumEnemyPrefap, spawnPosition, Quaternion.identity);
            }
            else if (enemyDifficulty == 3)
            {
                Instantiate(hardEnemyPrefap, spawnPosition, Quaternion.identity);
            }

            gameManager.EnemySpawned();

            enemySpawns.Remove(spawn);
        }
    }

}
