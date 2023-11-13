using Mirror;
using UnityEngine;

public abstract class Projectile : NetworkBehaviour
{
    [SerializeField] public abstract int ID { get; }
}
