using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private bool alive = true;

    [SerializeField] private SkillTree[] possibleSkillTrees;
    private SkillTree skillTree;
    [Space(5)]

    private Element[] availableElements;

    [SerializeField] private float minCD = 2f;
    [SerializeField] private float maxCD = 5f;
    [SerializeField] private float reactionTimeMin = 0.2f;
    [SerializeField] private float reactionTimeMax = 0.5f;
    [SerializeField] private float range = 15f;
    [Space(5)]

    [Header("Elemental Projectiles")]
    [SerializeField] private GameObject earthProjectile;
    [SerializeField] private GameObject fireProjetile;
    [SerializeField] private GameObject waterProjectile;
    [Space(5)]

    GameObject currentElementalProjectile;

    [Header("Elemental Shields")]
    [SerializeField] private GameObject earthShield;
    [SerializeField] private GameObject fireShield;
    [SerializeField] private GameObject waterShield;
    public GameObject activeShield;
    [Space(5)]

    bool shieldActivated = false;
    bool playerInRange = false;

    private SpellCastBehaviour spellCast;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        int r = Random.Range(0, possibleSkillTrees.Length);
        skillTree = possibleSkillTrees[r];

        availableElements = skillTree.AvailableElements;

        spellCast = GetComponent<SpellCastBehaviour>();
        spellCast.SetEnemySkillTree(skillTree);

        player = GameObject.Find("Player");

        StartCoroutine(SpellCastCoroutine());
    }

    void Update()
    {
        
        if (player != null && Vector3.Distance(this.transform.position, player.transform.position) < range)
        {
            playerInRange = true;
            Vector3 playerPosition = player.transform.position;
            playerPosition.y = this.transform.position.y; // avoid tilting of this is taller or smaller than player
            this.transform.LookAt(playerPosition);
        }
        else
        {
            playerInRange = false;
        }
    }

    private IEnumerator SpellCastCoroutine()
    {
        // Choose random element, and cast  that projectile
        while (alive == true)
        {
            yield return new WaitForSeconds(Random.Range(minCD, maxCD));
                     
            // Don't cast spell if are currently casting a shield
            if (activeShield == true || playerInRange == false) continue;

            int r = Random.Range(0, availableElements.Length);
            Element element = availableElements[r];

            GameObject spell = fireProjetile; //TODO: this is a workaround. Find a smarter solution.

            switch (element.ElementType)
            {
                case Element.ElementEnum.Normal:
                    break;
                case Element.ElementEnum.Fire:
                    spell = fireProjetile;
                    break;
                case Element.ElementEnum.Water:
                    spell = waterProjectile;
                    break;
                case Element.ElementEnum.Earth:
                    spell = earthProjectile;
                    break;
                default:
                    break;
            }

            spellCast.StartProjectileCast(this.gameObject, spell, element, player.transform.position);
        }
    }

    private IEnumerator shieldCastCoroutine(GameObject shield)
    {
        yield return new WaitForSeconds(Random.Range(reactionTimeMin, reactionTimeMax));

        spellCast.CastElementShield(this.gameObject, shield, player.transform.position);

        yield return new WaitForSeconds(Random.Range(1f, 2f));
        shieldActivated = false;
        Destroy(activeShield);

    }

    private void OnTriggerEnter(Collider other)
    {
        // Only cast shield if we are not already casting
        if (shieldActivated == true) return; 

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
                }
            }

            if (counterElementAvailable == true)
            {

                GameObject shield = fireShield; //TODO: This is a workaround. Find a smarter solution
                switch (counterElement.ElementType)
                {
                    case Element.ElementEnum.Normal:
                        break;
                    case Element.ElementEnum.Fire:
                        shield = fireShield;
                        break;
                    case Element.ElementEnum.Water:
                        shield = waterShield;
                        break;
                    case Element.ElementEnum.Earth:
                        shield = earthShield;
                        break;
                    default:
                        break;
                }

                shieldActivated = true;
                StartCoroutine(shieldCastCoroutine(shield));
            }
        }
    }
    private void OnDestroy()
    {
        if (activeShield != null)
        {
            Destroy(activeShield.gameObject);
        }
    }
}
