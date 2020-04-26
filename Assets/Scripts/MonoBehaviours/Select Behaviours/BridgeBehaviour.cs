using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBehaviour : MonoBehaviour
{
    [SerializeField] private Material selectedBridgeMat = default;
    [SerializeField] private Material nonSelectedBridgeMat = default;

    private SelectBehaviour selector;

    [SerializeField] private GameObject bridge = default;
    [SerializeField] private GameObject selectCollider = default;
    [SerializeField] private GameObject nextMemoryLevel = default;

    [SerializeField] private uint id;
    public uint ID { get { return id; } private set { id = value; } }


    private void Start()
    {
        DisableBridgeCollider();

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
        bridge.GetComponent<MeshCollider>().enabled = false;
    }


    private void EnableBridgeCollider()
    {
        bridge.GetComponent<MeshCollider>().enabled = true;
    }

    private SelectBehaviour AddSelector()
    {
        return this.gameObject.AddComponent<SelectBehaviour>();
    }

    public void PreviouslySelected()
    {
        bridge.GetComponent<MeshRenderer>().material = selectedBridgeMat;
    }


    private void Selected()
    {
        EnableBridgeCollider();

        bridge.GetComponent<MeshRenderer>().material = selectedBridgeMat; 

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