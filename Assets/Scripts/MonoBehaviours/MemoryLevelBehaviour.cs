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

    [SerializeField] private GameObject[] bridges;
    [SerializeField] bool isStartLevel;
    [SerializeField] bool isKnownFromStart;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        if (isKnownFromStart == true)
        {
            gameManager.AddStartKnownMemoryLevel(this.id);
        }

        isKnown = gameManager.IsMemoryLevelKnown(ID);
        isCompleted = gameManager.IsMemoryLevelCompleted(ID);

        bool enableMemoryLevel = isStartLevel == true || isKnown == true || isCompleted == true;
        bool disableBridges = isStartLevel == false && isCompleted == false && isKnown == true;

        if (enableMemoryLevel == false)
        {
            DisableAllBridges();
            this.gameObject.SetActive(false);
        }

        if (disableBridges == true)
        {
            DisableAllBridges();
        }

        if (isCompleted == true)
        {
            enableBridgeSelection();
        }
    }

    public void onBridgeSelected(uint bridgeID)
    {
        GameManager.Instance.SelectedBridge(this.id, bridgeID);

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
        if (bridges.Length == 0)
        {
            return;
        }
        foreach (GameObject bridge in bridges)
        {
            bridge.SetActive(false);
        }
    }
}
