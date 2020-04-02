using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    #endregion

    public Vector3 PlayerPosition;

    public int LevelsCompleted { get; private set; } = 0;

    private List<uint> selectorsCompleted = new List<uint>();

    private Element levelElement;
    private UpgradeType upgradeType;

    private int totalEnemies = 0;
    private int deadEnemies = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<SpellCastBehaviour>().ClearSkillTree();
    } 

    public bool SelectorCompleted(uint id)
    {
        return selectorsCompleted.Contains(id);
    }

    // Store the chosen level's element and upgrade type..
    // Store player's position before loading new scene
    public void LoadLevelScene(uint id, Element element, UpgradeType upgradeType)
    {
        selectorsCompleted.Add(id);
        levelElement = element;
        this.upgradeType = upgradeType;

        PlayerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>().position;

        SceneManager.LoadScene("LevelScene");
    }

    // TODO: make event based
    public void EnemySpawned() => totalEnemies++;

    // TODO: make event based
    public void EnemyDied()
    {
        deadEnemies++;

        if (deadEnemies == totalEnemies)
        {
            LevelComplete();
        }
    }


    private void LevelComplete()
    {
        LevelsCompleted++;

        // Upgrade the player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<SpellCastBehaviour>().UpgradeSkillTree(levelElement, upgradeType);
        }
        else
        {
            Debug.LogError("Player is NULL");
        }

        // Reset level-related variables
        levelElement = null;
        totalEnemies = 0;
        deadEnemies = 0;

        SceneManager.LoadScene("MemoryLevel");
    }
}
