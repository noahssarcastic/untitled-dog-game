using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleJump : MonoBehaviour
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
        // Add jump force
        if (Input.GetButtonDown("Jump"))
        {
            AddForce();
        }

        // // Change gravity curve
        // float playerYVelocity = playerRigidbody.velocity.y;
        // bool isPressingJump = Input.GetButton("Jump");
        // if (!isGrounded && playerYVelocity < 0)
        // {
        //     playerRigidbody.gravityScale = defaultGravity * fallMultiplier;
        // }
        // else if (!isGrounded && playerYVelocity > 0 && !isPressingJump)
        // {
        //     playerRigidbody.gravityScale = defaultGravity * lowJumpMultiplier;
        // }
        // else
        // {
        //     playerRigidbody.gravityScale = defaultGravity;
        // }
    }

    private void AddForce()
    {
        playerRigidbody.AddForce(Vector2.up * jumpMultiplier);
    }
}
