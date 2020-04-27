using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    EnemyBehaviour me;
    void Start()
    {
        GetComponentInParent<EnemyBehaviour>();
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }


    private void OnTriggerExit(Collider other)
    {
        
    }


}
