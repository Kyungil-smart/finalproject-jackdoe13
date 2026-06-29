using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Field : MonoBehaviour
{
    [SerializeField] private FieldData data;

    public FieldData Data => data;

    private void Awake()
    {
        ApplyVisual();
    }

    public void SetData(FieldData newData)
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