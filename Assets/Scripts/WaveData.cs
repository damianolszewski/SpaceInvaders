using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public List<string> enemies;
}

[System.Serializable]
public class WaveDataList
{
    public List<WaveData> waves;
}