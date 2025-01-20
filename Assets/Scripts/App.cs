using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [Header("Window Parameters")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _title;
    [Header("Window Elements")]
    [SerializeField] private GameObject _appWindow;
    [SerializeField] private Image _appIcon;
    [SerializeField] private TextMeshProUGUI _appTitle;

    public Sprite Icon { get { return _icon; } }
    public string Title { get { return _title; } }

    private void Awake()
    {
        _appIcon.sprite = _icon;
        _appTitle.text = _title;
    }

    private void Start()
    {
        // protection
        if (_appWindow == null)
            _appWindow = gameObject.GetComponent<GameObject>();
    }

    public void CloseApp()
    {
        if (!_appWindow.activeSelf) return;

        GlobalEventManager.SendAppClose(this);
        _appWindow.SetActive(false);
    }

    public void OpenApp()
    {
        if (_appWindow.activeSelf) return;

        GlobalEventManager.SendAppToTaskBarIcon(this);
        _appWindow.SetActive(true);
    }

    public void HideApp()
    {
        if (!_appWindow.activeSelf) return;

        _appWindow.SetActive(false);
    }
}
