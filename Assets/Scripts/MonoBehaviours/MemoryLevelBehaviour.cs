using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLevelBehaviour : MonoBehaviour
{
    private static Dictionary<uint, uint> memoryLevelsCompletedThisRun = new Dictionary<uint, uint>();

    private LevelSelectBehaviour levelSelector;

    private bool isCompletedThisRun;
    private bool isJustCompleted;
    private bool isPreviouslyCompleted;

    [SerializeField] private uint id;
    public uint ID { get { return id; } private set { id = value; } }

    [SerializeField] private GameObject[] bridges = default;
    [SerializeField] bool isKnownFromStart = default;


    void Start()
    {
        isJustCompleted = GameManager.Instance.GetCompletedLevel() == id;

        isCompletedThisRun = IsMemoryLevelCompletedThisRun();

        isPreviouslyCompleted = IsCompletedInPreviousRun();

        levelSelector = FindLevelSelector();

        DetermineStartState();
    }


    private void DetermineStartState()
    {
        if (isJustCompleted)
        {
            SetLevelSelectorInactive();
            enableBridgeSelection();
            SaveSystem.CompletedNewLevel(id);
        }
        else if (isCompletedThisRun)
        {
            SetLevelSelectorInactive();
            uint selectedBridge = GetSelectedBridge();
            NotifySelectedBridge(selectedBridge);
        }
        else if (isPreviouslyCompleted) // MemoryLevel and Bridges should stay visible
        {
            return; 
        }
        else if (isKnownFromStart) // special case for memorylevels that should be active from start.
        {
            SetAllBridgesInactive();
        }
        else
        {
            SetAllBridgesInactive(); // bridges should be disabled when this is set as active by a bridgge
            SetMemoryLevelInactive();
        }
    }


    private LevelSelectBehaviour FindLevelSelector()
    {
        LevelSelectBehaviour levelSelector = GetComponentInChildren<LevelSelectBehaviour>();
        return levelSelector;
    }


    private void SetLevelSelectorInactive()
    {
        levelSelector.gameObject.SetActive(false);
    }


    private void SetMemoryLevelInactive()
    {
        this.gameObject.SetActive(false);
    }


    private uint GetSelectedBridge()
    {
        uint selectedBridgeID = memoryLevelsCompletedThisRun[this.id];
        return selectedBridgeID;
    }


    private bool IsMemoryLevelCompletedThisRun()
    {
        return memoryLevelsCompletedThisRun.ContainsKey(id) && !isJustCompleted;
    }


    private bool IsCompletedInPreviousRun()
    {
        return SaveSystem.IsLevelCompleted(id);
    }


    private uint? MemoryLevelChosenBridge()
    {
        if (memoryLevelsCompletedThisRun.ContainsKey(id))
        {
            return memoryLevelsCompletedThisRun[id];
        }
        else return null;
    }


    public void OnBridgeSelected(uint bridgeID)
    {
        memoryLevelsCompletedThisRun.Add(this.id, bridgeID);

        foreach (GameObject bridge in bridges)
        {
            BridgeBehaviour bridgeBehaviour = bridge.GetComponent<BridgeBehaviour>();
            if (bridgeID != bridgeBehaviour.ID)
            {
                bridgeBehaviour.NotSelected();
            }
        }
    }


    private void enableBridgeSelection()
    {
        foreach (GameObject bridge in bridges)
        {
            bridge.GetComponent<BridgeBehaviour>().EnableSelection();
        }
    }


    private void NotifySelectedBridge(uint id)
    {
        foreach (GameObject bridge in bridges)
        {
            BridgeBehaviour bridgeBehaviour = bridge.GetComponent<BridgeBehaviour>();
            uint bridgeID = bridgeBehaviour.ID;

            if (bridgeID == id)
            {
                bridgeBehaviour.PreviouslySelected();
            }
        }
    }

    private void SetAllBridgesInactive()
    {
        if (bridges.Length > 0 && bridges != null)
        {
            foreach (GameObject bridge in bridges)
            {
                bridge.SetActive(false);
            }
        }

    }
}
