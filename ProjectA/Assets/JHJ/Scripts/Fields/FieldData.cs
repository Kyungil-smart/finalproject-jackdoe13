using UnityEngine;

[CreateAssetMenu(fileName = "FieldData", menuName = "Board/Field Data")]
public class FieldData : ScriptableObject
{
    [Tooltip("필드 이름")]
    public string fieldName;

    [Tooltip("필드 스프라이트")]
    public Sprite sprite;

    [Tooltip("설명문")]
    [TextArea(3, 6)]
    public string description;
}