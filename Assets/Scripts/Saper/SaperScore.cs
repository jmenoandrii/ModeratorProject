using TMPro;
using UnityEngine;

public class SaperScore : MonoBehaviour
{
    public static SaperScore instance;
    [SerializeField] private TMP_Text _scoreText;
    public int Score { get; private set; }
    public int BombCount { get; private set; }
    public bool IsAbleToDecrease { get => Score != 0; }
    public bool IsAbleToIncrease { get => Score != BombCount; }
    
    private void Awake()
    {
        BombCount = 1;

        instance = this;
    }

    public void SetBomdCount(int count)
    {
        if (count > 0)
            BombCount = count;
    }

    public void SetMaxScore()
    {
        Score = BombCount;
        UpdateScore();
    }

    public void ChangeScore(bool isFlag)
    {
        if (isFlag)
            DecreaseScore();
        else
            IncreaseScore();
    }

    public void IncreaseScore()
    {
        if (IsAbleToIncrease)
        {
            Score++;
            UpdateScore();
        }
    }

    public void DecreaseScore()
    {
        if (IsAbleToDecrease)
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
