using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float groundCheckDepth = 0.1f;
    [SerializeField] private float jumpMultiplier = 500f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRigidbody;
    private float defaultGravity;
    private bool isGrounded;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<BoxCollider2D>();
        defaultGravity = playerRigidbody.gravityScale;
    }

    void Update()
    {
        UpdateInput();

        // Add jump force
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            AddForce();
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

    private void UpdateInput()
    {
        // Update ground check
        isGrounded = PlayerIsGrounded();
    }

    private void AddForce()
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
}
