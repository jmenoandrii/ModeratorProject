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
        int zeroValue = InterpolateValue(0, _minAxisValue, _maxAxisValue, 0, _maxAuToDemProvinceCount);
        _currentTendence.Set(zeroValue, zeroValue);
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

        _provinces = _provinces.OrderBy(x => Random.value).ToList();

        Ideology[] ideologies = { Ideology.Authoritarianism, Ideology.Democracy, Ideology.Conspiracy, Ideology.Science };

        for (int i = 0, k = 0; i < _provinces.Count; i++, k++)
        {
            if (k == ideologies.Length)
                k = 0;

            _provinces[i].SetIdeology(ideologies[k]);
        }

        int provincesPerIdeology = _provinces.Count / 4;
        _maxAuToDemProvinceCount = provincesPerIdeology * 2;
        _maxConToSciProvinceCount = provincesPerIdeology * 2;
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
        int difAuToDem = newAuProvinceCount - _curAuProvinceCount;
        if (difAuToDem > 0)
        {
            // add Authoritarianism 
            ChangeIdeology(Ideology.Democracy, Ideology.Authoritarianism, difAuToDem);
        }
        else
        {
            // add Democracy
            ChangeIdeology(Ideology.Authoritarianism, Ideology.Democracy, -difAuToDem);
        }

        int difConToSci = newConProvinceCount - _curConProvinceCount;
        if (difConToSci > 0)
        {
            // add Conspiracy 
            ChangeIdeology(Ideology.Science, Ideology.Conspiracy, difConToSci);
        }
        else if (difConToSci < 0)
        {
            // add Science
            ChangeIdeology(Ideology.Conspiracy, Ideology.Science, -difConToSci);
        }

        // make map more dynamic
        if (difAuToDem != 0 || difConToSci != 0)
        {
            if (Random.value > 0.5f)
                SwapIdeology(Ideology.Authoritarianism, Ideology.Conspiracy);
            else
                SwapIdeology(Ideology.Authoritarianism, Ideology.Science);

            if (Random.value > 0.5f)
                SwapIdeology(Ideology.Democracy, Ideology.Conspiracy);
            else
                SwapIdeology(Ideology.Democracy, Ideology.Science);
        }

        _curAuProvinceCount = newAuProvinceCount;
        _curConProvinceCount = newConProvinceCount;
    }

    public enum Ideology
    {
        Neutral,
        Authoritarianism,
        Democracy,
        Conspiracy,
        Science
    }
}
