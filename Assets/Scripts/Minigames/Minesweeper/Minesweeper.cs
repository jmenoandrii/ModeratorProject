using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Minesweeper : App
{
    [Header("Minesweeper Components")]
    [SerializeField] protected GameObject _gameField;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Stopwatch _stopwatch;
    [SerializeField] private Image _emoji;
    [Header("Minesweeper Audio")]
    [SerializeField] private AudioSource _audioWin;
    [SerializeField] private AudioSource _audioLose;
    [SerializeField] private AudioSource _audioChoose;
    [SerializeField] private AudioSource _audioFlag;
    [SerializeField] private AudioSource _audioRestart;
    [Header("Minesweeper Sprites")]
    [SerializeField] private Sprite _flageSprite;
    [SerializeField] private Sprite _bombSprite;
    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _lossSprite;
    [SerializeField] private Sprite _neutralSprite;
    [Header("Minesweeper Input Data")]
    [SerializeField] private int _col;
    [SerializeField] private int _row;
    [SerializeField] private int _fixedBombCount;
    [SerializeField] private bool _isFixedBombCount = true;
    [SerializeField] private int _minbombCount;
    [SerializeField] private int _maxbombCount;
    private int _bombCount;
    // ***** init *****
    public static Minesweeper instance;     // (use in MinesweeperCell)
    public Sprite FlageSprite { get => _flageSprite; }
    public Sprite BombSprite { get => _bombSprite; }

    private MinesweeperCell[,] _cellField;
    public bool IsInitedField { get; private set; }

    private bool _isFirstOpen = true;

    private int _openedCellCount = 0;
    private int _cellCountToWin;
    public bool IsEndGame { get; private set; }

    private void Start()
    {
        // singleton initialization
        if (instance == null)
            instance = this;
        else
            Debug.LogError($"ERR[{gameObject.name}]: Minesweeper must be a singleton");

        _isFirstOpen = true;
        // initial settings
        Restart();

        // stopwatch
        if (!gameObject.activeSelf)
            _stopwatch.Refresh();
    }
    // ***** ***** *****

    private void CreateField()
    {
        GameObject obj;
        for (int i = 0; i < _col; i++)
        {
            for (int j = 0; j < _row; j++)
            {
                obj = Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity, transform);
                _cellField[i, j] = obj.GetComponent<MinesweeperCell>();

                obj.transform.SetParent(_gameField.transform, false);

                _cellField[i, j].SetCoord(i, j);
            }
        }
    }

    private bool IsNotAroundInitCoord(int x, int y, int initX, int initY)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int checkX = initX + dx;
                int checkY = initY + dy;

                if (checkX >= 0 && checkX < _row && checkY >= 0 && checkY < _col)
                {
                    if (x == checkX && y == checkY)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void InitField(int initX, int initY)
    {
        IsInitedField = true;

        // Calc the field number (gener bombs)
        int k = _isFixedBombCount ? _bombCount : Random.Range(_minbombCount, _maxbombCount);
        int i, j;
        while (k > 0)
        {
            i = Random.Range(0, _col);
            j = Random.Range(0, _row);

            if (_cellField[i, j].value >= 0 && IsNotAroundInitCoord(i, j, initX, initY))
            {
                _cellField[i, j].value = -10;

                // increase value around [i, j] cell
                if (i > 0) _cellField[i - 1, j].value++;
                if (j > 0) _cellField[i, j - 1].value++;
                if (i < _col - 1) _cellField[i + 1, j].value++;
                if (j < _row - 1) _cellField[i, j + 1].value++;
                if ((i > 0) && (j > 0)) _cellField[i - 1, j - 1].value++;
                if ((i > 0) && (j < _row - 1)) _cellField[i - 1, j + 1].value++;
                if ((i < _col - 1) && (j > 0)) _cellField[i + 1, j - 1].value++;
                if ((i < _col - 1) && (j < _row - 1)) _cellField[i + 1, j + 1].value++;

                k--;
            }
        }
    }

    public void Restart()
    {
        if (!_isFirstOpen)
            _audioRestart.Play();
        _isFirstOpen = false;

        IsEndGame = false;
        IsInitedField = false;

        // Stopwatch
        _stopwatch.Begin();

        // Initialization
        _cellField = new MinesweeperCell[_col, _row];
        _openedCellCount = 0;

        // Score
        _bombCount = _isFixedBombCount ? _fixedBombCount : Random.Range(_minbombCount, _maxbombCount);
        _cellCountToWin = _col * _row - _bombCount;
        MinesweeperScore.instance.SetMaxScore(_bombCount);
        MinesweeperScore.instance.SetScoreAsMax();

        // Emoji
        _emoji.sprite = _neutralSprite;

        // Clear field
        foreach (Transform child in _gameField.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Create field
        CreateField();
    }

    public void IncreaseOpenedCell()
    {
        // protection [maybe is optional]
        if (IsEndGame) return;

        _openedCellCount++;

        if (_openedCellCount == _cellCountToWin)
            GameWin();
    }

    public void OpenCell(int x, int y)
    {
        if (_cellField[x, y].IsOpened || IsEndGame) return;

        _cellField[x, y].OpenAsEmpty();

        if (_cellField[x, y].value == 0)
        {
            // open cells around [x, y] cell with zero value
            if (x > 0) OpenCell(x - 1, y);
            if (y > 0) OpenCell(x, y - 1);
            if (x < _col - 1) OpenCell(x + 1, y);
            if (y < _row - 1) OpenCell(x, y + 1);
            if ((x > 0) && (y > 0)) OpenCell(x - 1, y - 1);
            if ((x > 0) && (y < _row - 1)) OpenCell(x - 1, y + 1);
            if ((x < _col - 1) && (y > 0)) OpenCell(x + 1, y - 1);
            if ((x < _col - 1) && (y < _row - 1)) OpenCell(x + 1, y + 1);
        }
    }

    public void GameOver()
    {
        IsEndGame = true;

        _stopwatch.Stop();

        _audioLose.Play();
        _emoji.sprite = _lossSprite;
    }

    private void GameWin()
    {
        IsEndGame = true;

        _stopwatch.Stop();

        _audioWin.Play();
        _emoji.sprite = _winSprite;
    }


    // ***** audio *****
    public void PlayFlagAudio()
    {
        _audioFlag.Stop();
        _audioFlag.Play();
    }

    public void PlayChooseCellAudio()
    {
        _audioChoose.Stop();
        _audioChoose.pitch = Random.Range(0.75f, 1.2f);
        _audioChoose.Play();
    }
    // ***** ***** *****

    // ***** override app process *****
    public override void OpenApp()
    {
        if (!IsAppActive)
            _stopwatch.Begin();

        base.OpenApp();
    }

    public override void CloseApp()
    {
        Restart();

        _stopwatch.Refresh();

        base.CloseApp();
    }

    public override void HideApp()
    {
        _stopwatch.Stop();

        base.HideApp();
    }

    public override void ShowApp()
    {
        if (!IsEndGame)
            _stopwatch.Play();

        base.ShowApp();
    }
    // ***** ***** *****
}
