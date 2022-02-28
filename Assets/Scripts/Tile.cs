using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Tile
{
    public int[] tilesRequiredPerStage;
    public int row;
    public int column;
    public TileColor color;
    public int stage = 0;

    public Sprite[] sprites;
    public GameObject go;

    public Action<int, int> OnRemove;

    public Tile(int row, int column, TileColor color, int[] tilesRequiredPerStage)
    {
        this.row = row;
        this.column = column;
        this.color = color;
        this.tilesRequiredPerStage = tilesRequiredPerStage;
    }

    public void SetSprites(Sprite[] sprites)
    {
        this.sprites = sprites;
        SetSprite();
    }

    public void SetSprite()
    {
        go.GetComponent<Image>().sprite = sprites[stage];
    }

    public void SetStage(int adjacentTileCount)
    {
        stage = 0;
        for (int i = tilesRequiredPerStage.Length - 1; i >= 0; i--)
        {
            if (adjacentTileCount > tilesRequiredPerStage[i])
            {
                stage = i + 1;
                break;
            }
        }
        SetSprite();
    }

    public void Remove()
    {
        if (OnRemove != null)
        {
            OnRemove(row, column);
        }
    }
}

