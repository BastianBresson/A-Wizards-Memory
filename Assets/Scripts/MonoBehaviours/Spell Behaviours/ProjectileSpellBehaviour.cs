using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpellBehaviour : MonoBehaviour
{
    public ProjectileSpell projectileSpell;
    
    private float range;
    private float speed;
    private float damage;

    private float scaleTime = 0.5f;

    Vector3 startPosition;
    public Vector3 direction = Vector3.zero;
    public Vector3 EndPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        NullChecks();

        startPosition = transform.position;
        range = projectileSpell.Range;
        speed = projectileSpell.Speed;
        damage = projectileSpell.Damage;

    }

    // Update is called once per frame
    void Update()
    {
        DestroyOnMaxRange();

        // Movement
        if (direction != Vector3.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    public void ScaleProjectile(float multiplier)
    {
        damage *= multiplier;
        StartCoroutine(ScaleOverTimeCoroutine(scaleTime, multiplier));
    }

    void DestroyOnMaxRange()
    {
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "PlayerElementalProjectile" && collision.gameObject.tag != "EnemyElementalProjectile")
        {
            projectileSpell.Collide(collision.gameObject, this.gameObject);
        }
    }

    IEnumerator ScaleOverTimeCoroutine(float time, float multiplier)
    {
        Vector3 originalScale = this.transform.localScale;
        Vector3 destinationScale = originalScale * multiplier;

        float currentTime = 0.0f;

        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);

            yield return null;
        }
    }

    void NullChecks()
    {
        if (projectileSpell == null)
        {
            Debug.LogError("ProjectileSpellBehaviour:: projectileSpell is NULL");
        }
    }
}
