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

    // ***** Posts events *****
    public static event Action OnLoadNextPost;
    public static event Action OnInitAdminPost;
    public static event Action OnNoPostsFound;
    public static event Action<AdminPostsLoader.AdminPost> OnSendAdminPost;
    public static event Action<AdminPostsLoader.Impact> OnSendImpact;
    public static event Action<int> OnSendLeftPostCount;
    public static event Action<float> OnUpdateTimerOfLoadPosts;

    // ***** Game events *****
    public static event Action<EndingSummary> OnEndGame;
    public static event Action OnEndInitiate;

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

    public static void CallOnEndGame(EndingSummary summary)
    {
        OnEndGame?.Invoke(summary);
    }

    public static void CallOnEndInitiate()
    {
        OnEndInitiate?.Invoke();
    }

    public static void CallOnLoadNextPost()
    {
        OnLoadNextPost?.Invoke();
    }

    public static void CallOnInitAdminPost()
    {
        OnInitAdminPost?.Invoke();
    }

    public static void CallOnNoPostsFound()
    {
        OnNoPostsFound?.Invoke();
    }

    public static void CallOnSendAdminPost(AdminPostsLoader.AdminPost post)
    {
        OnSendAdminPost?.Invoke(post);
    }

    public static void CallOnSendImpact(AdminPostsLoader.Impact impact)
    {
        OnSendImpact?.Invoke(impact);
    }

    public static void CallOnSendLeftPostCount(int count)
    {
        OnSendLeftPostCount?.Invoke(count);
    }

    public static void CallOnUpdateTimerOfLoadPosts(float time)
    {
        OnUpdateTimerOfLoadPosts?.Invoke(time);
    }
}
