using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [Header("Window Parameters")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _title;
    [Header("Window Elements")]
    [SerializeField] private Image _appIcon;
    [SerializeField] private TMP_Text _appTitle;
    [SerializeField] private GameObject _appContainer;
    [SerializeField] private GameObject _appWindow;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private bool _isInitialized;

    public Sprite Icon { get { return _icon; } }
    public string Title { get { return _title; } }
    public bool IsActive { get {  return _appContainer.activeSelf; } }

    private void OnEnable()
    {
        if (!_isInitialized) 
        {
            if (_appIcon != null)
                _appIcon.sprite = _icon;
            if (_appTitle != null)
                _appTitle.SetText(_title);

            // protection
            if (_appWindow == null)
                _appWindow = gameObject;

            _initialPosition = _appWindow.transform.localPosition;
            _initialRotation = _appWindow.transform.localRotation;

            _isInitialized = true;
        }
    }

    //When we fully close app
    public void CloseApp()
    {
        GlobalEventManager.CallOnAppClose(this);

        HideApp();

        _appWindow.SetActive(false);
    }

    //When we first open app
    public void OpenApp()
    {
        if (_appWindow.activeSelf) return;

        _appWindow.SetActive(true);

        _appWindow.transform.SetLocalPositionAndRotation(_initialPosition, _initialRotation);

        GlobalEventManager.CallOnAppOpen(this);
        ShowApp();
    }

    public void HideApp()
    {
        if (!IsActive) return;

        _appContainer.SetActive(false);
    }

    public void ShowApp()
    {
        if (IsActive) return;

        _appWindow.transform.SetAsLastSibling();
        _appContainer.SetActive(true);
    }
}
