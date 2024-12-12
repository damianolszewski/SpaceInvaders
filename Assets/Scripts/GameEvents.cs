using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent<int> OnScoreChanged = new UnityEvent<int>();
    public static UnityEvent<int> OnHighScoreChanged = new UnityEvent<int>();
    public static UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();
    public static UnityEvent<Vector3> OnProjectileHit = new UnityEvent<Vector3>();
    public static UnityEvent OnPlayerHit = new UnityEvent();
    public static UnityEvent<EnemyLogic> OnEnemyKilled = new UnityEvent<EnemyLogic>();
    public static UnityEvent OnEnemiesMove = new UnityEvent();
    public static UnityEvent OnWaveKilled = new UnityEvent();
}
