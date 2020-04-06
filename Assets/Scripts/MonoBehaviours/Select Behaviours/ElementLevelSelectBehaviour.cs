using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move this to correct location
public enum UpgradeType { Multiplier, Projectiles };

public class ElementLevelSelectBehaviour : MonoBehaviour
{
    private uint id;
    private float intensityChange = 15f;

    private Light pointLight;
    private SelectBehaviour selector;
    private GameManager gameManager;

    [SerializeField] private Element element;
    
    public UpgradeType upgradeType = UpgradeType.Multiplier;


    // Start is called before the first frame update
    private void Start()
    {
        // Inheret ID
        id = GetComponentInParent<MemoryLevelBehaviour>().ID;

        pointLight = this.gameObject.GetComponentInChildren<Light>();

        gameManager = GameManager.Instance;
        selector = this.gameObject.AddComponent<SelectBehaviour>();
        selector.OnSelect = this.Selected;

        if (gameManager.IsMemoryLevelCompleted(this.id) == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Tell game manager about selection
    public void Selected()
    {
        if (gameManager != null)
        {
            gameManager.LoadLevelScene(this.id, element, upgradeType);
        }
        else
        {
            Debug.LogError("GameManager is NULL");
        }

        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pointLight.intensity += intensityChange;

            selector.NotifyPlayerController(true, other.GetComponent<PlayerController>());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pointLight.intensity -= intensityChange;

            selector.NotifyPlayerController(false, other.GetComponent<PlayerController>());
        }
    }
}
