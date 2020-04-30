using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpellBehaviour : MonoBehaviour
{
    public ProjectileSpell projectileSpell;

    private bool hasBeenCast;
    private float range;
    private float speed;
    public float damage;

    private Vector3 startPosition;

    private Rigidbody rigidBody;

    private Vector3 startScale;


    // Start is called before the first frame update
    void Start()
    {
        NullChecks();

        range = projectileSpell.Range;
        speed = projectileSpell.Speed;
        damage = projectileSpell.Damage;

        rigidBody = this.gameObject.GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {
        DestroyOnMaxRange();
    }

    public void CastProjectile(Vector3 direction, Vector3 originalScale)
    {
        this.transform.parent = null;
        startPosition = this.transform.position;

        hasBeenCast = true;

        rigidBody.AddForce(direction * speed);

        float scale = this.transform.localScale.magnitude / originalScale.magnitude;
        float scalingFactor = 0.25f * Mathf.Pow(3, scale);
        scalingFactor = (float)System.Math.Round(scalingFactor, 1);
        damage *= scalingFactor;
        rigidBody.mass *= scalingFactor;


    }


    void DestroyOnMaxRange()
    {
        if (hasBeenCast &&  Vector3.Distance(startPosition, transform.position) > range)
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


    void NullChecks()
    {
        if (projectileSpell == null)
        {
            Debug.LogError("ProjectileSpellBehaviour:: projectileSpell is NULL");
        }
    }
}
