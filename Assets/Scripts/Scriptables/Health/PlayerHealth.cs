using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "Scriptables/Health/PlayerHealth")]
public class PlayerHealth : Health
{
    public override void TakeDamage(GameObject obj, float currentHealth)
    {
        if (currentHealth <= 0)
        {
            GameManager.Instance.PlayerDied();
            Destroy(obj.gameObject);
        }
    }
}
