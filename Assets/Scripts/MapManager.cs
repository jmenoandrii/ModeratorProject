using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Range(2, 10)] private int _provinceSwapCoef;
    [SerializeField] private int _minAxisValue;
    [SerializeField] private int _maxAxisValue;
    [SerializeField] private List<Province> _provinces;
    private int _totalProvinces;
    [SerializeField] private List<Ideology> _ideologies;

    [SerializeField] private GameObject _ideologyPanelPrefab;
    [SerializeField] private Transform _ideologyPanelParent;

    private HashSet<Province> _selectedProvinces = new HashSet<Province>();

    private void Awake()
    {
        GlobalEventManager.OnChangeAxis += ChangeAxisHandler;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnChangeAxis -= ChangeAxisHandler;
    }

    private void OnEnable()
    {
        GlobalEventManager.CallOnInitWorldIndex();
    }

    private void Start()
    {
        _totalProvinces = _provinces.Count;
        InitDistribution();
    }

    private void InitDistribution()
    {
        // protection
        if (_provinces.Count == 0)
        {
            Debug.LogError($"ERR[{gameObject.name}]: _provinces lsit is empty");
            return;
        }
    }

    private void ChangeAxisHandler(int _conspiracyToScienceValue, int _conservatismToProgressValue, int _communismToCapitalismValue, int _authoritarianismToDemocracyValue, int _pacifismToMilitarismValue)
    {
        List<int> tendence = new() {_conspiracyToScienceValue, _conservatismToProgressValue, _communismToCapitalismValue, _authoritarianismToDemocracyValue, _pacifismToMilitarismValue};
        DistributeColorsOnMap(tendence);
    }

    private void CreateIdeologyPanel(Ideology ideology)
    {
        ideology.panel = Instantiate(_ideologyPanelPrefab, _ideologyPanelParent).GetComponent<MapIdeologyPanel>();
        ideology.panel.Initialize(ideology.name, ideology.countOfProvince, ideology.icon);
    }

    private void DistributeColorsOnMap(List<int> tendence)
    {
        int sumIdeologies = tendence.Sum(x => Mathf.Abs(x));

        if (sumIdeologies == 0)
            return;

        int i = 0;
        foreach (int impact in tendence)
        {
            if (i + 1 >= _ideologies.Count) 
                break;

            int nPopularity = GetNegativeValue(impact);
            int pPopularity = GetPositiveValue(impact);

            Ideology leftIdeology = _ideologies[i], rightIdeology = _ideologies[i + 1];

            leftIdeology.popularity = nPopularity;
            rightIdeology.popularity = pPopularity;

            float leftCoefficient = Mathf.Clamp01((float)nPopularity / (float)_maxAxisValue);
            float rightCoefficient = Mathf.Clamp01((float)pPopularity / (float)_maxAxisValue);

            leftIdeology.percent = (float)nPopularity / (float)sumIdeologies * leftCoefficient;
            rightIdeology.percent = (float)pPopularity / (float)sumIdeologies * rightCoefficient;

            int leftCountOfProvince = leftIdeology.countOfProvince;
            int rightCountOfProvince = rightIdeology.countOfProvince;

            leftIdeology.countOfProvince = (int)(_totalProvinces * leftIdeology.percent);
            rightIdeology.countOfProvince = (int)(_totalProvinces * rightIdeology.percent);

            leftIdeology.diffOfProvince = leftIdeology.countOfProvince - leftCountOfProvince;
            rightIdeology.diffOfProvince = rightIdeology.countOfProvince - rightCountOfProvince;

            if (leftIdeology.panel == null)
            {
                if (leftIdeology.diffOfProvince > 0)
                {
                    CreateIdeologyPanel(leftIdeology);
                }
            }
            else
            {
                leftIdeology.panel.UpdateCounterText(leftIdeology.countOfProvince);
            }

            if (rightIdeology.panel == null)
            {
                if (rightIdeology.diffOfProvince > 0)
                {
                    CreateIdeologyPanel(rightIdeology);
                }
            }
            else
            {
                rightIdeology.panel.UpdateCounterText(rightIdeology.countOfProvince);
            }

            i += 2;
        }

        foreach (Ideology ideology in _ideologies)
        {
            if (ideology.diffOfProvince >= 0)
                continue;

            SetRandomEmptyProvinces(ideology.id, Mathf.Abs(ideology.diffOfProvince));
        }

        foreach (Ideology ideology in _ideologies)
        {
            if (ideology.diffOfProvince <= 0)
                continue;

            List<Province> provinces = GetRandomEmptyProvinces(ideology.diffOfProvince);

            foreach (var province in provinces)
            {
                province.SetIdeology(ideology);
            }
        }
    }

    public List<Province> GetRandomEmptyProvinces(int count)
    {
        _selectedProvinces = new();

        // Filter the list to include only empty and unselected provinces
        List<Province> availableProvinces = _provinces
            .Where(province => province.IsEmpty() && !_selectedProvinces.Contains(province))
            .ToList();

        // Ensure we don't request more provinces than available
        if (availableProvinces.Count < count)
        {
            Debug.LogWarning("Not enough available provinces!");
            count = availableProvinces.Count; // Adjust count to the available number
        }

        // Shuffle the list and select the required number of provinces
        List<Province> randomProvinces = availableProvinces
            .OrderBy(p => Random.value)
            .Take(count)
            .ToList();

        // Add selected provinces to the set to prevent future duplicates
        _selectedProvinces.UnionWith(randomProvinces);

        return randomProvinces;
    }

    public void SetRandomEmptyProvinces(int owner, int countToRemove)
    {
        List<Province> provincesOwned = _provinces.FindAll(p => p.GetOwner() == owner && !p.IsEmpty());

        if (provincesOwned.Count <= countToRemove)
        {
            foreach (Province province in provincesOwned)
            {
                province.SetIdeology(null);
            }
        }
        else {
            for (int i = 0; i < countToRemove; i++)
            {
                int randomIndex = Random.Range(0, provincesOwned.Count);
                provincesOwned[randomIndex].SetIdeology(null);

                provincesOwned.RemoveAt(randomIndex);
            }
        }
    }

    private int GetNegativeValue(int value)
    {
        return Mathf.Max(-value, 0);
    }

    private int GetPositiveValue(int value)
    {
        return Mathf.Max(value, 0);
    }

    [System.Serializable]
    public class Ideology
    {
        public int id;
        public string name;
        public Sprite icon;
        public Color color;
        public float percent;
        public int popularity;
        public int countOfProvince;
        public int diffOfProvince;
        public MapIdeologyPanel panel = null;
    }
}
