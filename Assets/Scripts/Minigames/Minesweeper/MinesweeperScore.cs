using TMPro;
using UnityEngine;

public class MinesweeperScore : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text _scoreText;
    // ***** init *****
    public static MinesweeperScore instance;   // (use in MinesweeperCell)
    public int Score { get; private set; }
    public int MaxScore { get; private set; }
    public bool IsAbleToPutFlag { get => Score > 0; }   // IsAbleToPutFlag = IsAbleToDecrease

    private void Awake()
    {
        // singleton initialization
        if (instance == null)
            instance = this;
        else
            Debug.LogError($"ERR[{gameObject.name}]: MinesweeperScore must be a singleton");
    }

    public void SetMaxScore(int count)
    {
        if (count > 0)
            MaxScore = count;
        else
            Debug.LogWarning($"WRN[{gameObject.name}]: count is <= 0 in SetMaxScore - MaxScore isn't setted");
    }

    public void SetScoreAsMax()
    {
        Score = MaxScore;
        UpdateScore();
    }
    // ***** ***** *****

    public void ChangeScore(bool isFlag)
    {
        if (isFlag)
            DecreaseScore();
        else
            IncreaseScore();
    }

    public void IncreaseScore()
    {
        if (Score < MaxScore)
        {
            Score++;
            UpdateScore();
        }
    }

    public void DecreaseScore()
    {
        if (IsAbleToPutFlag)
        {
            Score--;
            UpdateScore();
        }

    }

    public void UpdateScore()
    {
        _scoreText.SetText($"{Score:000}");
    }
}
