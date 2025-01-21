using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskBarElement : BarElement
{

    [SerializeField] private App _app;

    private void Awake()
    {
        GlobalEventManager.OnAppClose += HandleAppClose;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAppClose -= HandleAppClose;
    }

    public void SetApp(App app)
    {
        _app = app;
        _icon.sprite = app.Icon;
        _title.text = app.Title;
    }

    public App GetApp() 
    { 
        return _app; 
    }

    public void Click()
    {
        if (_app == null)
        {
            Debug.LogError("Error: _app is not assigned!", this);
            return;
        }

        _app.ShowApp();
    }

    private void HandleAppClose(App app)
    {
        if (_app != null && _app == app)
        {
            Hide();
        }
    }
}
