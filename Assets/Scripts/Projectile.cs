using UnityEngine;

public enum ProjectileOwner
{
    Player,
    Enemy
}

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;

    public ProjectileOwner owner;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Invoke(nameof(Disable), lifeTime);
        rb.linearVelocity = new Vector2(0, owner.Equals(ProjectileOwner.Player) ? speed : -speed);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnHit(Vector3 explosionPosition)
    {
        GameEvents.OnProjectileHit.Invoke(explosionPosition);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner.Equals(ProjectileOwner.Player) && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyLogic>().Hit();
            OnHit(collision.transform.position);
        }
        else if (owner.Equals(ProjectileOwner.Enemy) && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Hit();
            OnHit(collision.transform.position);
        }
        else if (collision.CompareTag("Projectile"))
        {
            OnHit(transform.position);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            OnHit(transform.position);
        }
    }
}
