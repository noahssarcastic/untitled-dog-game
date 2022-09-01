using UnityEngine;

public class Respawn : MonoBehaviour
{
    public delegate void RespawnHandler();
    public RespawnHandler respawn;

    void Start()
    {
        if (respawn == null)
            respawn = DefaultRespawn;
    }

    private void DefaultRespawn()
    {
        Debug.Log("Respawn behavior not set.");
    }
}
