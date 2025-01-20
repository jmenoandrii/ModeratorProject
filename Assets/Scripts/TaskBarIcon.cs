using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskBarIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private RectTransform _clickZone;
    private App _app;
    public bool IsActive { get; private set; }

    private void Awake()
    {
        GlobalEventManager.OnAppClose += Hide;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAppClose -= Hide;
    }

    private void Update()
    {
        // Clicking
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(_clickZone, mousePosition))
            {
                if (_app.IsActive)
                    _app.HideApp();
                else 
                    _app.ShowApp();
            }
        }
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

    private void Hide(App app)
    {
        if (_app != null && _app == app)
        {
            Hide();
            GlobalEventManager.SendSortTaskBar();
        }
    }
}
