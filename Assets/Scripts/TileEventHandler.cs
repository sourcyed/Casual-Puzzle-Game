using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileEventHandler : MonoBehaviour
{
    public Tile tile;
    public int row;
    public int column;

    public void SetTile(Tile tile)
    {
        this.tile = tile;
        row = tile.row;
        column = tile.column;
    }

    public void OnClick()
    {
        GameManager.instance.ClickTile(tile);
    }
}