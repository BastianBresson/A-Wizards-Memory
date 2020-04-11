using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementLevelSelectBehaviour : MonoBehaviour
{
    public uint id;

    private SelectBehaviour selector;

    [SerializeField] LevelSelect levelSelect;
    private Element element;
    private UpgradeType upgradeType;
    private Material material;

    private void Start()
    {
        // Selector and MemoryLevel needs to have the same ID
        id = GetComponentInParent<MemoryLevelBehaviour>().ID;
        DisableIfCompleted();

        getLevelSelectVariables();
        setMaterial(material);

        selector = this.gameObject.AddComponent<SelectBehaviour>();
        selector.OnSelect = this.Selected;
    }


    public void Selected()
    {
        GameManager.Instance.LoadLevelScene(this.id, element, upgradeType);
    }


    private void DisableIfCompleted()
    {
        bool justCompleted = GameManager.Instance.isJustCompleted(this.id);
        bool previouslyCompleted = GameManager.Instance.isMemoryLevelCompleted(this.id);

        if (justCompleted || previouslyCompleted)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void getLevelSelectVariables()
    {
        element = levelSelect.element;
        upgradeType = levelSelect.upgradeType;
        material = levelSelect.material;
    }


    private void setMaterial(Material material)
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
