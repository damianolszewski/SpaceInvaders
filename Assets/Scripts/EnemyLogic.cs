using UnityEngine;

[RequireComponent(typeof(EnemyVisual))]
public class EnemyLogic : MonoBehaviour
{
    private EnemyVisual enemyVisual;

    private int maxLives;
    private int lives;
    private int scoreValue;

    private void Awake()
    {
        enemyVisual = GetComponent<EnemyVisual>();
    }

    public void Initialize(EnemyData data)
    {
        maxLives = data.lives;
        lives = data.lives;
        scoreValue = data.scoreValue;
    }

    public void Shoot(Projectile projectile)
    {
        projectile.owner = ProjectileOwner.Enemy;
        projectile.transform.position = transform.position;
        projectile.gameObject.SetActive(true);
    }

    public void Hit()
    {
        lives -= 1;
        enemyVisual.RefreshVisual(lives, maxLives);
        if (lives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameEvents.OnEnemyKilled.Invoke(this);
        gameObject.SetActive(false);
    }

    public int GetScoreValue()
    {
        return scoreValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Finish"))
        {
            GameEvents.OnGameStateChanged.Invoke(GameState.Lose);
        }
    }
}
