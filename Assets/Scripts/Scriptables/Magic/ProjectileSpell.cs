using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Scriptables/Elemental Projectile")]
public class ProjectileSpell : Spell
{
    public Element Element;
    public float Speed;

    public void Collide(GameObject collidedObject, GameObject projectileObject)
    {
        if (collidedObject.tag == "Enemy" || collidedObject.tag == "Player" || collidedObject.tag == "DestructableObject")
        {
            collidedObject.GetComponent<HealthBehaviour>().ReceiveDamage(this.Damage);
        }

        Destroy(projectileObject);  
    }
}
