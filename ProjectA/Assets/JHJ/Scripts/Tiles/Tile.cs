using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] private TileData data;

    public TileData Data => data;

    private void Awake()
    {
        ApplyVisual();
    }

    public void SetData(TileData newData)
    {
        data = newData;
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (data == null || data.sprite == null) return;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }
}
