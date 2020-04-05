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

    private uint? selectedLevel = null;
    private uint completedLevel;

    // stores completed levels, and their chosen bridge/path
    private Dictionary<uint, uint?> memoryLevelsCompleted = new Dictionary<uint, uint?>();
    private List<uint> memoryLevelsKnown = new List<uint>(); // Save/load dependant

    private Element levelElement;
    private UpgradeType upgradeType;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<SpellCastBehaviour>().ClearSkillTree();
    } 

    public bool IsMemoryLevelKnown(uint id)
    {
        return memoryLevelsKnown.Contains(id);
    }

    public bool IsMemoryLevelCompleted(uint id)
    {
        return memoryLevelsCompleted.ContainsKey(id);
    }

    public uint? MemoryLevelActiveBridge(uint id)
    {
        if (memoryLevelsCompleted.ContainsKey(id))
        {
            return memoryLevelsCompleted[id];
        }
        else return null;
    }

    public void SelectedBridge(uint id, uint? bridge)
    {
        memoryLevelsCompleted[id] = bridge;
    }

    public void AddStartKnownMemoryLevel(uint id)
    {
        if (memoryLevelsKnown.Contains(id) == false)
        {
            memoryLevelsKnown.Add(id);
        }
    }

    // Store the chosen level's element and upgrade type
    // Store player's position before loading new scene
    public void LoadLevelScene(uint id, Element element, UpgradeType upgradeType)
    {
        selectedLevel = id;
        levelElement = element;
        this.upgradeType = upgradeType;

        PlayerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>().position;

        switch (levelElement.ElementType)
        {
            case Element.ElementEnum.Fire:
                SceneManager.LoadScene("FireLevelScene");
                break;
            case Element.ElementEnum.Water:
                SceneManager.LoadScene("WaterLevelScene");
                break;
            case Element.ElementEnum.Earth:
                SceneManager.LoadScene("EarthLevelScene");
                break;
            default:
                break;
        }
    }

    public void LevelComplete()
    {
        LevelsCompleted++;

        completedLevel = (uint)selectedLevel;
        selectedLevel = null;

        memoryLevelsCompleted.Add(completedLevel, null);

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

        SceneManager.LoadScene("MemoryLevel");
    }
}
