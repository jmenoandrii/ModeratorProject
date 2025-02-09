using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuyAntivirusButton : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private GameObject _boughtText;
    private Button _button;

    private void Awake()
    {
        GlobalEventManager.OnChangeCryptoWallet += ChangeButtonState;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnChangeCryptoWallet -= ChangeButtonState;
    }

    private void Start()
    {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(Buy);
        ChangeButtonState(0);
    }

    private void ChangeButtonState(int amount)
    {
        if (Antivirus.instance.IsOn)
        {
            _boughtText.SetActive(true);
            return;
        }
        _boughtText.SetActive(false);

        _button.interactable = GameStats.Instance.CryptoWalletBalance >= _price;
    }

    private void Buy()
    {
        Antivirus.instance.Unlock();
        _button.interactable = false;
        GameStats.Instance.WithdrawFromWallet(_price);
    }
}
