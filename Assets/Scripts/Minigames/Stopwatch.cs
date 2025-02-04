using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text _stopwatch;
    [Header("Parameters")]
    [SerializeField] private TimeFormat _timeFormat;
    private float _elapsedTime = 0f;
    private bool _isRunning = false;

    void Awake()
    {
        UpdateStopwatch();
    }

    void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateStopwatch();
        }
    }

    public void Play()
    {
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private void UpdateStopwatch()
    {
        int seconds;
        switch (_timeFormat)
        {
            case TimeFormat.MM_SS:
                int minutes = Mathf.FloorToInt(_elapsedTime / 60);
                seconds = Mathf.FloorToInt(_elapsedTime % 60);
                if (minutes < 100)
                    _stopwatch.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                else
                    _stopwatch.text = "∞";
                break;
            case TimeFormat.SSS:
                seconds = Mathf.FloorToInt(_elapsedTime);
                if (seconds > 999)
                    _stopwatch.text = "∞";
                else
                    _stopwatch.text = $"{seconds:000}";
                break;
        }
    }

    public void Begin()
    {
        _isRunning = true;
        _elapsedTime = 0f;
        UpdateStopwatch();
    }

    public void Refresh()
    {
        _isRunning = false;
        _elapsedTime = 0f;
        UpdateStopwatch();
    }

    [System.Serializable]
    private enum TimeFormat
    {
        MM_SS,
        SSS
    }
}
