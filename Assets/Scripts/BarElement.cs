using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarElement : MonoBehaviour
{
    [SerializeField] protected Image _icon;
    [SerializeField] protected TextMeshProUGUI _title;
    public bool IsActive { get; private set; }

    public void Show()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }
}
