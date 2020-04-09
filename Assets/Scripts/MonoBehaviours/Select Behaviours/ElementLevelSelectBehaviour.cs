using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementLevelSelectBehaviour : MonoBehaviour
{
    private uint id;

    private SelectBehaviour selector;
    private GameManager gameManager;

    [SerializeField] LevelSelect levelSelect;
    private Element element;
    private UpgradeType upgradeType;
    private Material material;

    private void Start()
    {
        DisableIfCompleted();

        getLevelSelectVariables();
        setMaterial(material);

        // Selector and MemoryLevel needs to have the same ID
        id = GetComponentInParent<MemoryLevelBehaviour>().ID;

        gameManager = GameManager.Instance;

        selector = this.gameObject.AddComponent<SelectBehaviour>();
        selector.OnSelect = this.Selected;
    }


    public void Selected()
    {
        gameManager.LoadLevelScene(this.id, element, upgradeType);
    }


    private void DisableIfCompleted()
    {
        if (gameManager.isMemoryLevelCompleted(this.id))
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
