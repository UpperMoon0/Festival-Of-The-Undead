using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    private float speed = 5f;
    private Rigidbody2D rb;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isOwned) enabled = false;
    }

    void Update()
    {
        if (!isOwned) return;

        // Player movement
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y).normalized;
        rb.velocity = dir * speed;
    }
}
