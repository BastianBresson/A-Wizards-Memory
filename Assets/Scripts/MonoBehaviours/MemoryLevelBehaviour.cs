using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLevelBehaviour : MonoBehaviour
{
    private bool isKnown;
    private bool isCompleted;

    private uint? selectedBridgeID;

    [SerializeField] private uint id;
    public uint ID { get { return id; } private set { id = value; } }

    [SerializeField] private GameObject[] bridges = default;
    [SerializeField] bool isStartLevel = default;
    [SerializeField] bool isKnownFromStart = default;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        if (isKnownFromStart == true)
        {
            gameManager.AddStartKnownMemoryLevel(this.id);
        }

        isKnown = gameManager.isMemoryLevelKnown(ID);
        isCompleted = gameManager.isMemoryLevelCompleted(ID);

        bool enableMemoryLevel = isStartLevel == true || isKnown == true || isCompleted == true;
        bool isJustCompleted = gameManager.isJustCompleted(this.id);
        bool disableBridges = isCompleted == false && isJustCompleted == false;

        if (enableMemoryLevel == false)
        {
            DisableAllBridges();
            this.gameObject.SetActive(false);
        }
        else if (disableBridges == true)
        {
            DisableAllBridges();
        }
        else if (isJustCompleted == true)
        {
            enableBridgeSelection();
        }
        else if (isCompleted)
        {
            uint chosenBridge = (uint)gameManager.MemoryLevelChosenBridge(this.id);
            DisableNonSelectedBridges(chosenBridge);
        }
    }

    public void onBridgeSelected(uint bridgeID, uint nextMemoryLevel)
    {
        GameManager.Instance.SelectedBridge(this.id, bridgeID);
        GameManager.Instance.AddKnownMemoryLevel(nextMemoryLevel);
        DisableNonSelectedBridges(bridgeID);
    }

    private void enableBridgeSelection()
    {
        foreach (GameObject bridge in bridges)
        {
            bridge.GetComponent<BridgeBehaviour>().EnableSelection();
        }
    }


    private void DisableNonSelectedBridges(uint id)
    {
        foreach (GameObject bridge in bridges)
        {
            uint bridgeID = bridge.GetComponent<BridgeBehaviour>().ID;

            if (bridgeID != id)
            {
                bridge.SetActive(false);
            }
        }
    }

    private void DisableAllBridges()
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
