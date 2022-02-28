using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    //Settings
    public int rowCount;
    public int columnCount;
    public int colorCount;
    public int[] tilesRequiredPerStage = new int[3];
    public Vector2 tileSize;

    // Connection
    public GameObject tilePrefab;
    public Transform tileParent;
    public TilePalette[] tilePalettes;

    // State Variables
    public static GameManager instance;
    Board board;
    List<GameObject> tileGOs;

    // Start is called before the first frame update
    void Start()
    {
        InitConnections();
        InitState();
    }
    void InitConnections(){
    }
    void InitState(){
        instance = this;
    }

    public void CreateBoard()
    {
        TileColor[] colors = GetColors();
        board = new Board(rowCount, columnCount, colors, tilesRequiredPerStage);
        board.OnShuffle += MaterializeBoard;
        MaterializeBoard();
    }

    private TileColor[] GetColors()
    {
        TileColor[] colors = new TileColor[colorCount];
        List<TileColor> temp = System.Enum.GetValues(typeof(TileColor)).Cast<TileColor>().ToList();
        for (int i = 0; i < colorCount; i++)
        {
            int rnd = Random.Range(0, temp.Count);
            colors[i] = temp[rnd];
            temp.RemoveAt(rnd);
        }

        return colors;
    }

    public void MaterializeBoard()
    {
        if (tileGOs != null)
        {
            foreach (GameObject go in tileGOs)
            {
                Destroy(go);
            }
        }
        tileGOs = new List<GameObject>();
        Tile[,] tiles = board.GetTiles();
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                MaterializeTile(tiles, i, j);
            }
        }
        board.SetTileStages();
    }

    void MaterializeTile(Tile[,] tiles, int i, int j)
    {
        Tile tile = tiles[i, j];
        GameObject tileGO = Instantiate(tilePrefab);
        tileGO.transform.SetParent(tileParent);
        tileGO.transform.localPosition = new Vector2(tileSize.x / 2 + j * tileSize.x, tileSize.y / 2 + i * tileSize.y);
        tile.go = tileGO;
        tile.SetSprites(tilePalettes.Where(x => x.color == tile.color).FirstOrDefault().tileSprites);
        tileGO.GetComponent<TileEventHandler>().SetTile(tile);
        tileGO.name = (i * columnCount + j).ToString();
        tileGOs.Add(tileGO);
    }

    public void ClickTile(Tile tile)
    {
        foreach (Tile adjacentTile in board.GetAllAdjacentTiles(tile))
        {
            adjacentTile.Remove();
            Destroy(adjacentTile.go);
        }
        MaterializeBoard();
    }

    public void ShuffleBoard()
    {
        board.ShuffleBoard();
    }
}

