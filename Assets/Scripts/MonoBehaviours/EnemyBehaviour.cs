using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool alive = true;

    [SerializeField] private SkillTree[] possibleSkillTrees = default;
    private SkillTree skillTree;
    [Space(5)]

    private Element[] availableElements;

    [Header("Behaviour Values")]
    [SerializeField] private float minCD = 2f;
    [SerializeField] private float maxCD = 5f;
    [SerializeField] private float reactionTimeMin = 0.2f;
    [SerializeField] private float reactionTimeMax = 0.5f;
    [SerializeField] private float range = 15f;
    [SerializeField] private float rotationSpeed = 5f;
    [Space(5)]


    [Header("Elements")]

    [SerializeField] private Element earthElement;
    [SerializeField] private Element fireElement;
    [SerializeField] private Element waterElement;

    [Space(5)]


    [SerializeField] private GameObject spellCastPoint = default;

    private bool isShieldCasting;
    private bool isProjectileCharging;
    private bool isPlayerInRange;

    private SpellCastBehaviour spellCast;
    private float projectileChargeTime;

    private Vector3 playerPosition;
    private Quaternion rotation;

    private GameObject player;


    private void Start()
    {
        SetupSpellcast();

        player = GameObject.Find("Player");

        StartCoroutine(SpellCastCoroutine());
    }

    private void Update()
    {
        CheckIfPlayerIsRange();
        RotateMe();
        RotateSpellCastPoint();
    }


    private void SetupSpellcast()
    {
        int r = Random.Range(0, possibleSkillTrees.Length);
        skillTree = possibleSkillTrees[r];

        availableElements = skillTree.AvailableElements;

        spellCast = GetComponent<SpellCastBehaviour>();
        spellCast.SetEnemySkillTree(skillTree);
        projectileChargeTime = spellCast.ScaleTime;
    }


    private IEnumerator SpellCastCoroutine()
    {
        // Choose random element, and cast  that projectile
        while (alive == true)
        {
            yield return new WaitForSeconds(Random.Range(minCD, maxCD));
                     
            if (!CanCast()) continue;


            Element element = ChooseRandomElement();

            StartCastintElementProjectile(element);

            float chargeTime = ChooseRandomChargeTime();

            yield return new WaitForSeconds(chargeTime); // wait out charge time, then cast

            isProjectileCharging = false;
            spellCast.CastProjectileSpell();
            
        }
    }


    private bool CanCast()
    {
        return !isProjectileCharging && !isShieldCasting && isPlayerInRange;
    }


    private Element ChooseRandomElement()
    {
        int r = Random.Range(0, availableElements.Length);
        return availableElements[r];
    }


    private void StartCastintElementProjectile(Element element)
    {
        GameObject spell = element.ElementalSpellPrefab;

        isProjectileCharging = true;
        spellCast.StartProjectileCast(spell, element);
    }


    private float ChooseRandomChargeTime()
    {
        float chargeTime = Random.Range(0.2f, projectileChargeTime + projectileChargeTime * 0.25f);
        return chargeTime;
    }
    

    private void CheckIfPlayerIsRange()
    {
        if (player != null && Vector3.Distance(this.transform.position, player.transform.position) < range)
        {
            isPlayerInRange = true;
            playerPosition = player.transform.position;
            UpdateRotation();
        }
        else
        {
            isPlayerInRange = false;
        }
    }


    private void UpdateRotation()
    {
        rotation = rotationTowardsPoint(playerPosition);
    }


    private void RotateMe()
    {
        if (playerPosition != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        
    }


    private void RotateSpellCastPoint()
    {
        if (playerPosition != null)
        {
            spellCastPoint.transform.rotation = Quaternion.Slerp(spellCastPoint.transform.rotation, rotation, (rotationSpeed * 1.5f) * Time.deltaTime);
        }
    }


    private Quaternion rotationTowardsPoint(Vector3 point)
    {
        Vector3 rotateTowards = point - this.transform.position;
        rotateTowards.y = 0;

        Quaternion toRotation = Quaternion.LookRotation(rotateTowards);

        return toRotation;
    }


    private IEnumerator shieldCastCoroutine(GameObject shield)
    {
        yield return new WaitForSeconds(Random.Range(reactionTimeMin, reactionTimeMax));

        spellCast.CastElementShield(shield);

        yield return new WaitForSeconds(Random.Range(1f, 2f));
        isShieldCasting = false;
        spellCast.StopCastingShield();

    }


    private void OnTriggerEnter(Collider other)
    {
        // Only cast shield if we are not already casting
        if (isShieldCasting || isProjectileCharging) return; 

        if (other.tag == "PlayerElementalProjectile")
        {
            PlayerProjectileDetected(other.gameObject);
        }
    }


    private void PlayerProjectileDetected(GameObject projectile)
    {
        Element enemyElement = GetPlayerProjectileElement(projectile);

        Element counterElement = GetCounterElementIfAvailable(enemyElement);

        if (counterElement != null)
        {
            CastShield(counterElement);
        }
    }


    private Element GetPlayerProjectileElement(GameObject projectile)
    {
        Element projectileElement = projectile.GetComponent<ProjectileSpellBehaviour>().projectileSpell.Element;
        return projectileElement;
    }


    private Element GetCounterElementIfAvailable(Element elementToCounter)
    {
        Element counterElement = null;
        bool canCounter = false;
        foreach (Element element in availableElements)
        {
            if (elementToCounter.Countered(element.ElementType, elementToCounter.ElementType) == true)
            {
                canCounter = true;
                counterElement = element;
                break;
            }
        }

        if (canCounter)
        {
            return counterElement;
        }
        else
        {
            return null;
        }
    }

    private void CastShield(Element element)
    {
        GameObject shield = element.ElementalShieldPrefab;

        isShieldCasting = true;
        StartCoroutine(shieldCastCoroutine(shield));
    }
}
