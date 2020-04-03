using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomBehaviour : MonoBehaviour
{
    private int enemiesAlive = 0;

    [SerializeField] int roomLevel = 1;

    [Space(5)]
    public GameObject RoomStart;
    public GameObject RoomEnd;
    [SerializeField] GameObject roomBlocker;
    [SerializeField] bool disableBlockerFromStart;

    [Space(5)]
    [SerializeField] private GameObject easyEnemyPrefap;
    [SerializeField] private GameObject mediumEnemyPrefap;
    [SerializeField] private GameObject hardEnemyPrefap;

    [Space(5)]
    [SerializeField] private List<GameObject> enemySpawns = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        if (enemySpawns.Count == 0)
        {
            Debug.LogError("There is no Enemy spawns!");
        }

        if (disableBlockerFromStart == true)
        {
            roomBlocker.SetActive(false);
        }  

        StartCoroutine(SpawnEnemies());
        
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive == 0)
        {
            bool disabled = disableBlockerFromStart == true ? true : false;
            roomBlocker.SetActive(disabled);
            FindObjectOfType<LevelManager>().RoomCleared();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int numberOfSpaws = Random.Range(roomLevel, enemySpawns.Count);

        for (int i = 0; i <= numberOfSpaws; i++)
        {
            yield return null;

            int spawnIndex = Random.Range(0, enemySpawns.Count);
            int enemyDifficulty = Random.Range(1, GameManager.Instance.LevelsCompleted + 1);
            enemyDifficulty = Mathf.Clamp(enemyDifficulty, 1, 3);
            GameObject spawn = enemySpawns[spawnIndex];
            Vector3 spawnPosition = spawn.transform.position;

            GameObject enemyPrefab;

            switch (enemyDifficulty)
            {
                case 1:
                    enemyPrefab = easyEnemyPrefap;
                    break;
                case 2:
                    enemyPrefab = mediumEnemyPrefap;
                    break;
                case 3:
                    enemyPrefab = hardEnemyPrefap;
                    break;
                default:
                    enemyPrefab = easyEnemyPrefap;
                    break;
            }
            
            // TODO: Fix level prefaps root scale. Enemies inherent their scale, and I fucked up and scaled that at some point.
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, this.transform);

            enemiesAlive++;
            
            enemySpawns.Remove(spawn);
        }
    }
}
