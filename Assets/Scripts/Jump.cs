using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float groundCheckDepth = 0.1f;
    [SerializeField] private float jumpMultiplier = 500f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private Collider2D playerCollider;
    private Rigidbody2D playerRigidbody;
    private float defaultGravity;
    private bool isGrounded;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = transform.GetComponent<Collider2D>();
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

    void OnDrawGizmosSelected()
    {
        DrawGroundCheck();
    }

    private void DrawGroundCheck()
    {
        if (playerCollider == null) return;

        float boxWidth = playerCollider.bounds.size.x;
        float boxHeight = groundCheckDepth;
        Gizmos.color = isGrounded ? Color.green : Color.grey;
        Vector3 p1 = playerCollider.bounds.min;
        Vector3 p2 = p1 + Vector3.right * boxWidth;
        Vector3 p3 = p2 + Vector3.down * boxHeight;
        Vector3 p4 = p1 + Vector3.down * boxHeight;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
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
