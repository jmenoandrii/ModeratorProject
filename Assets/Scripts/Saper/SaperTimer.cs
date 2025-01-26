using TMPro;
using UnityEngine;

public class SaperTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    private float _elapsedTime;
    private bool _isRunning;

    public void StartTimer()
    {
        _elapsedTime = 0f;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
        _elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    public void PauesTimer()
    {
        _isRunning = false;
    }

    public void PlayTimer()
    {
        _isRunning = true;
    }

    public void ResetTimer()
    {
        _elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        /*int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        _timerText.text = $"{minutes:D2}:{seconds:D2}"; // "MM:SS"*/
        int seconds = Mathf.FloorToInt(_elapsedTime);
        if (seconds > 999)
            _timerText.text = "∞";
        else
            _timerText.text = $"{seconds:000}";
    }
}
