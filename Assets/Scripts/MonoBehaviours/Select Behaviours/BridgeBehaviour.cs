using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBehaviour : MonoBehaviour
{
    [SerializeField] private Material selectedBridgeMat = default;

    private SelectBehaviour selector;

    [SerializeField] private GameObject bridge = default;
    [SerializeField] private GameObject selectCollider = default;
    [SerializeField] private GameObject nextMemoryLevel = default;

    private MeshCollider bridgeCollider;

    [SerializeField] private uint id;
    public uint ID { get { return id; } private set { id = value; } }

    private void Awake()
    {
        bridgeCollider = bridge.GetComponent<MeshCollider>();
        DisableBridgeCollider();
    }

    private void Start()
    {
        selector = AddSelector();
        selector.OnSelect = Selected;

        selectCollider.SetActive(false);
    }


    public void EnableSelection()
    {
        StartCoroutine(activateSelector());
    }


    public void NotSelected()
    {
        selectCollider.SetActive(false);
    }


    public void OnPlayerEnteredTrigger(PlayerController playerController)
    {
        selector.NotifyPlayerController(true, playerController);
    }


    public void OnPlayerExitedTrigger(PlayerController playerController)
    {
        selector.NotifyPlayerController(false, playerController);
    }


    private void DisableBridgeCollider()
    {
        bridgeCollider.enabled = false;
    }


    private void EnableBridgeCollider()
    {
        bridgeCollider.enabled = true;
        bridge.GetComponent<MeshRenderer>().material = selectedBridgeMat;
    }

    private SelectBehaviour AddSelector()
    {
        return this.gameObject.AddComponent<SelectBehaviour>();
    }

    public void PreviouslySelected()
    {

        EnableBridgeCollider();
    }


    private void Selected()
    {
        EnableBridgeCollider();

        nextMemoryLevel.SetActive(true);

        GetComponentInParent<MemoryLevelBehaviour>().OnBridgeSelected(this.id);

        selectCollider.SetActive(false);
    }


    private IEnumerator activateSelector()
    {
        yield return new WaitForSeconds(.5f);
        selectCollider.SetActive(true);
    }
}