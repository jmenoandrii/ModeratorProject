using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private GameObject _appWindow;

    private void Awake()
    {
        // protection
        if (_appWindow == null)
            _appWindow = gameObject.GetComponent<GameObject>();
    }

    public void ExitApp()
    {
        _appWindow.SetActive(false);
    }

    public void OpenApp()
    {
        _appWindow.SetActive(true);
    }
}
