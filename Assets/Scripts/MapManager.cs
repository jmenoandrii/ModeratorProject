using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color _authoritarianismColor;
    [SerializeField] private Color _democracyColor;
    [SerializeField] private Color _conspiracyColor;
    [SerializeField] private Color _scienceColor;
    [Header("Components")]
    [SerializeField, Range(2, 10)] private int _provinceSwapCoef;
    [SerializeField] private int _minAxisValue;
    [SerializeField] private int _maxAxisValue;
    private int _maxAuToDemProvinceCount;    // _authoritarianismToDemocracy provinces
    private int _maxConToSciProvinceCount;   // _conspiracyToScience provinces
    private int _curAuProvinceCount;
    private int _curConProvinceCount;
    [SerializeField] private List<Province> _provinces;
    private Vector2Int _currentTendence;
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
        _currentTendence = Vector2Int.zero;
        DistributeColorsOnMap(_currentTendence);
    }
    // ***** ***** *****

    private void InitDistribution()
    {
        // protection
        if (_provinces.Count == 0)
        {
            Debug.LogError($"ERR[{gameObject.name}]: _provinces lsit is empty");
            return;
        }

        List<IdeologyCounter> provinceIdeologyList = new List<IdeologyCounter>(){
            new IdeologyCounter(Ideology.Authoritarianism, _provinces.Count / 4),
            new IdeologyCounter(Ideology.Democracy, _provinces.Count / 4),
            new IdeologyCounter(Ideology.Conspiracy, _provinces.Count / 4),
            new IdeologyCounter(Ideology.Science, _provinces.Count - (int)(_provinces.Count / 4) * 3),
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
        DistributeColorsOnMap(new Vector2Int(_authoritarianismToDemocracyValue, _conspiracyToScienceValue));
    }

    private int InterpolateValue(int value, int initMin, int initMax, int newMin, int newMax)
    {
        return Mathf.RoundToInt((value - initMin) * (newMax - newMin) / (float)(initMax - initMin) + newMin);
    }

    private void ChangeIdeology(Ideology _old, Ideology _new, int count)
    {
        var targetIndexes = _provinces
            .Select((p, i) => new { Province = p, Index = i })
            .Where(x => x.Province.Ideology == _old)
            .Select(x => x.Index)
            .ToList();

        if (targetIndexes.Count == 0)
            return;

        HashSet<int> usedIndexes = new HashSet<int>();

        for (int i = 0, index; i < count; i++)
        {
            do { index = targetIndexes[Random.Range(0, targetIndexes.Count)]; }
            while (!usedIndexes.Add(index));

            _provinces[index].SetIdeology(_new);
        }
    }

    private void SwapIdeology(Ideology _first, Ideology _second)
    {
        var targetFirstIndexes = _provinces
            .Select((p, i) => new { Province = p, Index = i })
            .Where(x => x.Province.Ideology == _first)
            .Select(x => x.Index)
            .ToList();

        var targetSecondIndexes = _provinces
            .Select((p, i) => new { Province = p, Index = i })
            .Where(x => x.Province.Ideology == _second)
            .Select(x => x.Index)
            .ToList();

        if (targetFirstIndexes.Count == 0 || targetSecondIndexes.Count == 0)
            return;

        int count = Mathf.Max(1, Mathf.Min(targetFirstIndexes.Count, targetSecondIndexes.Count) / _provinceSwapCoef);

        HashSet<int> usedIndexes = new HashSet<int>();

        for (int i = 0, firstIndex, secondIndex; i < count; i++)
        {
            do { firstIndex = targetFirstIndexes[Random.Range(0, targetFirstIndexes.Count)]; }
            while (!usedIndexes.Add(firstIndex));

            do { secondIndex = targetSecondIndexes[Random.Range(0, targetSecondIndexes.Count)]; }
            while (!usedIndexes.Add(secondIndex));

            _provinces[firstIndex].SetIdeology(_second);
            _provinces[secondIndex].SetIdeology(_first);
        }
    }

    private void DistributeColorsOnMap(Vector2Int tendence)
    {
        int newAuProvinceCount = InterpolateValue(tendence.x - _currentTendence.x, _minAxisValue, _maxAxisValue, 0, _maxAuToDemProvinceCount);
        int newConProvinceCount = InterpolateValue(tendence.y - _currentTendence.y, _minAxisValue, _maxAxisValue, 0, _maxConToSciProvinceCount);

        // process new tendence
        int dif = newAuProvinceCount - _curAuProvinceCount;
        if (dif > 0)
        {
            // add Authoritarianism 
            ChangeIdeology(Ideology.Democracy, Ideology.Authoritarianism, dif);
        }
        else
        {
            // add Democracy
            ChangeIdeology(Ideology.Authoritarianism, Ideology.Democracy, -dif);
        }

        dif = newConProvinceCount - _curConProvinceCount;
        if (dif > 0)
        {
            // add Conspiracy 
            ChangeIdeology(Ideology.Science, Ideology.Conspiracy, dif);
        }
        else if (dif < 0)
        {
            // add Science
            ChangeIdeology(Ideology.Conspiracy, Ideology.Science, -dif);
        }

        // make map more dynamic
        if (Random.value > 0.5f)
            SwapIdeology(Ideology.Authoritarianism, Ideology.Conspiracy);
        else
            SwapIdeology(Ideology.Authoritarianism, Ideology.Science);

        if (Random.value > 0.5f)
            SwapIdeology(Ideology.Democracy, Ideology.Conspiracy);
        else
            SwapIdeology(Ideology.Democracy, Ideology.Science);
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
