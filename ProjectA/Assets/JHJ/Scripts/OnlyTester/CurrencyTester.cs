using UnityEngine;

public class CurrencyTester : MonoBehaviour
{
    [SerializeField] private int amount = 100;

    public void AddCoin()
    {
        if (TempPlayer.Instance != null)
            TempPlayer.Instance.AddCurrency(amount);
    }

    public void RemoveCoin()
    {
        if (TempPlayer.Instance != null)
            TempPlayer.Instance.AddCurrency(-amount);
    }
}