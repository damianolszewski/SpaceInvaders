using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;

    [Header("Shooting Settings")]
    public ProjectileManager projectileFactory;
    public Transform shootPoint;
    public float fireRate;
    private float nextFireTime = 0f;

    [Header("Lives Settings")]
    public int lives;

    private Rigidbody2D rb;
    private float movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PlayerData data = Config.Instance.PlayerConfig;

        moveSpeed = data.moveSpeed;
        fireRate = data.fireRate;
        lives = data.lives;
    }

    private void Update()
    {
        HandleInput();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        movement = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        transform.position += new Vector3(movement, 0, 0) * moveSpeed * Time.fixedDeltaTime;
        if(transform.position.x > Config.RIGHT_BOUNDARY)
        {
            transform.position = new Vector3(Config.RIGHT_BOUNDARY, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < Config.LEFT_BOUNDARY)
        {
            transform.position = new Vector3(Config.LEFT_BOUNDARY, transform.position.y, transform.position.z);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Projectile projectile = projectileFactory.GetProjectile();
        projectile.owner = ProjectileOwner.Player;
        projectile.transform.position = shootPoint.position;
        projectile.gameObject.SetActive(true);
    }

    public void Hit()
    {
        lives -= 1;
        GameEvents.OnPlayerHit.Invoke();
        if (lives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameEvents.OnGameStateChanged.Invoke(GameState.Lose);
    }
}
