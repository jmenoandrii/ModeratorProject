using Unity.VisualScripting;
using UnityEngine;

public class EndingBookManager : MonoBehaviour
{
    [SerializeField] private int _endingCount;
    private EndingCurtain[] _endingCurtains;
    public static EndingBookManager instance;
    public int EndingCount { get { return _endingCount; } }

    private void Awake()
    {
        GlobalEventManager.OnResetEndings += ResetEndings;

        Debug.Log($"INFO[EndingBookManager]: PlayerPrefs = {PlayerPrefs.GetInt("Endings", 0)}");
        // singleton initialization
        if (instance == null)
            instance = this;
        else
            Debug.LogError($"ERR[{gameObject.name}]: EndingBookManager must be a singleton");
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnResetEndings -= ResetEndings;
    }

    public void UnlockEnding(int endingIndex)
    {
        if (_endingCount <= 0)
        {
            Debug.LogError($"ERR[EndingBookManager|UnlockEnding()]: _endingCount isn't setted");
            return;
        }
        if (endingIndex < 0 || endingIndex >= _endingCount)
        {
            Debug.LogError($"ERR[EndingBookManager|UnlockEnding()]: Invalid ending index: {endingIndex}. Must be between 0 and {_endingCount - 1}.");
            return;
        }

        int endings = PlayerPrefs.GetInt("Endings", 0);
        endings |= (1 << endingIndex); // Set bit at index
        PlayerPrefs.SetInt("Endings", endings);
        PlayerPrefs.Save();
    }

    public bool IsEndingUnlocked(int endingIndex)
    {
        if (_endingCount <= 0)
        {
            Debug.LogError($"ERR[EndingBookManager|IsEndingUnlocked()]: _endingCount isn't setted");
            return false;
        }
        if (endingIndex < 0 || endingIndex >= _endingCount)
        {
            Debug.LogError($"ERR[EndingBookManager|IsEndingUnlocked()]: Invalid ending index: {endingIndex}. Must be between 0 and {_endingCount - 1}.");
            return false;
        }

        int endings = PlayerPrefs.GetInt("Endings", 0);
        return (endings & (1 << endingIndex)) != 0;
    }


    public bool HasAnyEndingUnlocked()
    {
        return PlayerPrefs.GetInt("Endings", 0) != 0;
    }

    public bool AreAllEndingsUnlocked()
    {
        if (_endingCount <= 0)
        {
            Debug.LogError($"ERR[EndingBookManager|AreAllEndingsUnlocked()]: _endingCount isn't set");
            return false;
        }

        int endings = PlayerPrefs.GetInt("Endings", 0);
        int allEndingsMask = (1 << _endingCount) - 1; // ������ ����� ��� ��� �������

        return (endings & allEndingsMask) == allEndingsMask;
    }

    public void ResetEndings()
    {
        PlayerPrefs.SetInt("Endings", 0);
        PlayerPrefs.Save();

        Debug.Log($"INFO[EndingBookManager|RESET]: PlayerPrefs = {PlayerPrefs.GetInt("Endings", 0)}");
    }
}
