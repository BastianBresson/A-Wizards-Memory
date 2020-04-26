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

    private uint? selectedLevel = null;
    private uint completedLevel;

    // Stores completed levels, and their chosen bridge/path
    private Dictionary<uint, uint?> memoryLevelsCompleted = new Dictionary<uint, uint?>();
    private List<uint> memoryLevelsKnown = new List<uint>(); // Save/load dependant

    private Element levelElement;
    private UpgradeType levelUpgradeType;

    public Vector3 PlayerPosition;

    public int LevelsCompleted { get; private set; } = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Only runs ones (on game startup) due to singleton pattern and not being destroyed on scene load, see Awake()
    private void Start()
    {
        GameObject player = FindPlayer();
        ResetPlayer(player);
    }


    public void LoadLevelScene(uint id, Element element, UpgradeType upgradeType)
    {
        StoreSelectedLevelVariables(id, element, upgradeType);

        GameObject player = FindPlayer();
        PlayerPosition = GetPlayerPosition(player);

        LoadElementLevelScene(element);
    }


    public void LevelComplete()
    {
        LevelsCompleted++;

        completedLevel = (uint)selectedLevel;
        memoryLevelsCompleted.Add(completedLevel, null);

        GameObject player = FindPlayer();
        UpgradePlayer(player);

        ResetLevelVariables();

        SceneManager.LoadScene("MemoryLevel");
    }


    public uint GetCompletedLevel()
    {
        return completedLevel;
    }


    private void StoreSelectedLevelVariables(uint levelID, Element element, UpgradeType upgradeType)
    {
        selectedLevel = levelID;
        this.levelElement = element;
        this.levelUpgradeType = upgradeType;
    }


    private void ResetLevelVariables()
    {
        selectedLevel = null;
        levelElement = null;
    }


    private GameObject FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        return player;
    }


    private Vector3 GetPlayerPosition(GameObject player)
    {
        Vector3 position = player.transform.position;
        return position;
    }


    private SpellCastBehaviour GetPlayerSpellSystem(GameObject player)
    {
        SpellCastBehaviour spellSystem = player.GetComponent<SpellCastBehaviour>();
        return spellSystem;
    }


    private void ResetPlayer(GameObject player)
    {
        SpellCastBehaviour playerSpellSystem = GetPlayerSpellSystem(player);
        playerSpellSystem.ClearSkillTree();
    }


    private void UpgradePlayer(GameObject player)
    {
        if (levelUpgradeType != UpgradeType.None)
        {
            SpellCastBehaviour spellSystem = GetPlayerSpellSystem(player);
            spellSystem.UpgradeSkillTree(levelElement, levelUpgradeType);
        }
    }


    private void LoadElementLevelScene(Element element)
    {
        switch (element.ElementType)
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
}
