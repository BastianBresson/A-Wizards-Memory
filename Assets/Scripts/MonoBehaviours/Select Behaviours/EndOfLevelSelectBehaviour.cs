using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelSelectBehaviour : MonoBehaviour
{
    private SelectBehaviour selector;


    private void Start()
    {
        SetupSelection();
    }


    private void SetupSelection()
    {
        selector = this.gameObject.GetComponent<SelectBehaviour>();
        selector.OnSelect = OnSelected;
    }


    private void OnSelected()
    {
        GameManager.Instance.LevelComplete();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            selector.NotifyPlayerController(true, other.GetComponent<PlayerController>());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            selector.NotifyPlayerController(false, other.GetComponent<PlayerController>());
        }
    }
}
