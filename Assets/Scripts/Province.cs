using UnityEngine;
using UnityEngine.UI;

public class Province : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _fill;
    public MapManager.Ideology Ideology { get; private set; }

    public void SetIdeology(MapManager.Ideology ideology)
    {
        Ideology = ideology;

        switch (ideology)
        {
            case MapManager.Ideology.Authoritarianism:
                _fill.color = MapManager.AuthoritarianismColor;
                break;
            case MapManager.Ideology.Democracy:
                _fill.color = MapManager.DemocracyColor;
                break;
            case MapManager.Ideology.Conspiracy:
                _fill.color = MapManager.ConspiracyColor;
                break;
            case MapManager.Ideology.Science:
                _fill.color = MapManager.ScienceColor;
                break;
        }
    }
}

/*[SerializeField] private string _name;
    [SerializeField] private Continent _continent;*/
/*private Vector2 _axisState;
    private Color _currentColor;*/
/*public enum Continent
    {
        Asia,
        Africa,
        Europe,
        NorthAmerica,
        SouthAmerica,
        Australia,
        Antarctica
    }*/