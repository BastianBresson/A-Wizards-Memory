using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
            }
            return instance;
        }
    }
    #endregion

    
    private uint? selectedLevel = null;
    private uint completedLevel;

    private Element levelElement;
    private UpgradeType levelUpgradeType;

    public Vector3 PlayerPosition;
    public Vector3 FallRespawnPosition;


    public int LevelsCompleted { get; private set; } = 0;


    public uint GetCompletedLevel()
    {
        return completedLevel;
    }


    public void LevelComplete()
    {
        LevelsCompleted++;

        completedLevel = (uint)selectedLevel;

        GameObject player = FindPlayer();
        UpgradePlayer(player);

        ResetLevelVariables();

        LoadMemoryLevelScene();
    }


    public void PlayerDied()
    {
        ResetLevelVariables();

        PlayerPosition = Vector3.zero + Vector3.up * 6;
        GameObject player = FindPlayer();
        ResetPlayer(player);

        StartCoroutine(PlayerDiedCoroutine());
    }


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


    private IEnumerator PlayerDiedCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        LoadMemoryLevelScene();
    }


    public void PlayerOrEnemyHasFallenOfLevel(GameObject fallingGameObject)
    {
        StartCoroutine(FallingRespawnCoroutine(fallingGameObject));
    }


    IEnumerator FallingRespawnCoroutine(GameObject gameObjecToRespawn)
    {
        yield return new WaitForSeconds(1f);

        SetVelocityToZero(gameObjecToRespawn);

        ResetPostionAtFallRespawn(gameObjecToRespawn);
    }


    private void SetVelocityToZero(GameObject fallingGameObject)
    {
        fallingGameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    private void ResetPostionAtFallRespawn(GameObject fallingGameObject)
    {
        fallingGameObject.transform.position = FallRespawnPosition;
    }


    public void LoadLevelScene(uint id, Element element, UpgradeType upgradeType)
    {
        StoreSelectedLevelVariables(id, element, upgradeType);

        GameObject player = FindPlayer();
        PlayerPosition = GetPlayerPosition(player);

        LoadElementLevelScene(element);
    }


    private void LoadElementLevelScene(Element element)
    {
        GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>().OnLoadScene(element);
    }


    private void LoadMemoryLevelScene()
    {
        GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>().OnLoadScene();
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(FallRespawnPosition, Vector3.one * 3);
    }
}
