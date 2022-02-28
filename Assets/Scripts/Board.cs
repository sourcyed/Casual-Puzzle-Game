using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board
{
    Tile[,] tiles;
    TileColor[] colors;
    int[] tilesRequiredPerStage;
    public System.Action OnShuffle;

    public Board(int rowCount, int columnCount, TileColor[] colors, int[] tilesRequiredPerStage)
    {
        this.colors = colors;
        this.tilesRequiredPerStage = tilesRequiredPerStage;
        tiles = new Tile[rowCount, columnCount];
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                CreateTile(i, j);
            }
        }
    }

    Tile CreateTile(int row, int column)
    {
        TileColor color = colors[Random.Range(0, colors.Length)];
        Tile tile = new Tile(row, column, color, tilesRequiredPerStage);
        tile.OnRemove += OnRemoveTile;
        tiles[row, column] = tile;
        return tile;
    }

    public Tile[,] GetTiles()
    {
        Debug.Log(tiles.Length);
        return tiles;
    }

    public List<Tile> GetAdjacentTiles(Tile centerTile)
    {
        int centerRow = centerTile.row;
        int centerColumn = centerTile.column;

        Tile leftTile = null;
        Tile rightTile = null;
        Tile upTile = null;
        Tile downTile = null;

        if (centerColumn - 1 >= 0)
            leftTile = tiles[centerRow, centerColumn - 1];
        if (centerColumn + 1 < tiles.GetLength(1))
            rightTile = tiles[centerRow, centerColumn + 1];
        if (centerRow + 1 < tiles.GetLength(0))
            upTile = tiles[centerRow + 1, centerColumn];
        if (centerRow - 1 >= 0)
            downTile = tiles[centerRow - 1, centerColumn];

        List<Tile> adjacentTiles = new List<Tile>();
        List<Tile> temp = new List<Tile>() { leftTile, rightTile, upTile, downTile };

        foreach (Tile adjacentTile in temp)
        {
            if (adjacentTile != null && adjacentTile.color == centerTile.color)
            {
                adjacentTiles.Add(adjacentTile);
            }
        }
        return adjacentTiles;
    }

    public List<Tile> GetAllAdjacentTiles(Tile centerTile)
    {
        List<Tile> adjacentTiles;
        adjacentTiles = new List<Tile>();
        List<Tile> temp = AdjacentTileRecursion(centerTile, ref adjacentTiles);
        return temp.OrderByDescending(x => x.row).ToList();
    }

    public List<Tile> AdjacentTileRecursion(Tile centerTile, ref List<Tile> adjacentTiles)
    {
        List<Tile> tileList = new List<Tile>();

        foreach (Tile tile in GetAdjacentTiles(centerTile))
        {
            if (!adjacentTiles.Contains(tile))
            {
                tileList.Add(tile);
                adjacentTiles.Add(tile);
                foreach (Tile adjacentTile in AdjacentTileRecursion(tile, ref adjacentTiles))
                {
                    tileList.Add(adjacentTile);
                }
            }
        }

        return tileList;
    }

    public bool SetTileStages()
    {
        List<Tile> tileList = new List<Tile>();
        int adjacentTileGroupCount = 0;

        for (int i = 0; i < GetTiles().GetLength(0); i++)
        {
            for (int j = 0; j < GetTiles().GetLength(1); j++)
            {
                tileList.Add(GetTiles()[i, j]);
            }
        }
        int failsafe = 0;
        while (tileList.Count > 0)
        {
            List<Tile> adjacentTiles = GetAllAdjacentTiles(tileList[0]);
            if (adjacentTiles.Count > 0)
            {
                adjacentTileGroupCount++;
                tileList[0].SetStage(adjacentTiles.Count);
                tileList.RemoveAt(0);
                foreach (Tile adjacentTile in adjacentTiles)
                {
                    Debug.Log(adjacentTiles.Count);
                    adjacentTile.SetStage(adjacentTiles.Count);
                    tileList.Remove(adjacentTile);
                }
            }
            else
            {
                tileList.RemoveAt(0);
            }

            failsafe++;
            if (failsafe > 10000)
            {
                break;
            }
        }

        if (adjacentTileGroupCount == 0)
        {
            ShuffleBoard();
        }

        return true;
    }

    void OnRemoveTile(int row, int column)
    {
        tiles[row, column] = null;
        for (int i = row; i < tiles.GetLength(0); i++)
        {
            if (i < tiles.GetLength(0) - 1)
            {
                SetTilePosition(tiles[i + 1, column], i, column);
            }
            else
            {
                CreateTile(i, column);
            }
        }
    }

    void SetTilePosition(Tile tile, int row, int column)
    {
        tiles[row, column] = tile;
        tile.row = row;
        tile.column = column;
    }

    public void ShuffleBoard()
    {
        Debug.Log("Shuffle");

        List<Tile> availableTiles = new List<Tile>();

        for (int i = 0; i < GetTiles().GetLength(0); i++)
        {
            for (int j = 0; j < GetTiles().GetLength(1); j++)
            {
                availableTiles.Add(tiles[i, j]);
            }
        }

        for (int i = 0; i < GetTiles().GetLength(0); i++)
        {
            for (int j = 0; j < GetTiles().GetLength(1); j++)
            {
                Tile tileToUse = availableTiles[Random.Range(0, availableTiles.Count)];
                SetTilePosition(tileToUse, i, j);
                availableTiles.Remove(tileToUse);
            }
        }

        if (SetTileStages() == true)
        {
            if (OnShuffle != null)
            {
                OnShuffle();
            }
        }
    }
}