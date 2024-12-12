using UnityEngine;
using System.Collections.Generic;

public class Config
{
    private static Config instance;
    public static Config Instance { get { instance ??= new Config(); return instance; } }

    public Dictionary<string, EnemyData> EnemyConfigs { get; private set; } = new Dictionary<string, EnemyData>();
    public GameConfig GameConfig { get; private set; }
    public PlayerData PlayerConfig { get; private set; }
    public WaveDataList WaveData { get; private set; }

    public static readonly float LEFT_BOUNDARY = -8f;
    public static readonly float RIGHT_BOUNDARY = 8f;

    private Config()
    {
        LoadGameConfigs();
        LoadEnemyConfigs();
        LoadPlayerData();
        LoadWaveData();
    }

    private void LoadGameConfigs()
    {
        TextAsset configText = Resources.Load<TextAsset>("gameConfig");
        GameConfig = JsonUtility.FromJson<GameConfig>(configText.text);
    }

    private void LoadEnemyConfigs()
    {
        TextAsset configText = Resources.Load<TextAsset>("enemyConfig");
        EnemyDataList enemyDataList = JsonUtility.FromJson<EnemyDataList>(configText.text);

        foreach (EnemyData data in enemyDataList.enemies)
        {
            EnemyConfigs.Add(data.enemyType, data);
        }
    }

    private void LoadPlayerData()
    {
        TextAsset configText = Resources.Load<TextAsset>("playerConfig");
        PlayerConfig = JsonUtility.FromJson<PlayerData>(configText.text);
    }

    private void LoadWaveData()
    {
        TextAsset configText = Resources.Load<TextAsset>("waveConfig");
        WaveData = JsonUtility.FromJson<WaveDataList>(configText.text);
    }

    public EnemyData GetEnemyData(string enemyType)
    {
        if (EnemyConfigs.ContainsKey(enemyType))
            return EnemyConfigs[enemyType];
        else
            return null;
    }
}
