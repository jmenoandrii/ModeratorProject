using TMPro;
using UnityEngine;

public class MyMoneyUI : MonoBehaviour
{
    [SerializeField] TMP_Text _amount;

    private void Awake()
    {
        GlobalEventManager.OnChangeCryptoWallet += SetAmount;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnChangeCryptoWallet -= SetAmount;
    }

    private void OnEnable()
    {
        SetAmount(GameStats.Instance.CryptoWalletBalance);
    }

    private void SetAmount(int amount)
    {
        _amount.SetText(amount.ToString());
    }
}
