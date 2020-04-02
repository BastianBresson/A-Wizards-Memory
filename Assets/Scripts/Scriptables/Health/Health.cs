using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : ScriptableObject
{
    public int MaxHealth;

    public abstract void TakeDamage(GameObject obj, int currentHealth);
}
