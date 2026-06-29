using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class FieldTooltip : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Camera worldCamera;
    [SerializeField] private RectTransform tooltipPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("설정")]
    [SerializeField] private LayerMask fieldLayer = ~0;
    [SerializeField] private Vector2 cursorOffset = new Vector2(16f, -16f);

    private Field currentField;

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
        Hide();
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        // 마우스가 UI 위면 필드 감지 안 함.
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (currentField != null) Hide();
            return;
        }

        Vector2 screenPos = Mouse.current.position.ReadValue();

        // 카메라에서 마우스 방향으로 광선을 쏴 2D 콜라이더를 찾음 = 레이캐스트
        Ray ray = worldCamera.ScreenPointToRay(screenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, fieldLayer);
        Field field = hit.collider != null ? hit.collider.GetComponentInParent<Field>() : null;

        if (field != null && field.Data != null)
        {
            if (field != currentField)
            {
                currentField = field;
                Show(field.Data);
            }
            FollowCursor(screenPos);
        }
        else if (currentField != null)
        {
            Hide();
        }
    }

    private void Show(FieldData data)
    {
        if (titleText != null) titleText.text = data.fieldName;
        if (descriptionText != null) descriptionText.text = data.description;
        if (tooltipPanel != null) tooltipPanel.gameObject.SetActive(true);
    }

    private void Hide()
    {
        currentField = null;
        if (tooltipPanel != null) tooltipPanel.gameObject.SetActive(false);
    }

    private void FollowCursor(Vector2 screenPos)
    {
        if (tooltipPanel != null) tooltipPanel.position = screenPos + cursorOffset;
    }
}