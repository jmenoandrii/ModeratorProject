using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Province : MonoBehaviour
{
    [SerializeField]
    private Image _fill;
    private int _ideologyId;
    private bool _isEmpty = true;

    public bool IsEmpty()
    {
        return _isEmpty;
    }

    public int GetOwner()
    {
        return _ideologyId;
    }

    public void SetIdeology(MapManager.Ideology ideology)
    {
        if (ideology == null)
        {
            _fill.color = new Color(0,0,0,0);
            _isEmpty = true;
            return;
        }

        _isEmpty = false;

        _ideologyId = ideology.id;

        _fill.color = ideology.color;
    }
}