using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteData
{
    public List<Sprite> sprites;
}

[RequireComponent(typeof(EnemyController))]
public class EnemyFactory : MonoBehaviour
{
    private EnemyController enemyController;

    public List<SpriteData> enemySprites;

    public GameObject enemyPrefab;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public EnemyLogic SpawnEnemy(string enemyType, Vector2 position)
    {
        EnemyData enemyData = Config.Instance.GetEnemyData(enemyType);
        if (enemyData == null)
        {
            return null;
        }

        GameObject enemyObject = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
        EnemyLogic enemy = enemyObject.GetComponent<EnemyLogic>();
        enemy.Initialize(enemyData);
        EnemyVisual enemyVisual = enemyObject.GetComponent<EnemyVisual>();
        enemyVisual.SetSpriteData(enemySprites[enemyData.spriteIndex]);
        enemyVisual.SetColor(enemyData.color);

        enemyController.RegisterEnemy(enemy);

        return enemy;
    }
}
