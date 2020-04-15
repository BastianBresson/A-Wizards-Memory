using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastBehaviour : MonoBehaviour
{
    private GameObject castedShield;
    private GameObject castingProjectile;

    [SerializeField] SkillTree skillTree;

    [SerializeField] private GameObject SpellCastPoint;
    private bool isChargingProjectile;

    private float angle = 45f; 
    [SerializeField] private float spellSpawnOffset = 1.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartProjectileCast(GameObject caster, GameObject projectile, Element element, Vector3 rayHitLocation)
    {
        int projectilesLvl = 0;
        int projectileMultiplierLvl = 0;

        //Vector3 direction = Direction(rayHitLocation, caster.transform.position);

        switch (element.ElementType)
        {
            case Element.ElementEnum.Normal:
                break;
            case Element.ElementEnum.Fire:
                projectilesLvl = skillTree.FireMultiplerLvl;
                projectileMultiplierLvl = skillTree.FireMultiplerLvl;
                break;
            case Element.ElementEnum.Water:
                projectilesLvl = skillTree.WaterProjectilesLvl;
                projectileMultiplierLvl = skillTree.WaterMultiplierLvl;
                break;
            case Element.ElementEnum.Earth:
                projectilesLvl = skillTree.EarthProjectilesLvl;
                projectileMultiplierLvl = skillTree.EarthMultiplierLvl;
                break;
            default:
                projectilesLvl = 0;
                projectileMultiplierLvl = 0;
                break;
        }

        float multiplier = 1f + (projectileMultiplierLvl * skillTree.MultiplierValue);

        StartCoroutine(ChargingProjectileSpellRoutine(projectile, multiplier));

        //pawnProjectile(caster, projectile, direction, projectileMultiplierLvl);

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


    private void SpawnProjectile(GameObject caster, GameObject projectile, Vector3 direction, int multiplierLvl)
    {
        Vector3 spawnOffset = direction * spellSpawnOffset;
        Vector3 spawnPosition = caster.transform.position + spawnOffset;
        spawnPosition.y -= 0.5f;
        ProjectileSpellBehaviour spell = Instantiate(projectile, spawnPosition, Quaternion.LookRotation(direction)).GetComponent<ProjectileSpellBehaviour>();
        spell.direction = direction;

        if (multiplierLvl > 0)
        {
            float multiplier = 1 + multiplierLvl * skillTree.MultiplierValue;
            spell.GetComponent<ProjectileSpellBehaviour>().ScaleProjectile(multiplier);
        }
    }

    public void CastProjectileSpell(Vector3 rayHitLocation)
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


    public void CastElementShield(GameObject caster, GameObject shield, Vector3 rayHitLocation)
    {

        Vector3 direction = SpellCastPoint.transform.forward;
        direction.y = 0;

        castedShield = Instantiate(shield, SpellCastPoint.transform.position, Quaternion.LookRotation(direction), SpellCastPoint.transform);
    }


    public void StopCastingShield()
    {
        castedShield.GetComponent<ShieldBehaviour>().StopCasting();
    }


    // Upgrade the Player's skilltree. The skilltree is a Scriptable Object instance and persists through scenes
    public void UpgradeSkillTree(Element element, UpgradeType upgradeType)
    {
        if (this.gameObject.tag == "Player")
        {
            // TODO: CONSIDER Update the actual skilltree on the player controller
            switch (element.ElementType)
            {
                case Element.ElementEnum.Normal:
                    break;
                case Element.ElementEnum.Fire:
                    if (upgradeType == UpgradeType.Projectiles) { skillTree.FireProjectilesLvl++; }
                    else { skillTree.FireMultiplerLvl++; }
                    break;
                case Element.ElementEnum.Water:
                    if (upgradeType == UpgradeType.Projectiles) { skillTree.WaterProjectilesLvl++; }
                    else { skillTree.WaterMultiplierLvl++; }
                    break;
                case Element.ElementEnum.Earth:
                    if (upgradeType == UpgradeType.Projectiles) { skillTree.EarthProjectilesLvl++; }
                    else { skillTree.EarthMultiplierLvl++; }
                    break;
                default:
                    break;
            }
        }
    }

    public void SetEnemySkillTree(SkillTree st)
    {
        if (this.tag == "Enemy") { skillTree = st; }
        else { Debug.LogError("This was called from a on non enemy-tagged object"); }
    }

    public void ClearSkillTree()
    {
        skillTree.EarthProjectilesLvl = 0;
        skillTree.EarthMultiplierLvl = 0;

        skillTree.FireProjectilesLvl = 0;
        skillTree.FireMultiplerLvl = 0;

        skillTree.WaterProjectilesLvl = 0;
        skillTree.WaterMultiplierLvl = 0;
    }

    private Vector3 Direction(Vector3 targetPosition, Vector3 casterPosition)
    {
        Vector3 heading = targetPosition - casterPosition;
        heading.y = 0; // only interested in x-z direction (horizontal plane)
        return heading / heading.magnitude;
    }

    private Vector3 SpawnLocation(Vector3 direction, Vector3 casterPosition)
    {
        return Vector3.zero;
    }
}
