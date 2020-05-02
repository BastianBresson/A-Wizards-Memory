using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            GameManager.Instance.PlayerOrEnemyHasFallenOfLevel(other.gameObject);
        }
    }
}
