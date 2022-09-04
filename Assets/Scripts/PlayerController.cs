using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;

    private Vector2 velocity = Vector3.zero;
    private Rigidbody2D playerRigidbody;
    private Vector2 movement;


    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void UpdateInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), 0f);
        movement = input;
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector2(movement.x * 10f, playerRigidbody.velocity.y);
        playerRigidbody.velocity = Vector2.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
    }
}
