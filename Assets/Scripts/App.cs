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
    public bool IsContainerActive { get {  return _appContainer.activeSelf; } }
    public bool IsAppActive { get {  return _appWindow.activeSelf; } }

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
    public virtual void CloseApp()
    {
        GlobalEventManager.CallOnAppClose(this);

        HideApp();

        _appWindow.SetActive(false);
    }

    //When we first open app
    public virtual void OpenApp()
    {
        _appWindow.transform.SetAsLastSibling();
        
        if (IsAppActive) 
        {
            if (!IsContainerActive) 
                ShowApp();

            return;
        }

        _appWindow.SetActive(true);

        _appWindow.transform.SetLocalPositionAndRotation(_initialPosition, _initialRotation);

        GlobalEventManager.CallOnAppOpen(this);
        ShowApp();
    }

    public virtual void HideApp()
    {
        if (!IsContainerActive) return;

        _appContainer.SetActive(false);
    }

    public virtual void ShowApp()
    {
        _appWindow.transform.SetAsLastSibling();
        
        if (IsContainerActive) return;

        _appContainer.SetActive(true);
    }
}
