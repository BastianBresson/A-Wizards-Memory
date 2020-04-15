using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public GameObject Caster;
    public ElementalShield Shield;

    float tickCost;

    // Start is called before the first frame update
    void Start()
    {
        if (Shield == null)
        {
            Debug.LogError("SHIELD IS NULL");
        }

        tickCost = Shield.ShieldTickCost;
    }


    public void StopCasting()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Shield.Collide(other);
    }
}
