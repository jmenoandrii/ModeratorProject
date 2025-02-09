using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    // Class to define axis ranges
    [System.Serializable]
    public class AxisRange
    {
        public int min = -50;
        public int max = 50;

        public bool IsInRange(int value)
        {
            return value >= min && value <= max;
        }
    }


    [SerializeField, Range(-50, 50)] private int _conspiracyToScienceValue;
    [SerializeField, Range(-50, 50)] private int _conservatismToProgressValue;
    [SerializeField, Range(-50, 50)] private int _communismToCapitalismValue;
    [SerializeField, Range(-50, 50)] private int _authoritarianismToDemocracyValue;
    [SerializeField, Range(-50, 50)] private int _pacifismToMilitarismValue;

    [SerializeField] private List<Ending> _endings;

    [Header("Standart Endings")]
    [SerializeField] private int _limitValue = 35;
    [SerializeField] private StandartEnding _conspiracyEnding;
    [SerializeField] private StandartEnding _scienceEnding;
    [SerializeField] private StandartEnding _conservatismEnding;
    [SerializeField] private StandartEnding _progressEnding;
    [SerializeField] private StandartEnding _communismEnding;
    [SerializeField] private StandartEnding _capitalismEnding;
    [SerializeField] private StandartEnding _authoritarianismEnding;
    [SerializeField] private StandartEnding _democracyEnding;
    [SerializeField] private StandartEnding _pacifismEnding;
    [SerializeField] private StandartEnding _militarismEnding;

    void Start()
    {
        // !protection! : ending ids
        ValidateEndings(_endings, new List<StandartEnding> {
            _conspiracyEnding, _scienceEnding, _conservatismEnding, _progressEnding, _communismEnding,
            _capitalismEnding, _authoritarianismEnding, _democracyEnding, _pacifismEnding, _militarismEnding
        });
    }

    void ValidateEndings(List<Ending> endings, List<StandartEnding> standardEndings)
    {
        var allEndings = endings.Select(e => e.id).ToList();
        allEndings.AddRange(standardEndings.Where(se => se != null).Select(se => se.id));

        // Check for unique
        if (allEndings.Distinct().Count() != allEndings.Count)
        {
            Debug.LogError("ERR[EndingManager]: IDs aren't unique!");
            return;
        }

        // Check for gaps
        var sortedIds = allEndings.OrderBy(id => id).ToList();
        for (int i = 0; i < sortedIds.Count; i++)
        {
            if (sortedIds[i] != i)
            {
                Debug.LogError($"ERR[EndingManager]: Missing id: {i}");
                return;
            }
        }

        // Calc count
        if (EndingBookManager.instance.EndingCount != endings.Count + standardEndings.Count(se => se != null))
        {
            Debug.LogError($"ERR[EndingManager]: EndingBookManager.EndingCount != calc count in EndingManager");
        }

        /*Debug.Log($"INFO[EndingManager]: IDs are correct! Ending count: {endings.Count + standardEndings.Count(se => se != null)}");*/
    }

    private void Awake()
    {
        GlobalEventManager.OnSendImpact += EditValue;
        GlobalEventManager.OnEndInitiate += EndGame;
        GlobalEventManager.OnInitWorldIndex += InitWorldIndex;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnSendImpact -= EditValue;
        GlobalEventManager.OnEndInitiate -= EndGame;
        GlobalEventManager.OnInitWorldIndex += InitWorldIndex;
    }

    private T GetConditionalResult<T>(int value, T negativeRes, T positiveRes)
    {
        if (value < 0)
            return negativeRes;
        else if (value > 0)
            return positiveRes;

        return default(T);
    }

    public void DetermineEnding()
    {
        // Standart Endings processing
        StandartEnding selectedStandartEnding = null;

        int[] absValues = { 
            Math.Abs(_conspiracyToScienceValue), 
            Math.Abs(_conservatismToProgressValue),
            Math.Abs(_communismToCapitalismValue), 
            Math.Abs(_authoritarianismToDemocracyValue), 
            Math.Abs(_pacifismToMilitarismValue) 
        };
        int maxValue = Mathf.Max(absValues);

        int maxIndex = Array.IndexOf(absValues, maxValue);

        if (absValues.Count(value => value == maxValue) == 1 
            && absValues[maxIndex] >= _limitValue)
        {
            switch (maxIndex)
            {
                case 0:
                    selectedStandartEnding = GetConditionalResult(_conspiracyToScienceValue, _conspiracyEnding, _scienceEnding);
                    break;
                case 1:
                    selectedStandartEnding = GetConditionalResult(_conservatismToProgressValue, _conservatismEnding, _progressEnding);
                    break;
                case 2:
                    selectedStandartEnding = GetConditionalResult(_communismToCapitalismValue, _communismEnding, _capitalismEnding);
                    break;
                case 3:
                    selectedStandartEnding = GetConditionalResult(_authoritarianismToDemocracyValue, _authoritarianismEnding, _democracyEnding);
                    break;
                case 4:
                    selectedStandartEnding = GetConditionalResult(_pacifismToMilitarismValue, _pacifismEnding, _militarismEnding);
                    break;
            }
        }

        if (selectedStandartEnding != null)
        {
            EndingSummary summary = new()
            {
                Name = selectedStandartEnding.endingName,
                Description = selectedStandartEnding.description,
                Image = selectedStandartEnding.image,
                Timeline = selectedStandartEnding.timeline,
                CryptoWalletBalance = GameStats.Instance.CryptoWalletBalance,
                VictimsCount = GameStats.Instance.VictimsCount
            };

            EndingBookManager.instance.UnlockEnding(selectedStandartEnding.id);
            GlobalEventManager.CallOnEndGame(summary);
            return;
        }

        // Other Endings processing
        Ending selectedEnding = null;

        foreach (Ending ending in _endings)
        {
            if (ending.axisConspiracyToScience.IsInRange(_conspiracyToScienceValue) &&
                ending.axisConservatismToProgress.IsInRange(_conservatismToProgressValue) &&
                ending.axisCommunismToCapitalism.IsInRange(_communismToCapitalismValue) &&
                ending.axisAuthoritarianismToDemocracy.IsInRange(_authoritarianismToDemocracyValue) &&
                ending.axisPacifismToMilitarism.IsInRange(_pacifismToMilitarismValue))
            {
                selectedEnding = ending;
                break;
            }
        }

        if (selectedEnding != null)
        {
            EndingSummary summary = new()
            {
                Name = selectedEnding.endingName,
                Description = selectedEnding.description,
                Image = selectedEnding.image,
                Timeline = selectedEnding.timeline,
                CryptoWalletBalance = GameStats.Instance.CryptoWalletBalance,
                VictimsCount = GameStats.Instance.VictimsCount
            };

            EndingBookManager.instance.UnlockEnding(selectedEnding.id);
            GlobalEventManager.CallOnEndGame(summary);
        }
    }

    public void EndGame()
    {
        DetermineEnding();
    }

    private void EditValue(AdminPostLoader.Impact impact)
    {
        _conspiracyToScienceValue += impact.conspiracyToScienceValue;
        _conservatismToProgressValue += impact.conservatismToProgressValue;
        _communismToCapitalismValue += impact.communismToCapitalismValue;
        _authoritarianismToDemocracyValue += impact.authoritarianismToDemocracyValue;
        _pacifismToMilitarismValue += impact.pacifismToMilitarismValue;

        GlobalEventManager.CallOnChangeAxis(_conspiracyToScienceValue, _conservatismToProgressValue, _communismToCapitalismValue, _authoritarianismToDemocracyValue, _pacifismToMilitarismValue);
    }

    private void InitWorldIndex()
    {
        GlobalEventManager.CallOnChangeAxis(_conspiracyToScienceValue, _conservatismToProgressValue, _communismToCapitalismValue, _authoritarianismToDemocracyValue, _pacifismToMilitarismValue);
    }
}

[System.Serializable]
public class EndingSummary
{
    public string Name;
    public string Description;
    public int CryptoWalletBalance;
    public int VictimsCount;   
    
    public Sprite Image;
    public string Timeline;
}