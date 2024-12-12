using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public int lives;
    public int spriteIndex;
    public string enemyType;
    public int scoreValue;
    public float[] color;
}

[System.Serializable]
public class EnemyDataList
{
    public List<EnemyData> enemies;
}