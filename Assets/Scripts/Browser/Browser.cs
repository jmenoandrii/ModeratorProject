using UnityEngine;

public class Browser : App
{
    [Header("Browser Components")]
    [SerializeField] private GameObject _contentContainer;
    [SerializeField] private GameObject _currentPage;
    [SerializeField] private NewPageButton _newPageButton;

    private void Start()
    {
        // protection
        if (_contentContainer == null)
        {
            Debug.LogError("_contentContainer is not assigned!");
        }

        if (_currentPage == null)
        {
            Debug.LogError("_currentPage is not assigned!");
        }

        if (_newPageButton == null)
        {
            Debug.LogError("_newPageButton is not assigned!");
        }
    }

    private void Awake()
    {
        GlobalEventManager.OnPageOpen += SpawnOpenedPage;
        GlobalEventManager.OnNewPage += OpenMainPage;
        GlobalEventManager.OnShowPage += ShowPage;
        GlobalEventManager.OnClosePage += ClosePage;
        GlobalEventManager.OnCloseBrowser += CloseApp;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnPageOpen -= SpawnOpenedPage;
        GlobalEventManager.OnNewPage -= OpenMainPage;
        GlobalEventManager.OnShowPage -= ShowPage;
        GlobalEventManager.OnClosePage -= ClosePage;
        GlobalEventManager.OnCloseBrowser -= CloseApp;
    }

    private GameObject SpawnPage(PageParameter pageParameter, bool isForNewPageCalling = false)
    {
        GameObject newPage = Instantiate(pageParameter.prefab);

        RectTransform rectTransform = newPage.GetComponent<RectTransform>();

        rectTransform.SetParent(_contentContainer.transform, false);

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;

        if (!isForNewPageCalling)
            GlobalEventManager.CallOnPageOpenCreated(newPage);
        else
            GlobalEventManager.CallOnNewPageCreated(pageParameter, newPage);

        return newPage;
    }

    private void SpawnOpenedPage(PageParameter pageParameter)
    {
        GameObject newPage = SpawnPage(pageParameter);

        DeleteCurrentPage(newPage);
    }

    private void DeleteCurrentPage(GameObject newPage)
    {
        Destroy(_currentPage);
        _currentPage = newPage;
    }

    private void DeletePage(GameObject page)
    {
        Destroy(page);
    }

    private void DeleteAllPage()
    {
        foreach (Transform child in _contentContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClosePage(TabBarElement tab)
    {
        if (tab.IsCurrentPage)
        {
            DeleteCurrentPage(null);
        }
        else
        {
            DeletePage(tab.Page);
        }
    }

    private void OpenMainPage(PageParameter defaultPageParameter)
    {
        GameObject newPage = SpawnPage(defaultPageParameter, true);

        _currentPage?.SetActive(false);
        _currentPage = newPage;
        newPage.SetActive(true);
    }

    private void ShowPage(TabBarElement tab)
    {
        _currentPage?.SetActive(false);

        _currentPage = tab.Page;
        tab.Page.gameObject.SetActive(true);
    }

    private void ResetBrowser()
    {
        DeleteAllPage();
        GlobalEventManager.CallOnClearTabBar();

        _newPageButton.OpenPage();
    }

    public override void CloseApp()
    {
        ResetBrowser();

        base.CloseApp();
    }
}
