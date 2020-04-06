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

    Vector2 direction = Vector2.zero;
    public Vector3 currentDirection = Vector3.zero;

    [Header("Movement")]
    [SerializeField] float speed = 4f;
    [SerializeField] float gravity = 20f;
    [Space(5)]

    #region Elemental Assets/Prefabs

    [Header("Elements")]
    [SerializeField] Element earthElement;  
    [SerializeField] Element fireElement;
    [SerializeField] Element waterElement;
    [Space(5)]

    Element currentElement;

    [Header("Elemental Projectiles")]
    [SerializeField] GameObject earthProjectile;
    [SerializeField] GameObject fireProjetile;
    [SerializeField] GameObject waterProjectile;
    [Space(5)]

    GameObject currentElementalProjectile;

    [Header("Elemental Shields")]
    [SerializeField] GameObject earthShield;
    [SerializeField] GameObject fireShield;
    [SerializeField] GameObject waterShield;
    public GameObject activeShield;
    [Space(5)]

    GameObject currentShield;
    bool shieldActivated = false;

    ElementLevelSelectBehaviour levelSelector;
    
    #endregion

    SpellCastBehaviour spellCast;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Move.performed += context => InputDirection(context.ReadValue<Vector2>());

        controls.Player.EarthElement.performed += _ => InputEarthElement();
        controls.Player.FireElement.performed += _ => InputFireElement();
        controls.Player.WaterElement.performed += _ => InputWaterElement();
        controls.Player.CastElementalProjectile.performed += _ => InputCastElementalProjectile();
        controls.Player.CastElementShield.performed += _ => InputCastShield();
        controls.Player.Select.performed += _ => InputSelect();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        spellCast = GetComponent<SpellCastBehaviour>();

        // Sets initial projectile & element to earth
        InputEarthElement();

        // Check if player should spawn other places than default location
        Vector3 pos = GameManager.Instance.PlayerPosition;
        Scene currentScene = SceneManager.GetActiveScene();

        // TODO: This is not consistently spawing the player at the last selected selector.
        if (currentScene.name == "MemoryLevel" && pos != null) { this.transform.position = pos; }

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (controller.isGrounded == true)
        {
            currentDirection = new Vector3(direction.x, 0.0f, direction.y);
            currentDirection *= speed;
        }

        currentDirection.y -= gravity * Time.deltaTime;

        controller.Move(currentDirection * Time.deltaTime);
    }

    private void InputDirection(Vector2 inputDirection)
    {
        direction = inputDirection;
    }

    private void InputCastElementalProjectile()
    {
        if (shieldActivated == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                spellCast.CastElementalProjectile(this.gameObject, currentElementalProjectile, currentElement, hit.point);
            }
        }
    }

    private void InputCastShield()
    {
        if (shieldActivated == false)
        {
            shieldActivated = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                spellCast.CastElementShield(this.gameObject, currentShield, hit.point);
            }
        }
        else if (shieldActivated)
        {
            shieldActivated = false;
            activeShield.GetComponent<ShieldBehaviour>().StopCasting();
        }
    }

    //TODO check if element is available
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
}
