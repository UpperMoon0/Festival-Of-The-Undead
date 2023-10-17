using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    // NetworkTransform has authority over the player's position
    private float speed = 5f;
    private Rigidbody2D rb;
    void Start()
    {
        if (isLocalPlayer)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // Player movement
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(x, y).normalized;
            rb.velocity = dir * speed;
        }
    }
}
