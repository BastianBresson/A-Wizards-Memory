using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBehaviour : MonoBehaviour
{
    private SelectBehaviour selector;

    [SerializeField] private GameObject selectCollider;

    [SerializeField] private uint id;
    public uint ID { get { return id; } private set { id = value; } }

    // Start is called before the first frame update
    void Start()
    {
        selector = addSelector();
        selector.OnSelect = Selected;

        selectCollider.SetActive(false);
    }

    public void EnableSelection()
    {
        selectCollider.SetActive(true);
    }

    public void onPlayerEnteredTrigger(PlayerController playerController)
    {
        selector.NotifyPlayerController(true, playerController);
    }

    public void onPlayerExitedTrigger(PlayerController playerController)
    {
        selector.NotifyPlayerController(false, playerController);
    }

    private SelectBehaviour addSelector()
    {
        return this.gameObject.AddComponent<SelectBehaviour>();
    }

    private void Selected()
    {
        GetComponentInParent<MemoryLevelBehaviour>().onBridgeSelected(this.id);
        selectCollider.SetActive(false);
    }
}
