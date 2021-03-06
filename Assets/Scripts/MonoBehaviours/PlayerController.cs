﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool canSelect;
    private SelectBehaviour selector;

    // Movment
    private InputMaster controls;
    private CharacterController controller;
    private Rigidbody rigidBody;

    private Camera mainCamera;

    private Vector2 direction = Vector2.zero;
    private Vector3 movementDirection = Vector3.zero;


    [Header("Movement")]
    private float currentMovementSpeed;
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float rotationSpeed = 5f;
    [Space(5)]

    [SerializeField] private GameObject spellCastPoint = default;

    [Header("Elements")]
    [SerializeField] private Element earthElement = default;  
    [SerializeField] private Element fireElement = default;
    [SerializeField] private Element waterElement = default;
    [Space(5)]


    private static Element currentElement;
    private bool isProjectileCasting;
    private bool isShieldCasting;

    SpellCastBehaviour spellCast;


    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Move.performed += context => InputDirection(context.ReadValue<Vector2>());

        controls.Player.CastElementalProjectile.performed += _ => InputCastElementalProjectilePerformed();
        controls.Player.CastElementalProjectile.canceled += _ => InputCastElementalProjectileCanceled();
        controls.Player.CastElementShield.performed += _ => InputCastShieldPerformed();
        controls.Player.CastElementShield.canceled += _ => InputCastShieldCanceled();

        controls.Player.EarthElement.performed += _ => InputEarthElement();
        controls.Player.FireElement.performed += _ => InputFireElement();
        controls.Player.WaterElement.performed += _ => InputWaterElement();

        controls.Player.Select.performed += _ => InputSelect();
    }


    void Start()
    {
        SetComponents();

        SetPlayerPositionInMemoryScene();
    }


    private void Update()
    {
        UpdateMovementDireciton();

        PlayerRotation();
    }


    private void FixedUpdate()
    {
        PlayerMovement();
    }


    private void SetComponents()
    {
        if (currentElement == null)
        {
            currentElement = earthElement;
        }

        currentMovementSpeed = movementSpeed;
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        spellCast = GetComponent<SpellCastBehaviour>();
        mainCamera = Camera.main;
    }


    private void SetPlayerPositionInMemoryScene()
    {    
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "MemoryLevel")
        {
            Vector3 pos = GameManager.Instance.PlayerPosition;
            this.transform.position = pos;
        }
    }


    private void PlayerMovement()
    {
        MovePlayer();
    }


    private void UpdateMovementDireciton()
    {
        movementDirection = new Vector3(direction.x, 0.0f, direction.y);
        movementDirection *= currentMovementSpeed;
    }


    private void MovePlayer()
    {
        float currentVerticalMovement = rigidBody.velocity.y;
        Vector3 newVelocity = movementDirection;
        newVelocity.y = currentVerticalMovement;
        rigidBody.velocity = newVelocity;
    }

    private RaycastHit debugHit;
    private void PlayerRotation()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.SphereCast(ray, 0.2f,  out hit))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.point = hit.collider.gameObject.transform.position;
            }
            debugHit = hit;

            Quaternion playerRotation = RotationTowardsPoint(this.transform.position, hit.point);
            Quaternion spellPointRotation = RotationTowardsPoint(spellCastPoint.transform.position, hit.point);

            GraduallyRotatePlayer(playerRotation);

            GraduallyRotateSpellPoint(spellPointRotation);
        }
        
    }


    private Quaternion RotationTowardsPoint(Vector3 startPosition, Vector3 toPosition)
    {
        Vector3 rotateTowards = toPosition - startPosition;
        rotateTowards.y = 0;

        Quaternion toRotation = Quaternion.LookRotation(rotateTowards);

        return toRotation;
    }


    private void GraduallyRotatePlayer(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }


    private void GraduallyRotateSpellPoint(Quaternion rotation)
    {
        spellCastPoint.transform.rotation = Quaternion.Slerp(spellCastPoint.transform.rotation, rotation, (rotationSpeed * 2f) * Time.deltaTime);
    }


    private void InputDirection(Vector2 inputDirection)
    {
        direction = inputDirection;
    }


    private void InputCastElementalProjectilePerformed()
    {
        if (!isProjectileCasting && !isShieldCasting)
        {
            SlowDownMovementSpeed();

            isProjectileCasting =  true;
            
            spellCast.StartProjectileCast(currentElement.ElementalSpellPrefab, currentElement);
        }
    }


    private void InputCastElementalProjectileCanceled()
    {
        if (isProjectileCasting && !isShieldCasting)
        {
            SpeedUpMovementSpeed();

            isProjectileCasting = false;

            spellCast.CastProjectileSpell();
        }
    }


    private void InputCastShieldPerformed()
    {
        if (!isShieldCasting && !isProjectileCasting)
        {
            SlowDownMovementSpeed();

            isShieldCasting = true;

            spellCast.CastElementShield(currentElement.ElementalShieldPrefab);
        }

    }


    private void InputCastShieldCanceled()
    {
        if (isShieldCasting && !isProjectileCasting)
        {
            SpeedUpMovementSpeed();

            isShieldCasting = false;

            spellCast.StopCastingShield();
        }
    }


    private void SlowDownMovementSpeed()
    {
        if (this.gameObject != null)
        {
            StopCoroutine(SpeedUpMomementSpeedCoroutine());

            StartCoroutine(SlowDownMomementSpeedCoroutine());
        }
    }


    IEnumerator SlowDownMomementSpeedCoroutine()
    {
        while (currentMovementSpeed > (movementSpeed / 2))
        {
            currentMovementSpeed -= 5 * Time.deltaTime;
            yield return null;
        }
    }


    private void SpeedUpMovementSpeed()
    {
        if (this.gameObject != null)
        {
            StopCoroutine(SlowDownMomementSpeedCoroutine());

            StartCoroutine(SpeedUpMomementSpeedCoroutine());
        }
    }


    IEnumerator SpeedUpMomementSpeedCoroutine()
    {
        while (currentMovementSpeed < movementSpeed)
        {
            currentMovementSpeed += 5 * Time.deltaTime;
            yield return null;
        }
    }


    private void InputEarthElement()
    {
        currentElement = earthElement;
    }


    private void InputFireElement()
    {
        currentElement = fireElement;
    }


    private void InputWaterElement()
    {
        currentElement = waterElement;
    }


    private void InputSelect()
    {
        if (canSelect == true)
        {
            selector.Selected();
        }
    }


    public void SelectCollision(bool entered, SelectBehaviour selector)
    {
        canSelect = entered;

        this.selector = entered == true ? selector : null;

    }


    private void OnEnable()
    {
        controls.Enable();
    }


    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(debugHit.point, 0.2f);
    }
}
