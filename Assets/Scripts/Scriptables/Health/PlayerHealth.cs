using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "Scriptables/Health/PlayerHealth")]
public class PlayerHealth : Health
{
    public override void TakeDamage(GameObject obj, int currentHealth)
    {
        if (currentHealth <= 0)
        {
            // TODO: alert game managet of death.
            Destroy(obj.gameObject);
        }
    }
}
