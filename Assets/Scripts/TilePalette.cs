using UnityEngine;

[CreateAssetMenu(fileName = "TilePalette", menuName = "TilePalette", order = 1)]
public class TilePalette : ScriptableObject
{
    public TileColor color;
    public Sprite[] tileSprites;
}