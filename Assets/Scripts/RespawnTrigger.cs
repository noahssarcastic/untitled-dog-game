using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.gameObject.GetComponent<Respawn>().respawn();
    }
}
