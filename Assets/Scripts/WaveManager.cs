using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Rendering.STP;

[RequireComponent(typeof(EnemyFactory))]
public class WaveManager : MonoBehaviour
{
    private EnemyFactory enemyFactory;

    private WaveDataList waveDataList;

    private int maxWaves;
    private int currentWaveIndex = 0;

    private int enemiesPerRow;

    private float xSpacing;
    private float ySpacing;

    private void Start()
    {
        enemyFactory = GetComponent<EnemyFactory>();

        waveDataList = Config.Instance.WaveData;
        maxWaves = waveDataList.waves.Count;

        enemiesPerRow = Config.Instance.GameConfig.enemiesPerRow;

        xSpacing = Config.Instance.GameConfig.enemyXSpacing;
        ySpacing = Config.Instance.GameConfig.enemyYSpacing;

        GameEvents.OnGameStateChanged.AddListener(OnGameStateChanged);
        GameEvents.OnWaveKilled.AddListener(OnWaveKilled);
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Start)
        {
            SpawnNextWave();
            GameEvents.OnGameStateChanged.Invoke(GameState.Gameplay);
        }
    }

    private void OnWaveKilled()
    {
        if (currentWaveIndex < maxWaves)
        {
            SpawnNextWave();
        }
        else
        {
            GameEvents.OnGameStateChanged.Invoke(GameState.Win);
        }
    }

    private void SpawnNextWave()
    {
        WaveData enemies = waveDataList.waves[currentWaveIndex];
        SpawnWave(enemies);
        currentWaveIndex++;
    }

    private void SpawnWave(WaveData enemies)
    {
        for (int i = 0; i < enemies.enemies.Count; i++)
        {
            for (int j = 0; j < enemiesPerRow; j++)
            {
                Vector2 spawnPosition = CalculateSpawnPosition(j, i);
                enemyFactory.SpawnEnemy(enemies.enemies [i], spawnPosition);
            }
        }
    }

    private Vector2 CalculateSpawnPosition(int index, int row)
    {
        float xOffset = (enemiesPerRow - 1) * xSpacing / 2f;
        float xRowPosition = (index % enemiesPerRow) * xSpacing;
        float xWorldPosition = xRowPosition - xOffset;

        float yOffset = (enemiesPerRow - 1) * ySpacing / 2f;
        float yRowPosition = row * ySpacing;
        float yWorldPosition = yOffset - yRowPosition - 1;

        return new Vector2(xWorldPosition, yWorldPosition);
    }
}
