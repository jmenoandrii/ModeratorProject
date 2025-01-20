using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private GameObject _appWindow;
    private bool _isFullScreen = false;

    private void Awake()
    {
        // protection
        if (_appWindow == null)
            _appWindow = gameObject.GetComponent<GameObject>();
    }

    public void HideApp()
    {
        _appWindow.SetActive(false);
    }

    public void OpenApp()
    {
        _appWindow.SetActive(true);
    }

    public void FullScreenApp()
    {
        _isFullScreen = !_isFullScreen;

    }

    public void NormalScreenApp()
    {
        _isFullScreen = !_isFullScreen;

    }
}
