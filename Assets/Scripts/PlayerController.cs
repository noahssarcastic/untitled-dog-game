using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float groundCheckDepth = 0.1f;
    [SerializeField] private float jumpMultiplier = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;

    private Vector2 velocity = Vector3.zero;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRigidbody;
    private float defaultGravity;
    private bool isGrounded;
    private Vector2 movement;
    private bool isMoving;
    private UnityEvent respawnEvent;

    void Awake()
    {
        GetComponent<Respawn>().respawn = Respawn;
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<BoxCollider2D>();
        defaultGravity = playerRigidbody.gravityScale;
    }

    void Start()
    {
        if (respawnEvent == null)
            respawnEvent = new UnityEvent();

        respawnEvent.AddListener(Respawn);
    }

    void Update()
    {
        UpdateInput();

        // Add jump force
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Change gravity curve
        float playerYVelocity = playerRigidbody.velocity.y;
        bool isPressingJump = Input.GetButton("Jump");
        if (!isGrounded && playerYVelocity < 0)
        {
            playerRigidbody.gravityScale = defaultGravity * fallMultiplier;
        }
        else if (!isGrounded && playerYVelocity > 0 && !isPressingJump)
        {
            playerRigidbody.gravityScale = defaultGravity * lowJumpMultiplier;
        }
        else
        {
            playerRigidbody.gravityScale = defaultGravity;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void UpdateInput()
    {
        // Update ground check
        isGrounded = PlayerIsGrounded();

        // Get horizontal movement
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), 0f);
        isMoving = input.x != 0f;
        movement = input;
    }

    private void Move()
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(movement.x * 10f, playerRigidbody.velocity.y);
        // And then smoothing it out and applying it to the character
        playerRigidbody.velocity = Vector2.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
    }

    private void ResetHorizontalVelocity()
    {
        playerRigidbody.velocity *= new Vector2(0, 1);
    }

    private void Jump()
    {
        playerRigidbody.AddForce(Vector2.up * jumpMultiplier);
    }

    private bool PlayerIsGrounded()
    {
        Vector2 playerCenter = playerCollider.bounds.center;
        float playerHeight = playerCollider.bounds.size.y;
        float centerToCenter = (playerHeight / 2) + (groundCheckDepth / 2);
        Vector2 groundCheckOffset = Vector2.down * centerToCenter;
        Vector2 groundCheckCenter = playerCenter + groundCheckOffset;
        Vector2 groundCheckSize = new Vector2(playerHeight, groundCheckDepth);
        Collider2D hit = Physics2D.OverlapBox(
            groundCheckCenter,
            groundCheckSize,
            0f,
            LayerMask.GetMask("Ground"));
        return hit != null;
    }

    private void Respawn()
    {
        transform.position = Vector3.zero;
    }
}
