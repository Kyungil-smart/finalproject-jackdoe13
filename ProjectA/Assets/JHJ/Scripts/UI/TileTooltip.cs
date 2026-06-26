using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TileTooltip : MonoBehaviour
{
    [Header("참조")]
    [Tooltip("타일이 보이는 카메라. 비워두면 Camera.main 사용")]
    [SerializeField] private Camera worldCamera;

    [Tooltip("팝업 패널의 RectTransform (Canvas 안의 UI). 켜고/끄며 사용")]
    [SerializeField] private RectTransform tooltipPanel;

    [Tooltip("팝업 제목 텍스트")]
    [SerializeField] private TMP_Text titleText;

    [Tooltip("팝업 설명 텍스트")]
    [SerializeField] private TMP_Text descriptionText;

    [Header("설정")]
    [Tooltip("타일이 속한 레이어. 기본값은 모든 레이어")]
    [SerializeField] private LayerMask tileLayer = ~0;

    [Tooltip("커서 기준 팝업 위치 보정 (우측 하단: x+ / y-)")]
    [SerializeField] private Vector2 cursorOffset = new Vector2(16f, -16f);

    private Tile currentTile;

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
        Hide();
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = worldCamera.ScreenToWorldPoint(screenPos);

        Collider2D hit = Physics2D.OverlapPoint(worldPos, tileLayer);
        Tile tile = hit != null ? hit.GetComponentInParent<Tile>() : null;

        if (tile != null && tile.Data != null)
        {
            if (tile != currentTile)
            {
                currentTile = tile;
                Show(tile.Data);
            }
            FollowCursor(screenPos);
        }
        else if (currentTile != null)
        {
            Hide();
        }
    }

    private void Show(TileData data)
    {
        if (titleText != null) titleText.text = data.tileName;
        if (descriptionText != null) descriptionText.text = data.description;
        if (tooltipPanel != null) tooltipPanel.gameObject.SetActive(true);
    }

    private void Hide()
    {
        currentTile = null;
        if (tooltipPanel != null) tooltipPanel.gameObject.SetActive(false);
    }

    private void FollowCursor(Vector2 screenPos)
    {
        if (tooltipPanel != null) tooltipPanel.position = screenPos + cursorOffset;
    }
}
