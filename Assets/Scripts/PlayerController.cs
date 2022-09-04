using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, .3f)][SerializeField] private float smoothTime = .05f;
    [SerializeField] private float maxVelocity = 10f;

    private Rigidbody2D playerRigidbody;
    private float horizontalInput;
    private Vector2 smoothDampRef = Vector2.zero; // required for SmoothDamp to work


    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateInput();
        Move();
    }

    private void UpdateInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        Vector2 currentVelocity = playerRigidbody.velocity;
        Vector2 targetVelocity = new Vector2(horizontalInput * maxVelocity, currentVelocity.y);
        playerRigidbody.velocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref smoothDampRef, smoothTime);
    }
}
