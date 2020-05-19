using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSelectBehaviour : MonoBehaviour
{
    private SelectBehaviour selector;

    [SerializeField] LevelSelect levelSelect = default;
    private Element element;
    private UpgradeType upgradeType;
    private Material material;

    private void Start()
    {
        GetLevelSelectVariables();
        SetMaterial(material);

        SetupSelection();
    }


    public void Selected()
    {
        uint id = GetComponentInParent<MemoryLevelBehaviour>().ID;
        GameManager.Instance.LoadLevelScene(id, element, upgradeType);
    }


    private void GetLevelSelectVariables()
    {
        element = levelSelect.element;
        upgradeType = levelSelect.upgradeType;
        material = levelSelect.material;
    }


    private void SetupSelection()
    {
        selector = this.gameObject.AddComponent<SelectBehaviour>();
        selector.OnSelect = this.Selected;
    }


    private void SetMaterial(Material material)
    {
        this.gameObject.GetComponent<Renderer>().material = this.material;
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
