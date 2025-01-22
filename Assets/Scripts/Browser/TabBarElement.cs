using UnityEngine;
using UnityEngine.UI;

public class TabBarElement : BarElement
{
    [Header("Tab Parameters")]
    [SerializeField] private GameObject _page;
    [SerializeField] private bool _isCurrentPage;
    [SerializeField] private Image _background;
    public GameObject Page {  get { return _page; } }
    public bool IsCurrentPage { get { return _isCurrentPage; } }
    
    private void Awake()
    {
        if (_background == null)
            Debug.LogError($"{gameObject.name} doesn't have the Image component.\n\t" +
                "Set '_background' in the inspector");

        if (_isCurrentPage)
            _background.color = TabBar.CurrentTabColor;
    }

    public void SetAsCurrent()
    {
        _isCurrentPage = true;
        _background.color = TabBar.CurrentTabColor;
    }

    public void SetAsCommon()
    {
        _isCurrentPage = false;
        _background.color = TabBar.CommonTabColor;
    }

    public void SetPage(GameObject page)
    { 
        _page = page; 
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }

    public void SetTitle(string title)
    {
        _title.text = title;
    }

    public void ShowTab()
    {
        if (IsCurrentPage) return;

        GlobalEventManager.CallOnShowPage(this);
    }

    public void CloseTab()
    {
        gameObject.SetActive(false);
        GlobalEventManager.CallOnClosePage(this);
    }
}
