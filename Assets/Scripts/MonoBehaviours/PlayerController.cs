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
    InputMaster controls;
    CharacterController controller;

    Vector3 rotateTowards;
    Camera mainCamera;

    Vector2 direction = Vector2.zero;
    Vector3 movementDirection = Vector3.zero;

    [Header("Movement")]
    [SerializeField] float movementSpeed = 4f;
    [SerializeField] float gravity = 20f;
    [SerializeField] float rotationSpeed = 5f;
    [Space(5)]

    #region Elemental Assets/Prefabs

    [Header("Elements")]
    [SerializeField] Element earthElement = default;  
    [SerializeField] Element fireElement = default;
    [SerializeField] Element waterElement = default;
    [Space(5)]

    [Header("Elemental Projectiles")]
    [SerializeField] GameObject earthProjectile = default;
    [SerializeField] GameObject fireProjetile = default;
    [SerializeField] GameObject waterProjectile = default;
    [Space(5)]

    [Header("Elemental Shields")]
    [SerializeField] GameObject earthShield = default;
    [SerializeField] GameObject fireShield = default;
    [SerializeField] GameObject waterShield = default;
    public GameObject activeShield;
    [Space(5)]

    private GameObject currentShield;
    private GameObject currentElementalProjectile;
    private Element currentElement;
    private bool isProjectileCasting;
    private bool isShieldCasting;

    ElementLevelSelectBehaviour levelSelector;
    
    #endregion

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

    // Start is called before the first frame update
    void Start()
    {
        SetComponents();

        // Sets initial projectile & element to earth
        InputEarthElement();

        SetPlayerPositionInMemoryScene();

    }


    void Update()
    {
        PlayerMovement();
    }


    private void FixedUpdate()
    {
        PlayerRotation();
    }


    private void SetComponents()
    {
        controller = GetComponent<CharacterController>();
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
        if (controller.isGrounded == true)
        {
            GroundMovement();
        }

        ApplyGravity();

        MovePlayer();
    }


    private void GroundMovement()
    {
        movementDirection = new Vector3(direction.x, 0.0f, direction.y);
        movementDirection *= movementSpeed;
    }


    private void ApplyGravity()
    {
        movementDirection.y -= gravity * Time.deltaTime;
    }


    private void MovePlayer()
    {
        controller.Move(movementDirection * Time.deltaTime);
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

            spellCast.UpdateSpellCastRotation(hit.point);
        }
        
    }


    private Quaternion RotationTowardsPoint(Vector3 point)
    {
        rotateTowards = point - this.transform.position;
        rotateTowards.y = 0;

        Quaternion toRotation = Quaternion.LookRotation(rotateTowards);

        return toRotation;
    }


    private void GraduallyRotatePlayer(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
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

            spellCast.StartProjectileCast(currentElementalProjectile, currentElement);
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

            spellCast.CastElementShield(currentShield);
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
        currentElementalProjectile = earthProjectile;
        currentShield = earthShield;
    }


    private void InputFireElement()
    {
        currentElement = fireElement;
        currentElementalProjectile = fireProjetile;
        currentShield = fireShield;
    }


    private void InputWaterElement()
    {
        currentElement = waterElement;
        currentElementalProjectile = waterProjectile;
        currentShield = waterShield;
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
