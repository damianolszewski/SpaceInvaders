using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public ProjectileManager projectileManager;

    public List<EnemyLogic> enemies = new List<EnemyLogic>();

    public float enemyKillSpeedUp;
    public float enemyKillDistanceUp;
    public float enemyKillPauseTime;
    public float moveDistance;
    public float moveSpeedTime;
    public float minMoveSpeedTime;
    public float maxMoveDistance;
    private float moveSpeedTimer = 0;
    public float moveDownAmount;

    private Vector2 direction = Vector2.right;

    private bool areEnemiesPaused = false;

    private void Start()
    {
        moveDistance = Config.Instance.GameConfig.enemiesMoveDistance;
        moveSpeedTime = Config.Instance.GameConfig.enemiesStartMoveSpeed;
        minMoveSpeedTime = Config.Instance.GameConfig.enemiesMinMoveSpeed;
        maxMoveDistance = Config.Instance.GameConfig.enemiesMaxMoveDistance;

        enemyKillSpeedUp = Config.Instance.GameConfig.enemyKillSpeedUp;
        enemyKillDistanceUp = Config.Instance.GameConfig.enemyKillDistanceUp;
        enemyKillPauseTime = Config.Instance.GameConfig.enemyKillPauseTime;
        moveDownAmount = Config.Instance.GameConfig.enemiesDownMoveAmount;

        GameEvents.OnEnemyKilled.AddListener(OnEnemyKilled);
        GameEvents.OnWaveKilled.AddListener(OnWaveKilled);

        InvokeRepeating(nameof(ShootFromRandomEnemy), 1, 1);
    }

    private void OnEnemyKilled(EnemyLogic enemy)
    {
        if(moveSpeedTime > minMoveSpeedTime)
        {
            moveSpeedTime -= enemyKillSpeedUp;
        }
        if(moveSpeedTime <= minMoveSpeedTime)
        {
            moveSpeedTime = minMoveSpeedTime;
        }

        if(moveDistance <= maxMoveDistance)
        {
            moveDistance += enemyKillDistanceUp;
        }
        if (moveDistance > maxMoveDistance)
        {
            moveDistance = maxMoveDistance;
        }

        UnregisterEnemy(enemy);
    }

    private void OnWaveKilled()
    {
        moveSpeedTime = Config.Instance.GameConfig.enemiesStartMoveSpeed;
    }

    private void Update()
    {
        MoveGroup();
    }

    private void MoveGroup()
    {
        if (areEnemiesPaused)
        {
            return;
        }

        moveSpeedTimer += Time.deltaTime;

        if (moveSpeedTimer < moveSpeedTime)
        {
            return;
        }

        moveSpeedTimer = 0;
        Vector2 lastPosition = transform.position;
        transform.Translate(moveDistance * direction);

        GameEvents.OnEnemiesMove.Invoke();

        foreach (EnemyLogic enemy in enemies)
        {
            if (enemy.transform.position.x >= Config.RIGHT_BOUNDARY && direction == Vector2.right)
            {
                direction = Vector2.left;
                MoveGroupDown(lastPosition.x);
                return;
            }
            else if (enemy.transform.position.x <= Config.LEFT_BOUNDARY && direction == Vector2.left)
            {
                direction = Vector2.right;
                MoveGroupDown(lastPosition.x);
                return;
            }
        }
    }

    private void MoveGroupDown(float xBoundary)
    {
        transform.position = new Vector2(xBoundary, transform.position.y - moveDownAmount);
    }

    private void ShootFromRandomEnemy()
    {
        if (enemies.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, enemies.Count);
        Projectile projectile = projectileManager.GetProjectile();
        enemies[randomIndex].Shoot(projectile);
    }

    public void RegisterEnemy(EnemyLogic enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyLogic enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            GameEvents.OnWaveKilled.Invoke();
        } else
        {
            PauseEnemies();
        }
    }

    public void PauseEnemies()
    {
        areEnemiesPaused = true;
        Invoke(nameof(ResumeEnemies), enemyKillPauseTime);
    }

    private void ResumeEnemies()
    {
        areEnemiesPaused = false;
    }
}
