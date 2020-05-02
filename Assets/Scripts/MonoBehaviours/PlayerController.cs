using System;
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
    }


    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }


    private void SetComponents()
    {
        if (currentElement == null)
        {
            currentElement = earthElement;
        }

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
        movementDirection *= movementSpeed;
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
            debugHit = hit;
            Quaternion rotation = RotationTowardsPoint(hit.point);

            GraduallyRotatePlayer(rotation);

            GraduallyRotateSpellPoint(rotation);
        }
        
    }


    private Quaternion RotationTowardsPoint(Vector3 point)
    {
        Vector3 rotateTowards = point - this.transform.position;
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
        spellCastPoint.transform.rotation = Quaternion.Slerp(spellCastPoint.transform.rotation, rotation, (rotationSpeed * 1.5f) * Time.deltaTime);
    }


    private void InputDirection(Vector2 inputDirection)
    {
        direction = inputDirection;
    }


    private void InputCastElementalProjectilePerformed()
    {
        if (!isProjectileCasting && !isShieldCasting)
        {
            isProjectileCasting =  true;
            
            spellCast.StartProjectileCast(currentElement.ElementalSpellPrefab, currentElement);
        }
    }


    private void InputCastElementalProjectileCanceled()
    {
        if (isProjectileCasting && !isShieldCasting)
        {
            isProjectileCasting = false;

            spellCast.CastProjectileSpell();
        }
    }


    private void InputCastShieldPerformed()
    {
        if (!isShieldCasting && !isProjectileCasting)
        {
            isShieldCasting = true;

            spellCast.CastElementShield(currentElement.ElementalShieldPrefab);
        }

    }


    private void InputCastShieldCanceled()
    {
        if (isShieldCasting && !isProjectileCasting)
        {
            isShieldCasting = false;

            spellCast.StopCastingShield();
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
