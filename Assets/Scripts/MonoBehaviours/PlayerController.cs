using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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
    [SerializeField] private float movementSpeedIncreaseRate = 5f;
    [SerializeField] private float movementSpeedDecreaseRate = 5f;
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


    public void OnGameStarted()
    {
        controls.Enable();
    }


    public void OnSelectCollision(bool entered, SelectBehaviour selector)
    {
        canSelect = entered;

        this.selector = entered == true ? selector : null;

    }


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

        controls.Player.Map.performed += _ => InputMap();
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

        ControlMovementSpeedWhenCasting();
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

        if (currentScene.name == "MemoryLevel" && MainMenu.gameHasStarted)
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


    private void ControlMovementSpeedWhenCasting()
    {
        if (isShieldCasting || isProjectileCasting)
        {
            IncreaseMovementSpeed();
        }
        else if (currentMovementSpeed < movementSpeed)
        {
            DecreaseMovementSpeed();
        }
    }

    
    private void IncreaseMovementSpeed()
    {
        if (currentMovementSpeed > movementSpeed / 2)
        {
            currentMovementSpeed -= movementSpeedDecreaseRate * Time.deltaTime;
        }
    }


    private void DecreaseMovementSpeed()
    {
        currentMovementSpeed += movementSpeedIncreaseRate * Time.deltaTime;
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


    private void InputMap()
    {
        FindCamera().GetComponent<CameraController>().OnDisplayMap();
    }


    private void InputMenu()
    {

    }


    private GameObject FindCamera()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        return camera;
    }


    private void OnEnable()
    {
        if (MainMenu.gameHasStarted)
        {
            controls.Enable();
        }
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
