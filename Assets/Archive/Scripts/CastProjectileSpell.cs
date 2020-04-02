using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CastProjectileSpell", menuName = "Scriptables/Cast/CastProjectileSpell")]
public class CastProjectileSpell : CastSpell
{
    public override void Cast(GameObject caster, GameObject spell, Vector3 rayHitLocation)
    {
        // Calculates the direction from the caster to 
        Vector3 heading = rayHitLocation - caster.transform.position;
        heading.y = 0; // only interested in x-z direction 
        Vector3 spawnPosition = caster.transform.position + (heading / heading.magnitude) * 1.2f;
        spawnPosition.y = 1; // ensures spawing above ground

        ProjectileSpellBehaviour spellClone = Instantiate(spell, spawnPosition, Quaternion.LookRotation(heading)).GetComponent<ProjectileSpellBehaviour>();
        spellClone.direction = heading / heading.magnitude;
        spellClone.EndPosition = rayHitLocation;

    }
}
