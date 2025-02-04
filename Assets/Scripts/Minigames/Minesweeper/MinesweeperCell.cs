using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MinesweeperCell : MonoBehaviour, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private TMP_Text _number;
    [SerializeField] private Image _img;
    // ***** init *****
    public int value;    // (use in Minesweeper)
    private int _xCoord;
    private int _yCoord;
    private bool _isFlag = false;
    private Button _button;

    public bool IsOpened { get => !_button.interactable; }

    private void Start()
    {
        _button = GetComponent<Button>();

        _img.sprite = Minesweeper.instance.FlageSprite;

        value = 0;
    }

    public void SetCoord(int x, int y)
    {
        _xCoord = x;
        _yCoord = y;
    }

    public void SetNumber(int number)
    {
        value = number;
        _number.SetText(number.ToString());
    }
    // ***** ***** *****

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Minesweeper.instance.IsEndGame) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Open();
            Minesweeper.instance.PlayChooseCellAudio();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ToggleFlag();
        }
    }

    private void Open()
    {
        // protection [maybe fiexed bug with ghost IncreaseOpenedCell (extra OpenedCell)]
        if (IsOpened) 
            return;

        if (!Minesweeper.instance.IsInitedField)
        {
            Minesweeper.instance.InitField(_xCoord, _yCoord);
            // value = 0;
        }

        // uncheck
        if (_isFlag)
            ToggleFlag();

        if (value == 0)
            Minesweeper.instance.OpenCell(_xCoord, _yCoord);
        else
        {
            // --- Open ---
            _button.interactable = false;

            if (value < 0)
            {
                // show BOMB
                _img.sprite = Minesweeper.instance.BombSprite;
                _img.gameObject.SetActive(true);
                Minesweeper.instance.GameOver();
                return;
            }
            else   // (_value > 0)
            {
                // show cell value
                _number.SetText(value.ToString());
                _number.gameObject.SetActive(true);
                _img.gameObject.SetActive(false);
            }

            Minesweeper.instance.IncreaseOpenedCell();
        }
    }

    public void OpenAsEmpty()
    {
        // protection [value < 0: maybe is optional] & [IsOpened: maybe fiexed bug with ghost IncreaseOpenedCell (extra OpenedCell)]
        if (IsOpened || value < 0) return;

        _button.interactable = false;
        Minesweeper.instance.IncreaseOpenedCell();

        if (value > 0)
        {
            // show cell value
            _number.SetText(value.ToString());
            _number.gameObject.SetActive(true);
        }
    }

    private void ToggleFlag()
    {
        // protection
        if (IsOpened || (!_isFlag && !MinesweeperScore.instance.IsAbleToPutFlag))
            return;

        _isFlag = !_isFlag;

        if (_isFlag)
            Minesweeper.instance.PlayFlagAudio();

        _img.gameObject.SetActive(_isFlag);

        MinesweeperScore.instance.ChangeScore(_isFlag);
    }
}
