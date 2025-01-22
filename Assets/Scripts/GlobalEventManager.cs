using System;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    // ***** App events *****
    public static event Action<App> OnAppOpen;
    public static event Action<App> OnAppClose;

    public static void CallOnAppOpen(App app)
    {
        OnAppOpen?.Invoke(app);
    }

    public static void CallOnAppClose(App app)
    {
        OnAppClose?.Invoke(app);
    }

    // ***** Browser events *****
    public static event Action<PageParameter> OnPageOpen;
    public static event Action<GameObject> OnPageOpenCreated;
    public static event Action<PageParameter> OnNewPage;
    public static event Action<PageParameter, GameObject> OnNewPageCreated;
    public static event Action<TabBarElement> OnShowPage;
    public static event Action<TabBarElement> OnClosePage;
    public static event Action OnCloseBrowser;
    public static event Action OnClearTabBar;

    public static void CallOnPageOpen(PageParameter pageParameter)
    {
        OnPageOpen?.Invoke(pageParameter);
    }

    public static void CallOnPageOpenCreated(GameObject page)
    { 
        OnPageOpenCreated?.Invoke(page); 
    }

    public static void CallOnNewPage(PageParameter defaultPageParameter)
    {
        OnNewPage?.Invoke(defaultPageParameter);
    }

    public static void CallOnNewPageCreated(PageParameter defaultPageParameter, GameObject page)
    {
        OnNewPageCreated?.Invoke(defaultPageParameter, page);
    }

    public static void CallOnShowPage(TabBarElement tab)
    {
        OnShowPage?.Invoke(tab);
    }

    public static void CallOnClosePage(TabBarElement tab)
    {
        OnClosePage?.Invoke(tab);
    }

    public static void CallOnCloseBrowser()
    {
        OnCloseBrowser?.Invoke();
    }

    public static void CallOnClearTabBar()
    {
        OnClearTabBar?.Invoke();
    }
}
