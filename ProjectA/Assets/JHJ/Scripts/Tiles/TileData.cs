using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Board/Tile Data")]
public class TileData : ScriptableObject
{
    [Tooltip("타일 이름")]
    public string tileName;

    [Tooltip("타일 스프라이트")]
    public Sprite sprite;

    [Tooltip("설명문")]
    [TextArea(3, 6)]
    public string description;
}
