using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSelectBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<BridgeBehaviour>().OnPlayerEnteredTrigger(other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<BridgeBehaviour>().OnPlayerExitedTrigger(other.GetComponent<PlayerController>());
        }
    }
}
