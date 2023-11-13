using Mirror;
using UnityEngine;

public class GuardTowerProj : Projectile
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector2 dir;
    [SerializeField] private float distance;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float MaxDistance
    {
        get { return maxDistance; }
        set { maxDistance = value; }
    }
    public Vector2 Dir
    {
        get { return dir; }
        set { dir = value; }
    }

    public override int ID => 0;

    void Start()
    {
        // Calculate the angle of rotation based on the direction vector
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Since the sprite is facing upwards, subtract 90 degrees from the angle
        angle -= 90;

        // Set the rotation of the sprite
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        if (isServer)
        {
            // Move projectile in world space
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
            distance += speed * Time.deltaTime;

            // Destroy projectile if it has traveled too far
            if (distance >= maxDistance)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
