using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : ScriptableObject
{
    public float MaxHealth;

    public abstract void TakeDamage(GameObject obj, float currentHealth);
}
