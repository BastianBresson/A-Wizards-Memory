using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectHealth", menuName = "Scriptables/Health/ObjectHealth")]
public class ObjectHealth : Health
{
    public override void TakeDamage(GameObject obj, float currentHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(obj.gameObject);
        }
    }
}
