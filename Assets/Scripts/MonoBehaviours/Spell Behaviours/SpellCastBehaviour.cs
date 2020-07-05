using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastBehaviour : MonoBehaviour
{
    [SerializeField] SkillTree skillTree;

    [SerializeField] private GameObject spellCastPoint = default;
    [SerializeField] private GameObject shieldCastPoint = default;

    [SerializeField] private float angle = 15f;

    private List<GameObject> castingProjectiles = new List<GameObject>();

    private Vector3 baseTargetScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 targetScale;

    private GameObject castingShield;

    float scaleTime = .5f;
    float currentScaleTime = 0.0f;

    public float ScaleTime { get { return scaleTime; } private set { scaleTime = value; } }

    private void Update()
    {
        // Time managed in Update instead of in coroutine seems to produce expected results
        if (currentScaleTime <= scaleTime)
        {
            currentScaleTime += Time.deltaTime;
        }
    }

    public void StartProjectileCast(GameObject projectile, Element element)
    {
        Tuple<int, int> elementLevels = skillTree.ElementLevels(element);

        int projectilesLvl = elementLevels.Item1;
        int projectileMultiplierLvl = elementLevels.Item2;

        
        float multiplier = 1f + (projectileMultiplierLvl * skillTree.MultiplierValue);


        StartCoroutine(ChargingProjectileSpellRoutine(projectile, projectilesLvl, multiplier));
    }


    public void CastProjectileSpell()
    {
        if (castingProjectiles != null || castingProjectiles.Count != 0)
        {
            Vector3 direction = SpawnDirection();

            castProjectile(0, direction);

            if (castingProjectiles.Count > 1)
            {
                CastAdditionalProjectiles(direction);
            }
        }

        FinishProjectileCast();

    }


    public void CastElementShield(GameObject shield)
    {
        Vector3 direction = shieldCastPoint.transform.forward;
        direction.y = 0;


        castingShield = Instantiate(shield, shieldCastPoint.transform.position, Quaternion.LookRotation(direction), shieldCastPoint.transform);
        castingShield.transform.Rotate(new Vector3(0, 0, 90));
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


    private IEnumerator ChargingProjectileSpellRoutine(GameObject projectileToCast, int projectilesLvl, float multiplier)
    {
        currentScaleTime = 0.0f;

        Vector3 direction = SpawnDirection();
        Vector3 position = spellCastPoint.transform.position;

        InstantiateProjectile(projectileToCast, position, direction);

        if (projectilesLvl > 0)
        {
            ChargeAdditionalProjectile(projectileToCast, direction, projectilesLvl);
        }

        SetTargetScale(multiplier);

        currentScaleTime = 0;

        while (currentScaleTime < scaleTime && castingProjectiles != null)
        {
            ScaleAllChargingProjectiles(currentScaleTime, scaleTime);

            yield return null;
        }
    }


    private void InstantiateProjectile(GameObject projectileToCast, Vector3 startPosition, Vector3 lookDirection)
    {
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        Transform parent = spellCastPoint.transform;

        GameObject projectile = Instantiate(projectileToCast, startPosition , rotation, parent);

        castingProjectiles.Add(projectile);
    }


    private Vector3 SpawnDirection()
    {
        Vector3 direction = spellCastPoint.transform.forward;
        direction.y = 0;
        return direction;
    }


    private void ChargeAdditionalProjectile(GameObject projectileToCast, Vector3 lookDirection, int projectilesLvl)
    {
        Vector3 basePosition = spellCastPoint.transform.position;

        Vector3 right = spellCastPoint.transform.right;
        Vector3 forward = spellCastPoint.transform.forward * 0.1f;
        Vector3 rightSpawnOffset = (right - forward) * 0.75f;
        Vector3 leftSpawnOffset = ((right * -1) - forward) * 0.75f;

        for (int i = 1; i <= projectilesLvl; i++)
        {
            Vector3 rightDirection = TurnDirectionByAngle(lookDirection, angle * i);
            Vector3 leftDirection = TurnDirectionByAngle(lookDirection, -1 * (angle * i));

            Vector3 rightSpawn = basePosition + (i * rightSpawnOffset);
            Vector3 leftSpawn = basePosition + (i * leftSpawnOffset);

            InstantiateProjectile(projectileToCast, rightSpawn, rightDirection);
            InstantiateProjectile(projectileToCast, leftSpawn, leftDirection);
        }
    }
    

    private Vector3 TurnDirectionByAngle(Vector3 startDirection, float angle)
    {
        Vector3 direction = Quaternion.Euler(0, angle, 0) * startDirection;
        return direction;
    }


    private void SetTargetScale(float multiplier)
    {
        targetScale = baseTargetScale * multiplier;
    }


    private void ScaleAllChargingProjectiles(float timeScaled, float scaleTime)
    {
        for (int i = 0; i < castingProjectiles.Count; i++)
        {
            if (castingProjectiles[i] != null)
            {
                castingProjectiles[i].transform.localScale = Vector3.Lerp(startScale, targetScale, timeScaled / scaleTime);

            }
        }
    }


    private void castProjectile(int index, Vector3 direction)
    {
        if (index >= castingProjectiles.Count)
        {
            return;
        }
        if (castingProjectiles[index] != null)
        {
            castingProjectiles[index].GetComponent<ProjectileSpellBehaviour>().CastProjectile(direction, startScale);
        }
    }


    private void CastAdditionalProjectiles(Vector3 direction)
    {
        int angleIncrease = 1;
        for (int i = 1; i < castingProjectiles.Count; i += 2)
        {
            Vector3 rightDirection = TurnDirectionByAngle(direction, (angle * angleIncrease));
            Vector3 leftDirection = TurnDirectionByAngle(direction, -1 * (angle * angleIncrease));

            castProjectile(i, rightDirection);
            castProjectile(i + 1, leftDirection);

            angleIncrease++;
        }
    }


    private void FinishProjectileCast()
    {
        castingProjectiles.Clear();
    }


    // The skilltree is a Scriptable Object instance and persists through scenes
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
