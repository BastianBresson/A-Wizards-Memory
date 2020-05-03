using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    public Health Health;

    float maxHealth;
    [SerializeField] float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Increase health based on lvl
        maxHealth = Health.MaxHealth;

        currentHealth = maxHealth;

    }

    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;

        Health.TakeDamage(this.gameObject, currentHealth);
    }

}
