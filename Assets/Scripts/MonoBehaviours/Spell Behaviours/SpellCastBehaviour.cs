using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastBehaviour : MonoBehaviour
{
    [SerializeField] SkillTree skillTree;

    [SerializeField] private GameObject SpellCastPoint = default;

    private GameObject castingShield;

    private List<GameObject> castingProjectiles = new List<GameObject>();
    private List<Vector3> projectileOriginalScales = new List<Vector3>();
    private List<Vector3> projectileTargetScales = new List<Vector3>();

    private bool isChargingProjectile;

    private float angle = 15f; 


    public void StartProjectileCast(GameObject projectile, Element element)
    {
        Tuple<int, int> elementLevels = skillTree.ElementLevels(element);

        int projectilesLvl = elementLevels.Item1;
        int projectileMultiplierLvl = elementLevels.Item2;

        
        float multiplier = 1f + (projectileMultiplierLvl * skillTree.MultiplierValue);

        StartCoroutine(ChargingProjectileSpellRoutine(projectile, projectilesLvl, multiplier));
    }


    IEnumerator ChargingProjectileSpellRoutine(GameObject projetileToCast, int projectilesLvl, float multiplier)
    {
        isChargingProjectile = true;

        Vector3 direction = SpellCastPoint.transform.forward;
        direction.y = 0;

        castingProjectiles.Add(Instantiate(projetileToCast, SpellCastPoint.transform.position, Quaternion.LookRotation(direction), SpellCastPoint.transform));

        if (projectilesLvl > 0)
        {
            Vector3 right = SpellCastPoint.transform.right;
            Vector3 forward = SpellCastPoint.transform.forward * 0.5f;
            Vector3 offset1 = (right - forward) * 0.75f;
            Vector3 offset2 = ((right * -1) - forward) * 0.75f;

            for (int i = 1; i <= projectilesLvl; i++)
            {
                Vector3 direction1 = Quaternion.Euler(0, angle * i, 0) * direction;
                Vector3 direction2 = Quaternion.Euler(0, -1 * (angle * i), 0) * direction;
                castingProjectiles.Add(Instantiate(projetileToCast, SpellCastPoint.transform.position + (i * offset1), Quaternion.LookRotation(direction1), SpellCastPoint.transform));
                castingProjectiles.Add(Instantiate(projetileToCast, SpellCastPoint.transform.position + (i * offset2), Quaternion.LookRotation(direction2), SpellCastPoint.transform));
            }
        }


        Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);

        foreach (GameObject projectile in castingProjectiles)
        {
            Vector3 currentScale = projectile.transform.localScale;
            projectileTargetScales.Add(currentScale * multiplier);
            projectile.transform.localScale = startScale;
            projectileOriginalScales.Add(projectile.transform.localScale);
            
        }


        float scaleTime = .5f;
        float currentTime = 0.0f;


        while (currentTime <= scaleTime && castingProjectiles != null)
        {

            for (int i = 0; i < castingProjectiles.Count; i++)
            {
                if (castingProjectiles[i] != null)
                {
                    castingProjectiles[i].transform.localScale = Vector3.Lerp(projectileOriginalScales[i], projectileTargetScales[i], currentTime / scaleTime);

                }
            }

            currentTime += Time.deltaTime;

            yield return null;
        }
    }


    public void CastProjectileSpell()
    {
        if (castingProjectiles != null || castingProjectiles.Count != 0)
        {
            isChargingProjectile = false;


            Vector3 direction = SpellCastPoint.transform.forward;
            direction.y = 0;

            if (castingProjectiles[0] != null)
            {
                castingProjectiles[0].GetComponent<ProjectileSpellBehaviour>().CastProjectile(direction, projectileOriginalScales[0]);
                castingProjectiles[0].transform.parent = null;
            }


            if (castingProjectiles.Count > 1)
            {
                int angleIncrease = 1;
                for (int i = 1; i < castingProjectiles.Count; i+=2)
                {
                    Vector3 direction1 = Quaternion.Euler(0, angle * angleIncrease, 0) * direction;
                    Vector3 direction2 = Quaternion.Euler(0, -1 * (angle * angleIncrease), 0) * direction;

                    castingProjectiles[i].GetComponent<ProjectileSpellBehaviour>().CastProjectile(direction1, projectileOriginalScales[0]);
                    castingProjectiles[i].transform.parent = null;

                    castingProjectiles[i+1].GetComponent<ProjectileSpellBehaviour>().CastProjectile(direction2, projectileOriginalScales[0]);
                    castingProjectiles[i+1].transform.parent = null;

                    angleIncrease++;
                }
            }

            
        }
        else
        {
            return;
        }

        castingProjectiles.Clear();
        projectileOriginalScales.Clear();
        projectileTargetScales.Clear();
    }


    public void CastElementShield(GameObject shield)
    {
        Vector3 direction = SpellCastPoint.transform.forward;
        direction.y = 0;

        castingShield = Instantiate(shield, SpellCastPoint.transform.position, Quaternion.LookRotation(direction), SpellCastPoint.transform);
    }


    public void StopCastingShield()
    {
        if (castingShield != null)
        {
            castingShield.GetComponent<ShieldBehaviour>().StopCasting();

            castingShield = null;
        }
        else
        {
            return;
        }
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
        //skillTree.ClearTree();
    }

}
