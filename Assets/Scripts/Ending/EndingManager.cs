using System.Collections.Generic;
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

    [SerializeField] private List<Ending> _endings;  
    [SerializeField, Range(-50, 50)] private int _conspiracyToScienceValue;
    [SerializeField, Range(-50, 50)] private int _conservatismToProgressValue;
    [SerializeField, Range(-50, 50)] private int _communismToCapitalismValue;
    [SerializeField, Range(-50, 50)] private int _authoritarianismToDemocracyValue;
    [SerializeField, Range(-50, 50)] private int _pacifismToMilitarismValue;

    public void DetermineEnding()
    {
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
                CryptoWalletBalance = GameStats.Instance.CryptoWalletBalance,
                VictimsCount = GameStats.Instance.VictimsCount
            };

            GlobalEventManager.CallOnEndGame(summary);
        }
    }

    public void EndGame()
    {
        DetermineEnding();
    }
}

[System.Serializable]
public class EndingSummary
{
    public string Name;
    public string Description;
    public int CryptoWalletBalance;
    public int VictimsCount;
}