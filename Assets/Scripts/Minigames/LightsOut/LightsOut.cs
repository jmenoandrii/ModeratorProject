using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class LightsOut : App
{
    [Header("LightsOut Components")]
    [SerializeField] private GameObject _fieldBox;
    [SerializeField] private Counter _counter;
    [SerializeField] private Stopwatch _stopwatch;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Image _emoji;    // TODO: !!! it should be smth. else ...
    [Header("LightsOut Sprites")]   // TODO: !!! it should be smth. else (like a win screen instead of field of cells), but for now let it be like this
    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _neutralSprite;
    [Header("LightsOut Input Data")]
    [SerializeField] private int _col = 5;
    [SerializeField] private int _row = 5;
    [SerializeField] private Color _offColor = new Color(0.231f, 0.231f, 0.231f, 1f);
    [SerializeField] private Color _onColor = new Color(0.207f, 0.69f, 0.325f, 1f);
    [SerializeField] private int _fixedOnCellCount;
    [SerializeField] private bool _isFixedOnCellCount = false;
    [SerializeField] private int _minOnCellCount = 1;
    [SerializeField] private int _maxOnCellCount = 10;
    [SerializeField] private bool _isAutoCalcOnCell = true; // _minOnCellCount = (_col * _row * 1 / 5) , _maxOnCellCount = (_col * _row * 4 / 5)
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _restartSound;
    private int _onCellCount;
    // ***** init *****
    public static LightsOut instance { get; private set; }
    private LightCell[,] _field;
    public bool IsEndGame { get; private set; }
    private bool _isFirstOpen = true;

    private void Start()
    {
        // singleton initialization
        if (instance == null)
            instance = this;
        else
            Debug.LogError($"ERR[{gameObject.name}]: LightsOut must be a singleton");

        // verification
        if (!_isAutoCalcOnCell) 
        {
            if (_minOnCellCount < 1)
                Debug.LogError($"ERR[{gameObject.name}]: _minOnCellCount must be >= 1");
            else if (_maxOnCellCount >= _col * _row)
                Debug.LogError($"ERR[{gameObject.name}]: _minOnCellCount must be < {_col * _row}");
            else if (_minOnCellCount >= _maxOnCellCount)
                Debug.LogError($"ERR[{gameObject.name}]: _minOnCellCount must be < _maxOnCellCount");
        }
        else   // calc OnCellCount (_isAutoCalcOnCell == true)
        {
            _minOnCellCount = _col * _row / 5;
            _maxOnCellCount = _col * _row * 4 / 5;
        }

        _isFirstOpen = true;
        // initial settings
        Restart();
    }
    // ***** ***** *****

    private void CreateField()
    {
        // clear field
        foreach (Transform child in _fieldBox.transform)
        {
            Destroy(child.gameObject);
        }

        _field = new LightCell[_row, _col];

        // create cells
        GameObject obj;
        int i, j;
        for (i = 0; i < _row; i++)
        {
            for (j = 0; j < _col; j++)
            {
                obj = Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity, transform);
                _field[i, j] = obj.GetComponent<LightCell>();

                obj.transform.SetParent(_fieldBox.transform, false);

                _field[i, j].SetCoord(i, j);
                _field[i, j].CellImage.color = _offColor;
            }
        }

        // rand turn on some cells
        int k = Random.Range(_minOnCellCount, _maxOnCellCount);
        while (k > 0)
        {
            i = Random.Range(0, _row);
            j = Random.Range(0, _col);

            if (!_field[i, j].isActive)
            {
                ToggleCell(i, j);

                k--;
            }
        }
    }

    public void HandleClick(int x, int y)
    {
        // protection
        if (IsEndGame) return;

        _clickSound.Pause();
        _clickSound.pitch = Random.Range(0.75f, 1.2f);
        _clickSound.Play();

        _counter.Increment();

        ToggleCell(x, y);

        // toggle cells around [i, j] cell
        if (x > 0) ToggleCell(x - 1, y);
        if (y > 0) ToggleCell(x, y - 1);
        if (x < _col - 1) ToggleCell(x + 1, y);
        if (y < _row - 1) ToggleCell(x, y + 1);

        CheckWin();
    }

    private void ToggleCell(int x, int y)
    {
        _field[x, y].isActive = !_field[x, y].isActive;

        _field[x, y].CellImage.color = _field[x, y].isActive ? _onColor : _offColor;
    }

    private void CheckWin()
    {
        bool state = _field[0, 0].isActive;
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _col; j++)
            {
                if (_field[i, j].isActive != state)
                    return;
            }
        }

        // victory processing
        IsEndGame = true;
        ////_fieldBox.SetActive(false);////
        ////_victoryBox.SetActive(true);////
        _emoji.sprite = _winSprite;
        _winSound.Play();

        _stopwatch.Stop();
    }

    public void Restart()
    {
        if (!_isFirstOpen)
            _restartSound.Play();
        _isFirstOpen = false;

        IsEndGame = false;

        // Stopwatch
        _stopwatch.Begin();

        // Counter
        _counter.Refresh();

        // Emoji
        _emoji.sprite = _neutralSprite;

        // Calc OnCellCount
        _onCellCount = _isFixedOnCellCount ? _fixedOnCellCount : Random.Range(_minOnCellCount, _maxOnCellCount);

        // Create field
        CreateField();

        ////_victoryBox.SetActive(false);////
        ////_fieldBox.SetActive(true);////
    }

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
