using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Saper : App
{
    public static Saper instance;
    [Header("Saper Settings")]
    [SerializeField] private int _xCount;
    [SerializeField] private int _yCount;
    [SerializeField] private int _bombCount;
    [Header("Saper Components")]
    [SerializeField] protected GameObject _gameField;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private SaperTimer _timer;
    [SerializeField] private SaperScore _score;
    [SerializeField] private Image _emoji;
    [Header("Saper Sprites")]
    [SerializeField] private Sprite _flageSprite;
    [SerializeField] private Sprite _bombSprite;
    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _lossSprite;
    [SerializeField] private Sprite _neutralSprite; // can be null

    private int[,] _valueField;
    private SaperCell[,] _cellField;
    private int _openedCellCount = 0;
    private int _cellCountToWin;
    private (int, int) _initCoord;
    private bool _isInited = false;
    public bool IsEndGame { get; private set; }

    public static Sprite FlageSprite { get; private set; }
    public static Sprite BombSprite { get; private set; }
    public int GetValue(int x, int y) { return _isInited ? _valueField[x, y] : -1000; }

    private void Awake()
    {
        GlobalEventManager.OnOpenEnptySaperCell += OpenCell;
        GlobalEventManager.OnSaperGameOver += GameOver;
        GlobalEventManager.OnInitSaperField += InitField;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnOpenEnptySaperCell -= OpenCell;
        GlobalEventManager.OnSaperGameOver -= GameOver;
        GlobalEventManager.OnInitSaperField -= InitField;
    }

    private void Start()
    {
        // Set the static value
        instance = this;
        FlageSprite = _flageSprite;
        BombSprite = _bombSprite;
        _cellCountToWin = _xCount * _yCount - _bombCount;

        SaperScore.instance.SetBomdCount(_bombCount);

        Restart();

        // Timer
        if (gameObject.activeSelf)
            _timer.StartTimer();
    }

    private bool CheckAroundInitCoord(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int checkX = _initCoord.Item1 + dx;
                int checkY = _initCoord.Item2 + dy;

                if (checkX >= 0 && checkX < _yCount && checkY >= 0 && checkY < _xCount)
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

    private void InitField(int initX, int initY)
    {
        _isInited = true;
        _initCoord = (initX, initY);

        // Calc the field number (gener bombs)
        int k = _bombCount;
        int i, j;
        while (k > 0)
        {
            i = Random.Range(0, _xCount);
            j = Random.Range(0, _yCount);

            if (_valueField[i, j] >= 0 && CheckAroundInitCoord(i, j))
            {
                _valueField[i, j] = -10;
                if (i > 0) _valueField[i - 1, j]++;
                if (j > 0) _valueField[i, j - 1]++;
                if (i < _xCount - 1) _valueField[i + 1, j]++;
                if (j < _yCount - 1) _valueField[i, j + 1]++;
                if ((i > 0) && (j > 0)) _valueField[i - 1, j - 1]++;
                if ((i > 0) && (j < _yCount - 1)) _valueField[i - 1, j + 1]++;
                if ((i < _xCount - 1) && (j > 0)) _valueField[i + 1, j - 1]++;
                if ((i < _xCount - 1) && (j < _yCount - 1)) _valueField[i + 1, j + 1]++;

                k--;
            }
        }
    }

    public void IncreaseOpenedCell()
    {
        _openedCellCount++;

        if (_openedCellCount == _cellCountToWin)
            GameWin();
    }

    private void OpenCell(int x, int y)
    {
        if (_cellField[x, y].IsOpened || IsEndGame) return;

        _cellField[x, y].OpenAsEmpty(_valueField[x, y]);  

        if (_valueField[x, y] == 0)
        {
            if (x > 0) OpenCell(x - 1, y);
            if (y > 0) OpenCell(x, y - 1);
            if (x < _xCount - 1) OpenCell(x + 1, y);
            if (y < _yCount - 1) OpenCell(x, y + 1);
            if ((x > 0) && (y > 0)) OpenCell(x - 1, y - 1);
            if ((x > 0) && (y < _yCount - 1)) OpenCell(x - 1, y + 1);
            if ((x < _xCount - 1) && (y > 0)) OpenCell(x + 1, y - 1);
            if ((x < _xCount - 1) && (y < _yCount - 1)) OpenCell(x + 1, y + 1);
        }
    }

    public void Restart()
    {
        IsEndGame = false;
        _isInited = false;

        // timer
        _timer.StartTimer();

        // Initialization
        _valueField = new int[_xCount, _yCount];
        _cellField = new SaperCell[_xCount, _yCount];
        _openedCellCount = 0;

        // Score
        SaperScore.instance.SetMaxScore();

        // Emoji
        _emoji.sprite = _neutralSprite;

        // Clear field
        foreach (Transform child in _gameField.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Create field
        GameObject obj;
        for (int i = 0; i < _xCount; i++)
        {
            for (int j = 0; j < _yCount; j++)
            {
                obj = Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity, transform);
                _cellField[i, j] = obj.GetComponent<SaperCell>();

                obj.transform.SetParent(_gameField.transform, false);

                _cellField[i, j].SetCoord(i, j);
            }
        }
    }

    private void GameOver()
    {
        IsEndGame = true;

        _timer.PauesTimer();

        _emoji.sprite = _lossSprite;
    }

    private void GameWin()
    {
        IsEndGame = true;

        _timer.PauesTimer();

        _emoji.sprite = _winSprite;
    }

    public override void OpenApp()
    {
        _timer.StartTimer();

        base.OpenApp();
    }

    public override void CloseApp()
    {
        Restart();

        _timer.StopTimer();

        base.CloseApp();
    }

    public override void HideApp()
    {
        _timer.PauesTimer();

        base.HideApp();
    }

    public override void ShowApp()
    {
        _timer.PlayTimer();

        base.ShowApp();
    }
}
