using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CastShield", menuName = "Scriptables/Cast/CastShield")]
public class CastShield : CastSpell
{
    public override void Cast(GameObject caster, GameObject spell, Vector3 rayHitLocation)
    {
        // Calculates the direction from the caster to 
        Vector3 heading = rayHitLocation - caster.transform.position;
        heading.y = 0; // only interested in x-z direction 
        Vector3 spawnPosition = caster.transform.position + (heading / heading.magnitude) * 1.2f;

        float spawnHeight = spell.transform.lossyScale.y / 2 + 0.5f; // spawns the shield 0.5 above ground
        spawnPosition.y = spawnHeight;

        GameObject shield = Instantiate(spell, spawnPosition, Quaternion.LookRotation(heading));
        shield.GetComponent<ShieldBehaviour>().Caster = caster;

        if (caster.tag == "Player")
        {
            caster.GetComponent<PlayerController>().activeShield = shield;
        }
    }


}
