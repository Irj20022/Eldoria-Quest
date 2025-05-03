using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform respawnPoint; 
    private int savedWeaponSlot = 0;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;
    private bool facingLeft = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();

        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AssignRespawnPoint();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.gettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    public void RespawnPlayer()
    {
        savedWeaponSlot = ActiveInventory.Instance.GetActiveSlotIndex();
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        rb.velocity = Vector2.zero;

        // Reset death animation
        myAnimator.ResetTrigger("Death");
        myAnimator.SetTrigger("Idle");

        mySpriteRender.enabled = false;
        playerControls.Disable();

        yield return new WaitForSeconds(1.5f);

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Respawn point is missing!");
        }

        mySpriteRender.enabled = true;
        playerControls.Enable();

        PlayerHealth.Instance.ResetHealth();
        ActiveInventory.Instance.SetActiveSlot(savedWeaponSlot);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignRespawnPoint();
    }

    private void AssignRespawnPoint()
    {
        GameObject found = GameObject.FindWithTag("RespawnPoint");
        if (found != null)
        {
            respawnPoint = found.transform;
        }
        else
        {
            Debug.LogWarning("No RespawnPoint found in scene: " + SceneManager.GetActiveScene().name);
        }
    }
}
