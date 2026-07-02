using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public static TempPlayer Instance { get; private set; }

    [SerializeField] private int currency = 300;
    public int Currency => currency;

    public event System.Action<int> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        OnCurrencyChanged?.Invoke(currency);
    }

    public bool SpendCurrency(int amount)
    {
        if (currency < amount) return false;
        currency -= amount;
        OnCurrencyChanged?.Invoke(currency);
        return true;
    }
}