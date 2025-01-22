using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarElement : MonoBehaviour
{
    [SerializeField] protected Image _icon;
    [SerializeField] protected TextMeshProUGUI _title;
    public bool IsActive { get { return gameObject.activeSelf; } }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
