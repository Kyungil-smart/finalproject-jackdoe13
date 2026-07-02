using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    private void Start()
    {
        if (TempPlayer.Instance == null) return;
        UpdateText(TempPlayer.Instance.Currency);
        TempPlayer.Instance.OnCurrencyChanged += UpdateText;
    }

    private void OnDestroy()
    {
        if (TempPlayer.Instance != null)
            TempPlayer.Instance.OnCurrencyChanged -= UpdateText;
    }

    private void UpdateText(int value)
    {
        if (currencyText != null) currencyText.text = value.ToString();
    }
}