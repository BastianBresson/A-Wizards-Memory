using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastBehaviour : MonoBehaviour
{
    private GameObject castedShield;
    private GameObject castingProjectile;

    [SerializeField] SkillTree skillTree;

    [SerializeField] private GameObject SpellCastPoint = default;
    private bool isChargingProjectile;

    private float angle = 45f; 
    [SerializeField] private float spellSpawnOffset = 1.2f;



    public void StartProjectileCast(GameObject projectile, Element element)
    {
        Tuple<int, int> elementLevels = skillTree.ElementLevels(element);

        int projectilesLvl = elementLevels.Item1;
        int projectileMultiplierLvl = elementLevels.Item2;

        
        float multiplier = 1f + (projectileMultiplierLvl * skillTree.MultiplierValue);

        StartCoroutine(ChargingProjectileSpellRoutine(projectile, multiplier));

        // For each level above 0:
        // spawn two projectiles with direction turned to the left and right.
        // Creates a bigger and bigger cone for each level of spawned projectiles
        // TODO: angle should be smaller. Requires spells not to collide with each other
        /*if (projectilesLvl > 0)
        {
            for (int i = 1; i <= projectilesLvl; i++)
            {
                Vector3 direction1 = Quaternion.Euler(0, angle * i, 0) * direction;
                Vector3 direction2 = Quaternion.Euler(0, -1 * (angle * i), 0) * direction;
                SpawnProjectile(caster, projectile, direction1, projectileMultiplierLvl);
                SpawnProjectile(caster, projectile, direction2, projectileMultiplierLvl);
            }
        }*/
    }


    IEnumerator ChargingProjectileSpellRoutine(GameObject projetileToCast, float multiplier)
    {
        isChargingProjectile = true;

        Vector3 direction = SpellCastPoint.transform.forward;
        direction.y = 0;

        castingProjectile = Instantiate(projetileToCast, SpellCastPoint.transform.position, Quaternion.LookRotation(direction), SpellCastPoint.transform);

        Vector3 originalScale = castingProjectile.transform.localScale;
        Vector3 targetScale = originalScale * multiplier;

        float scaleTime = .5f;
        float currentTime = 0.0f;

        Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
        castingProjectile.transform.localScale = startScale;

        while (isChargingProjectile && castingProjectile != null)
        {
            if (currentTime <= scaleTime)
            {
                castingProjectile.transform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / scaleTime);

                currentTime += Time.deltaTime;
            }

            yield return null;
        }
    }


    public void UpdateSpellCastRotation(Vector3 raycastHit)
    {
        Vector3 postionToLookAt = raycastHit;
        postionToLookAt.y = this.transform.position.y;
        SpellCastPoint.transform.LookAt(postionToLookAt);
    }


    public void CastProjectileSpell()
    {
        if (castingProjectile == null)
        {
            return;
        }
        isChargingProjectile = false;

        Vector3 direction = SpellCastPoint.transform.forward;
        direction.y = 0;

        castingProjectile.GetComponent<ProjectileSpellBehaviour>().CastProjectile(direction);

        castingProjectile.transform.parent = null;

    }


    public void CastElementShield(GameObject shield)
    {
        Vector3 direction = SpellCastPoint.transform.forward;
        direction.y = 0;

        castedShield = Instantiate(shield, SpellCastPoint.transform.position, Quaternion.LookRotation(direction), SpellCastPoint.transform);
    }


    public void StopCastingShield()
    {
        castedShield.GetComponent<ShieldBehaviour>().StopCasting();

        castedShield = null;
    }


    // Upgrade the Player's skilltree. The skilltree is a Scriptable Object instance and persists through scenes
    public void UpgradeSkillTree(Element element, UpgradeType upgradeType)
    {
        if (this.gameObject.tag == "Player")
        {
            skillTree.UpgradeSkillTree(element, upgradeType);
        }
    }

    public void SetEnemySkillTree(SkillTree st)
    {
        if (this.tag == "Enemy")
        {
            skillTree = st;
        }
        else
        {
            Debug.LogError("This was called from a on non enemy-tagged object");
        }
    }

    public void ClearSkillTree()
    {
        skillTree.ClearTree();
    }

}
