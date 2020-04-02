using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementalShield", menuName = "Scriptables/Elemental Shield")]
public class ElementalShield : ScriptableObject
{
    public Element Element;
    public float ShieldMovementSpeed;
    public int ShieldTickCost;

    public bool ShieldCounter(Element b)
    {
        return Element.Countered(Element.ElementType, b.ElementType);
    }

    public void Collide(Collider other)
    {
        if (other.tag == "PlayerElementalProjectile" || other.tag == "EnemyElementalProjectile")
        {
            ProjectileSpellBehaviour elementalProjectile = other.GetComponent<ProjectileSpellBehaviour>();
            if (ShieldCounter(elementalProjectile.projectileSpell.Element))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
