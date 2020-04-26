using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLevelBehaviour : MonoBehaviour
{
    private static Dictionary<uint, uint> memoryLevelsCompleted = new Dictionary<uint, uint>();
    private static List<uint> memoryLevelsKnown = new List<uint>(); // TODO: Save/load dependant

    private LevelSelectBehaviour levelSelector;

    private bool isCompleted;
    private bool isJustCompleted;

    [SerializeField] private uint id;
    public uint ID { get { return id; } private set { id = value; } }

    [SerializeField] private GameObject[] bridges = default;
    [SerializeField] bool isKnownFromStart = default;


    void Start()
    {
        isJustCompleted = GameManager.Instance.GetCompletedLevel() == id;

        isCompleted = isMemoryLevelCompleted();

        levelSelector = FindLevelSelector();

        DetermineStartState();
    }


    private void DetermineStartState()
    {
        if (isJustCompleted)
        {
            SetLevelSelectorInactive();
            enableBridgeSelection();
        }
        else if (isCompleted)
        {
            SetLevelSelectorInactive();
            uint selectedBridge = GetSelectedBridge();
            NotifySelectedBridge(selectedBridge);
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
        uint selectedBridgeID = memoryLevelsCompleted[this.id];
        return selectedBridgeID;
    }


    public bool isMemoryLevelCompleted()
    {
        return memoryLevelsCompleted.ContainsKey(id) && !isJustCompleted;
    }


    public uint? MemoryLevelChosenBridge()
    {
        if (memoryLevelsCompleted.ContainsKey(id))
        {
            return memoryLevelsCompleted[id];
        }
        else return null;
    }


    public void OnBridgeSelected(uint bridgeID)
    {
        memoryLevelsCompleted.Add(this.id, bridgeID);

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
