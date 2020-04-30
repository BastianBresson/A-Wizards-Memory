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
                     
            // Don't cast spell if are currently casting a shield
            if (isShieldCasting || !isPlayerInRange) continue;

            int r = Random.Range(0, availableElements.Length);
            Element element = availableElements[r];

            GameObject spell = element.ElementalSpellPrefab; 

            isProjectileCharging = true;
            spellCast.StartProjectileCast(spell, element);
            float chargeTime = Random.Range(0.2f, projectileChargeTime + projectileChargeTime * 0.25f);
            yield return new WaitForSeconds(chargeTime);
            spellCast.CastProjectileSpell();
        }
    }


    private bool CanCast()
    {
        return !isProjectileCharging && !isShieldCasting;
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
        spellCast.StopCastingShield();
        isShieldCasting = false;
    }


    public void ProjectileDetected()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        // Only cast shield if we are not already casting
        if (isShieldCasting || isProjectileCharging) return; 

        // Check if we can counter the spell, if we can, then cast shield.
        if (other.tag == "PlayerElementalProjectile")
        {
            Element enemyElement = other.GetComponent<ProjectileSpellBehaviour>().projectileSpell.Element;

            bool counterElementAvailable = false;
            Element counterElement = availableElements[0];

            foreach (Element element in availableElements)
            {
                if (enemyElement.Countered(element.ElementType, enemyElement.ElementType) == true)
                {
                    counterElementAvailable = true;
                    counterElement = element;
                    break;
                }
            }

            if (counterElementAvailable == true)
            {
                GameObject shield = counterElement.ElementalShieldPrefab;

                isShieldCasting = true;
                StartCoroutine(shieldCastCoroutine(shield));
            }
        }
    }
}
