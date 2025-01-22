using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _endGameCanvas;

    [SerializeField] private TMP_Text _endingTitle;
    [SerializeField] private TMP_Text _endingDesc;
    [SerializeField] private TMP_Text _endingCryptoBalance;
    [SerializeField] private TMP_Text _endingVictimsCount;

    private void Awake()
    {
        GlobalEventManager.OnEndGame += DisplayEnding;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnEndGame -= DisplayEnding;
    }

    private void DisplayEnding(EndingSummary ending)
    {
        _gameCanvas.SetActive(false);
        _endGameCanvas.SetActive(true);

        _endingTitle.SetText(ending.Name);
        _endingDesc.SetText(ending.Description);
        _endingCryptoBalance.SetText(ending.CryptoWalletBalance.ToString());
        _endingVictimsCount.SetText(ending.VictimsCount.ToString());
    }
}
