using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    [SerializeField] private int _cryptoWalletInitialBalance = 0;
    [SerializeField] private int _cryptoWalletBalance = 0;
    [SerializeField] private int _victimsCount = 0;

    public int CryptoWalletBalance { get => _cryptoWalletBalance; }
    public int CryptoWalletIntitalBalance { get => _cryptoWalletInitialBalance; }
    public int CryptoWalletDifference { get => _cryptoWalletBalance - _cryptoWalletInitialBalance; }
    public int VictimsCount { get => _victimsCount; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _cryptoWalletInitialBalance = PlayerPrefs.GetInt("PlayerWallet");
        AddToWallet(_cryptoWalletInitialBalance);

        GlobalEventManager.OnSendImpact += ProcessImpact;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnSendImpact -= ProcessImpact;
    }

    private void ProcessImpact(AdminPostLoader.Impact impact)
    {
        AddToWallet(impact.profit);
        IncrementVictimsCount(impact.victims);
    }

    public void AddToWallet(int amount)
    {
        _cryptoWalletBalance += amount;
        PlayerPrefs.SetInt("PlayerWallet", _cryptoWalletBalance);
        GlobalEventManager.CallOnChangeCryptoWallet(_cryptoWalletBalance);
    }

    public void WithdrawFromWallet(int amount)
    {
        AddToWallet(-amount);
    }

    public void IncrementVictimsCount(int amount)
    {
        _victimsCount += amount;
    }
}
