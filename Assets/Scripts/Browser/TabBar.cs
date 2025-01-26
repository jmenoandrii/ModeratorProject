using UnityEngine;
using System.Linq;

public class TabBar : MonoBehaviour
{
    [SerializeField] private NewPageButton _newPageButton;
    [SerializeField] private TabBarElement[] _elementList;
    public static int CountOfActiveElement { get; private set; }
    public static int MaxOfActiveElement { get; private set; }
    private TabBarElement _currentTab;

    // static tab colors
    private static Color _commonTabColor = new(0.858f, 0.858f, 0.858f, 1f);
    public static Color CommonTabColor { get { return _commonTabColor; } }
    private static Color _currentTabColor = new(1f, 1f, 1f, 1f);
    public static Color CurrentTabColor { get { return _currentTabColor; } }

    private void Start()
    {
        CountOfActiveElement = 0;
        MaxOfActiveElement = _elementList.Length;

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
            if (element.IsActive)
            {
                CountOfActiveElement++;
            }
        }
        if (currentTabCount > 1)
            Debug.LogError("There are more than one current tab in the TabBar");
        else if (currentTabCount < 1)
            Debug.LogError("There are no current tab in the TabBar");

        if (_newPageButton == null)
        {
            Debug.LogError("_newPageButton is not assigned!");
        }
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
        CountOfActiveElement++;

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
        _newPageButton.transform.SetAsLastSibling();
    }

    private void TabClosingHandling(TabBarElement tab)
    {
        CountOfActiveElement--;

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
        CountOfActiveElement = 0;

        foreach (TabBarElement element in _elementList)
        {
            element.Hide();
        }
    }
}
