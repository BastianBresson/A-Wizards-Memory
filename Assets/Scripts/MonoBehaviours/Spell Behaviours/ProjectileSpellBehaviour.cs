using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpellBehaviour : MonoBehaviour
{
    public ProjectileSpell projectileSpell;

    private bool canSpawnOnDestroy = true;

    private bool hasBeenCast;
    private float range;
    private float speed;
    private float damage;

    private Vector3 startPosition;
    private Vector3 startScale;

    private Rigidbody rigidBody;


    private void Start()
    {
        NullChecks();

        range = projectileSpell.Range;
        speed = projectileSpell.Speed;
        damage = projectileSpell.Damage;

        rigidBody = this.gameObject.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;

    }


    private void Update()
    {
        DestroyOnMaxRange();
    }


    public void CastProjectile(Vector3 direction, Vector3 originalScale)
    {
        OnCastSetVariables();

        LaunchProjectile(direction); //should happen before increasing mass for consistent speed

        float scaleIncrease = ScaleIncreaseFactor(originalScale);
        IncreaseDamageAndMass(scaleIncrease);
    }


    private void OnCastSetVariables()
    {
        this.transform.parent = null;
        startPosition = this.transform.position;

        hasBeenCast = true;

        rigidBody.isKinematic = false;
    }


    private void LaunchProjectile(Vector3 direction)
    {
        rigidBody.AddForce(direction * speed);
    }


    private float ScaleIncreaseFactor(Vector3 originalScale)
    {
        float scale = this.transform.localScale.magnitude / originalScale.magnitude;
        float scalingFactor = 0.25f * Mathf.Pow(2, scale);
        scalingFactor = (float)System.Math.Round(scalingFactor, 1);

        return scalingFactor;
    }


    private void IncreaseDamageAndMass(float scaleIncrease)
    {
        damage *= scaleIncrease;
        rigidBody.mass *= scaleIncrease;
    }


    private void DestroyOnMaxRange()
    {
        if (hasBeenCast &&  Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        // projectiles should not be destroyed on collision with other projetiles
        if (!IsProjectileColliosion(collidedObject)) 
        {
            DealDamageIfObjectHasHealth(collidedObject);

            Destroy(this.gameObject);
        }
    }


    private bool IsProjectileColliosion(GameObject collidedObject)
    {
        bool isPlayerProjectile = collidedObject.tag == "PlayerElementalProjectile";
        bool isEnemyProjectile = collidedObject.tag == "EnemyElementalProjectile";

        return isPlayerProjectile || isEnemyProjectile;
    }


    private void DealDamageIfObjectHasHealth(GameObject collidedObject)
    {
        if (collidedObject.tag == "Enemy" || collidedObject.tag == "Player" || collidedObject.tag == "DestructableObject")
        {
            collidedObject.GetComponent<HealthBehaviour>().ReceiveDamage(this.damage);
        }
    }


    private void OnDestroy()
    {
        if (!canSpawnOnDestroy) return;

        SpawnExplosionParticle();
    }


    private void SpawnExplosionParticle()
    {
        Quaternion spawnRotation = Quaternion.LookRotation(-1 * this.transform.forward);
        GameObject particle = Instantiate(projectileSpell.ExplosionParticle, transform.position, spawnRotation);

        float explosionDuration = particle.GetComponent<ParticleSystem>().main.duration;

        MoveProjectileLight(explosionDuration);

        Destroy(particle, explosionDuration);
    }

    private void MoveProjectileLight(float duration)
    {
        Light light = this.gameObject.GetComponentInChildren<Light>();
        light.enabled = true;

        light.transform.parent = null;
        light.transform.position = this.transform.position;

        Destroy(light.gameObject, duration);
    }


    private void OnApplicationQuit()
    {
        canSpawnOnDestroy = false;
    }



    void NullChecks()
    {
        if (projectileSpell == null)
        {
            Debug.LogError("ProjectileSpellBehaviour:: projectileSpell is NULL");
        }
    }
}
