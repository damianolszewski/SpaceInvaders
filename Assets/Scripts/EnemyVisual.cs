using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteChanger))]
public class EnemyVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteChanger spriteChanger;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteChanger = GetComponent<SpriteChanger>();

        GameEvents.OnEnemiesMove.AddListener(() => spriteChanger.ChangeToNextSprite());
    }

    public void SetSpriteData(SpriteData spriteData)
    {
        spriteChanger.SetSprites(spriteData.sprites);
        spriteRenderer.sprite = spriteData.sprites[0];
    }

    public void SetColor(float[] color)
    {
        spriteRenderer.color = new Color(color[0], color[1], color[2]);
    }

    public void RefreshVisual(int lives, int maxLives)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (float) lives / maxLives);
    }
}
