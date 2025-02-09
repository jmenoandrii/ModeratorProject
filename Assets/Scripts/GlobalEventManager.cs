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
    public static event Action<AdminPostLoader.AdminPost> OnSendAdminPost;
    public static event Action<AdminPostLoader.Impact> OnSendImpact;
    public static event Action<int> OnSendLeftPostCount;
    public static event Action<float> OnUpdateTimerOfLoadPosts;
    public static event Action<MailUI> OnAdminPostMailAdded;

    // ***** Mail *****
    public static event Action<Mail> OnAddNewMail;
    public static event Action<Mail, GameObject> OnShowFullMail;
    public static event Action OnHideFullMail;
    public static event Action<Mail> OnAcceptQuest;

    // ***** Game events *****
    public static event Action<EndingSummary> OnEndGame;
    public static event Action OnEndInitiate;
    public static event Action<int> OnChangeCryptoWallet; 
    public static event Action<int, int, int, int, int> OnChangeAxis; 
    public static event Action OnInitWorldIndex; 
    public static event Action<EndingCurtain[]> OnEndingReset;

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

    public static void CallOnChangeCryptoWallet(int amount)
    {
        OnChangeCryptoWallet?.Invoke(amount);
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

    public static void CallOnSendAdminPost(AdminPostLoader.AdminPost post)
    {
        OnSendAdminPost?.Invoke(post);
    }

    public static void CallOnSendImpact(AdminPostLoader.Impact impact)
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

    public static void CallOnAdminPostMailAdded(MailUI mailUI)
    {
        OnAdminPostMailAdded?.Invoke(mailUI);
    }

    public static void CallOnAddNewMail(Mail mail)
    {
        OnAddNewMail?.Invoke(mail);
    }

    public static void CallOnAcceptQuest(Mail mail)
    {
        OnAcceptQuest?.Invoke(mail);
    }

    public static void CallOnShowFullMail(Mail mail, GameObject shortMailObj)
    {
        OnShowFullMail?.Invoke(mail, shortMailObj);
    }

    public static void CallOnHideFullMail()
    {
        OnHideFullMail?.Invoke();
    }

    public static void CallOnChangeAxis(int _conspiracyToScienceValue, int _conservatismToProgressValue, int _communismToCapitalismValue, int _authoritarianismToDemocracyValue, int _pacifismToMilitarismValue)
    {
        OnChangeAxis?.Invoke(_conspiracyToScienceValue, _conservatismToProgressValue, _communismToCapitalismValue, _authoritarianismToDemocracyValue, _pacifismToMilitarismValue);
    }

    public static void CallOnInitWorldIndex()
    {
        OnInitWorldIndex?.Invoke();
    }

    public static void CallOnEndingReset(EndingCurtain[] endingCurtains)
    {
        OnEndingReset?.Invoke(endingCurtains);
    }
}
