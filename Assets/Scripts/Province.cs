using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Province : MonoBehaviour
{
    private Image _fill;
    public MapManager.Ideology Ideology { get; private set; }

    private void Awake()
    {
        _fill = GetComponent<Image>();
    }

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