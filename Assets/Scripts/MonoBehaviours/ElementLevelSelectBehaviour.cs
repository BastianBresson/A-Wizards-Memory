using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move this to correct location
public enum UpgradeType { Multiplier, Projectiles };

public class ElementLevelSelectBehaviour : MonoBehaviour
{
    [SerializeField] private Element element;
    [SerializeField] private uint id;
    
    public UpgradeType upgradeType = UpgradeType.Multiplier;

    private float intensityChange = 15f;

    Light pointLight;

    GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        pointLight = this.gameObject.GetComponentInChildren<Light>();

        gameManager = GameManager.Instance;

        if (gameManager.SelectorCompleted(this.id) == true) { this.gameObject.SetActive(false); }
    }


    public void Selected()
    {
        // Tell game manager about selection
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

            // Tell player they can select
            other.GetComponent<PlayerController>().LevelSelectorCollision(true, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pointLight.intensity -= intensityChange;

            // Tell player they can no longer select
            other.GetComponent<PlayerController>().LevelSelectorCollision(false, this);
        }
    }
}
