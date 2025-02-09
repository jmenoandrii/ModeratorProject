using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _endGameCanvas;

    [SerializeField] private Image _mainImg;

    [SerializeField] private TMP_Text _endingTitle;
    [SerializeField] private TMP_Text _endingDesc;
    [SerializeField] private TMP_Text _endingTimeline;
    [SerializeField] private TMP_Text _endingCryptoBalance;
    [SerializeField] private TMP_Text _endingCryptoDifference;
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
        _endingTimeline.SetText(ending.Timeline);

        _endingCryptoBalance.SetText(ending.CryptoWalletBalance.ToString());
        
        string differenceText = "";
        if (ending.CryptoWalletDifference > 0)
        {
            differenceText = "(+ "+ending.CryptoWalletDifference.ToString()+")";
        }
        else if (ending.CryptoWalletDifference < 0)
        {
            differenceText = "(- "+Mathf.Abs(ending.CryptoWalletDifference).ToString()+")";
        }

        _endingCryptoDifference.SetText(differenceText);
        _endingVictimsCount.SetText(ending.VictimsCount.ToString());

        _mainImg.sprite = ending.Image;
    }
}
