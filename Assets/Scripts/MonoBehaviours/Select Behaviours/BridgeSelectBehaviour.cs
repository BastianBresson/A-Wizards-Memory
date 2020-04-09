using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSelectBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<BridgeBehaviour>().onPlayerEnteredTrigger(other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<BridgeBehaviour>().onPlayerExitedTrigger(other.GetComponent<PlayerController>());
        }
    }
}
