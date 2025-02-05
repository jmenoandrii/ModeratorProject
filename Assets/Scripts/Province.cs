using UnityEngine;

public class Province : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Continent _continent;
    private Vector2 _axisState;
    private Color _currentColor;

    public enum Continent
    {
        Eroup,
        Asia,
        SoulsAme
    }
}
