using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyHealth", menuName = "Scriptables/Health/EnemyHealth")]
public class EnemyHealth : Health
{
    public override void TakeDamage(GameObject obj, int currentHealth)
    {
        if (currentHealth <= 0)
        {
            GameManager.Instance.EnemyDied();
            Destroy(obj.gameObject);
        }
    }
}
