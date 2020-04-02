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

    private void FixedUpdate()
    {
        if (Caster != null && Caster.gameObject.tag == "Player")
        {
            ShieldMovement();
        }
    }

    private void ShieldMovement()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 heading = hit.point - Caster.transform.position;
            heading.y = 0;
            Vector3 newPosition = Caster.transform.position + (heading / heading.magnitude) * 1.2f;
            newPosition.y = transform.position.y;

            // TODO: make shield move slowly in a circular motion
            transform.position = Vector3.Lerp(transform.position, newPosition, 1);
            transform.rotation = Quaternion.LookRotation(heading);
        }
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
