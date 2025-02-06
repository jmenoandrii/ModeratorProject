using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color _authoritarianismColor;
    [SerializeField] private Color _democracyColor;
    [SerializeField] private Color _conspiracyColor;
    [SerializeField] private Color _scienceColor;
    [Header("Components")]
    [SerializeField] private int _minAxisValue;
    [SerializeField] private int _maxAxisValue;
    private int _maxAuToDemProvinceCount;    // _authoritarianismToDemocracy provinces
    private int _maxConToSciProvinceCount;   // _conspiracyToScience provinces
    private int _curAuProvinceCount;
    private int _curConProvinceCount;
    [SerializeField] private List<Province> _provinces;
    private (int, int) _currentTendence;
    // ***** ***** *****
    public static Color AuthoritarianismColor { get; private set; }
    public static Color DemocracyColor { get; private set; }
    public static Color ConspiracyColor { get; private set; }
    public static Color ScienceColor { get; private set; }

    private void Awake()
    {
        GlobalEventManager.OnChangeAxis += ChangeAxisHandler;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnChangeAxis -= ChangeAxisHandler;
    }
    private void Start()
    {
        AuthoritarianismColor = _authoritarianismColor;
        DemocracyColor = _democracyColor;
        ConspiracyColor = _conspiracyColor;
        ScienceColor = _scienceColor;

        InitDistribution();
        DistributeColorsOnMap((0, 0));
    }
    // ***** ***** *****

    private void InitDistribution()
    {
        // protection
        if (_provinces.Count == 0)
        {
            Debug.LogError($"ERR[{gameObject.name}]: _provinces lsit is void");
            return;
        }

        List<IdeologyCounter> provinceIdeologyList = new List<IdeologyCounter>(){
            new IdeologyCounter(Ideology.Authoritarianism, _provinces.Count / 4),
            new IdeologyCounter(Ideology.Authoritarianism, _provinces.Count / 4),
            new IdeologyCounter(Ideology.Authoritarianism, _provinces.Count / 4),
            new IdeologyCounter(Ideology.Authoritarianism, _provinces.Count - (int)(_provinces.Count / 4) * 3),
        };
        _maxAuToDemProvinceCount = provinceIdeologyList[0].count + provinceIdeologyList[1].count;
        _maxConToSciProvinceCount = provinceIdeologyList[2].count + provinceIdeologyList[3].count;
        Debug.Log($"{_provinces.Count} = {provinceIdeologyList[0].count} + {provinceIdeologyList[1].count} + {provinceIdeologyList[2].count} + {provinceIdeologyList[3].count} = {_maxAuToDemProvinceCount + _maxConToSciProvinceCount}");

        for (int i = 0, randIdeologyIndex; i < _provinces.Count; i++)
        {
            randIdeologyIndex = Random.Range(0, provinceIdeologyList.Count);

            _provinces[i].SetIdeology(provinceIdeologyList[randIdeologyIndex].ideology);
            provinceIdeologyList[randIdeologyIndex].Decrease();

            if (provinceIdeologyList[randIdeologyIndex].count == 0)
                provinceIdeologyList.RemoveAt(randIdeologyIndex);
        }
    }

    private void ChangeAxisHandler(int _conspiracyToScienceValue, int _conservatismToProgressValue, int _communismToCapitalismValue, int _authoritarianismToDemocracyValue, int _pacifismToMilitarismValue)
    {
        DistributeColorsOnMap((_authoritarianismToDemocracyValue, _conspiracyToScienceValue));
    }

    private int InterpolateValue(int value, int initMin, int initMax, int newMin, int newMax)
    {
        return Mathf.RoundToInt((value - initMin) * (newMax - newMin) / (float)(initMax - initMin) + newMin);
    }

    private void DistributeColorsOnMap((int, int) tendence)
    {
        int newAuProvinceCount = InterpolateValue(tendence.Item1 - _currentTendence.Item1, _minAxisValue, _maxAxisValue, 0, _maxAuToDemProvinceCount);
        int newConProvinceCount = InterpolateValue(tendence.Item2 - _currentTendence.Item2, _minAxisValue, _maxAxisValue, 0, _maxConToSciProvinceCount);

        int randIndex;
        int dif = newAuProvinceCount - _curAuProvinceCount;
        if (dif > 0)
        {
            // add Authoritarianism 
            while (dif != 0)
            {
                randIndex = Random.Range(0, _provinces.Count);
                if (_provinces[randIndex].Ideology == Ideology.Democracy)
                {
                    _provinces[randIndex].SetIdeology(Ideology.Authoritarianism);
                    dif--;
                }
            }
        }
        else
        {
            // add Democracy
            while (dif != 0)
            {
                randIndex = Random.Range(0, _provinces.Count);
                if (_provinces[randIndex].Ideology == Ideology.Authoritarianism)
                {
                    _provinces[randIndex].SetIdeology(Ideology.Democracy);
                    dif++;
                }
            }
        }


        dif = newConProvinceCount - _curConProvinceCount;
        if (dif > 0)
        {
            // add Conspiracy 
            while (dif != 0)
            {
                randIndex = Random.Range(0, _provinces.Count);
                if (_provinces[randIndex].Ideology == Ideology.Science)
                {
                    _provinces[randIndex].SetIdeology(Ideology.Conspiracy);
                    dif--;
                }
            }
        }
        else
        {
            // add Science
            while (dif != 0)
            {
                randIndex = Random.Range(0, _provinces.Count);
                if (_provinces[randIndex].Ideology == Ideology.Conspiracy)
                {
                    _provinces[randIndex].SetIdeology(Ideology.Science);
                    dif++;
                }
            }
        }
    }

    public enum Ideology
    {
        Neutral,
        Authoritarianism,
        Democracy,
        Conspiracy,
        Science
    }
    
    private struct IdeologyCounter
    {
        public Ideology ideology;
        public int count;

        public void Decrease() { count--; }

        public IdeologyCounter(Ideology ideology, int count)
        {
            this.ideology = ideology;
            this.count = count;
        }
    }
}
