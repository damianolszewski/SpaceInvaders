using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteChanger : MonoBehaviour
{
    public List<Sprite> sprites;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprites(List<Sprite> sprites)
    {
        this.sprites = sprites;
    }

    public void ChangeToNextSprite()
    {
        if (sprites.Count == 0)
        {
            return;
        }

        int currentIndex = sprites.IndexOf(spriteRenderer.sprite);
        int nextIndex = (currentIndex + 1) % sprites.Count;
        spriteRenderer.sprite = sprites[nextIndex];
    }
}