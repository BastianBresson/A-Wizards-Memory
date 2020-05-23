using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Scriptables/Elemental Projectile")]
public class ProjectileSpell : Spell
{
    public Element Element;
    public float Speed;

    public GameObject ExplosionParticle;
}
