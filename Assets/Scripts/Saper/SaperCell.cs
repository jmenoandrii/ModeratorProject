using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaperCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _number;
    [SerializeField] private Image _img;
    private bool _isFlag = false;
    private Button _button;
    private (int, int) _coord;  // (x, y)

    public bool IsOpened { get => !_button.interactable; }

    private void Start()
    {
        _button = GetComponent<Button>();

        _img.sprite = Saper.FlageSprite;
    }

    public void SetCoord(int x, int y)
    {
        _coord = (x, y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Saper.instance.IsEndGame) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Open();
            Saper.instance.PlayChooseCellAudio();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleFlag();
        }
    }

    private void Open()
    {
        int value = Saper.instance.GetValue(_coord.Item1, _coord.Item2);

        if (value <= -1000)
        {
            GlobalEventManager.CallOnInitSaperField(_coord.Item1, _coord.Item2);
            value = 0;
        }

        if (value == 0)
            GlobalEventManager.CallOnOpenEnptySaperCell(_coord.Item1, _coord.Item2);
        else
            Open(value);
    }

    private void Open(int value)
    {
        _button.interactable = false;
        Saper.instance.IncreaseOpenedCell();

        if (value < 0)
        {
            _img.sprite = Saper.BombSprite;
            _img.gameObject.SetActive(true);
            GlobalEventManager.CallOnSaperGameOver();
            return;
        }
        else if (value > 0)
        {
            _number.SetText(value.ToString());
            _number.gameObject.SetActive(true);
        }
        _img.gameObject.SetActive(false);
    }

    public void OpenAsEmpty(int value)
    {
        _button.interactable = false;
        Saper.instance.IncreaseOpenedCell();

        if (value > 0)
        {
            _number.SetText(value.ToString());
            _number.gameObject.SetActive(true);
        }
    }

    private void ToggleFlag()
    {
        if ((IsOpened & !_isFlag) || (_isFlag && !SaperScore.instance.IsAbleToIncrease) ||
            (!_isFlag && !SaperScore.instance.IsAbleToDecrease))
            return;

        _isFlag = !_isFlag;

        if (_isFlag)
            Saper.instance.PlayFlagAudio();
     
        _img.gameObject.SetActive(_isFlag);

        SaperScore.instance.ChangeScore(_isFlag);
    }
}
