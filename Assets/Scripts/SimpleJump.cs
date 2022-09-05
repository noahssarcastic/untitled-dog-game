using UnityEngine;

public class SimpleJump : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 1f;

    private Rigidbody2D playerRigidbody;
    private float initialVelocity;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        CalculateInitialVelocity();
    }


    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, initialVelocity);
        }
    }

    void OnValidate()
    {
        CalculateInitialVelocity();
    }

    private void CalculateInitialVelocity()
    {
        if (playerRigidbody == null) return;

        // Readjust p factor to be able to jump onto platforms jumpHeight tall.
        float deltaPosition = jumpHeight * 10 + 1;
        initialVelocity = Mathf.Sqrt(2 * playerRigidbody.gravityScale * deltaPosition);
    }
}
