using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

public class TabBar : MonoBehaviour
{
    [SerializeField] private TabBarElement[] _elementList;
    private TabBarElement _currentTab;
    private bool _isCurrentTabChanging = false;

    // static tab colors
    private static Color _commonTabColor = new Color(0.314f, 0.314f, 0.314f, 1f);
    public static Color CommonTabColor { get { return _commonTabColor; } }
    private static Color _currentTabColor = new Color(0.196f, 0.196f, 0.196f, 1f);
    public static Color CurrentTabColor { get { return _currentTabColor; } }

    private void Start()
    {
        // protection
        int currentTabCount = 0;
        foreach (TabBarElement element in _elementList)
        {
            if (element.IsCurrentPage)
            { 
                currentTabCount++;
                // set
                _currentTab = element;
            }
        }
        if (currentTabCount > 1)
            Debug.LogError("There are more than one current tab in the TabBar");
        else if (currentTabCount < 1)
            Debug.LogError("There are no current tab in the TabBar");
    }

    private void Awake()
    {
        GlobalEventManager.OnPageOpen += ConfigureCurrentTab;
        GlobalEventManager.OnPageOpenCreated += BindPageToCurrentTab;
        GlobalEventManager.OnNewPageCreated += AddBarElement;
        GlobalEventManager.OnShowPage += ChangeCurrentTab;
        GlobalEventManager.OnClosePage += TabClosingHandling;
        GlobalEventManager.OnClearTabBar += ClearTabBar;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnPageOpen -= ConfigureCurrentTab;
        GlobalEventManager.OnPageOpenCreated -= BindPageToCurrentTab;
        GlobalEventManager.OnNewPageCreated -= AddBarElement;
        GlobalEventManager.OnShowPage -= ChangeCurrentTab;
        GlobalEventManager.OnClosePage -= TabClosingHandling;
        GlobalEventManager.OnClearTabBar -= ClearTabBar;
    }

    private void ChangeCurrentTab(TabBarElement newTab)
    {
        _currentTab.SetAsCommon();
        _currentTab = newTab;
        newTab.SetAsCurrent();
    }

    private void ConfigureCurrentTab(PageParameter pageParameter)
    {
        _currentTab.SetIcon(pageParameter.icon);
        _currentTab.SetTitle(pageParameter.title);
    }

    private void BindPageToCurrentTab(GameObject page)
    {
        _currentTab.SetPage(page);
    }

    private void AddBarElement(PageParameter defaultPageParameter, GameObject newPage)
    {
        foreach (TabBarElement element in _elementList)
        {
            if (!element.IsActive)
            {
                element.Show();
                ChangeCurrentTab(element);
                ConfigureCurrentTab(defaultPageParameter);
                element.transform.SetAsLastSibling();
                break;
            }
        }
        BindPageToCurrentTab(newPage);
    }

    private void TabClosingHandling(TabBarElement tab)
    {
        if (!tab.IsCurrentPage) return; // if it wasn't current page, TabBar have no need to do something

        foreach (TabBarElement element in _elementList.Reverse())
        {
            if (element.IsActive)
            {
                GlobalEventManager.CallOnShowPage(element);
                return;
            }
        }

        // if all tabs are closed
        GlobalEventManager.CallOnCloseBrowser();
    }

    private void ClearTabBar()
    {
        foreach(TabBarElement element in _elementList)
        {
            element.Hide();
        }
    }
}
