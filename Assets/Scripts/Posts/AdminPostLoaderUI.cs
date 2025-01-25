using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdminPostLoaderUI : MonoBehaviour
{
    [SerializeField] private PostUI _postUI;
    [SerializeField] private GameObject _loading;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private GameObject _noFoundPosts;
    [SerializeField] private TMP_Text _counter;
    [SerializeField] private RectTransform _valueContainer;
    [SerializeField] private RectTransform _acceptPosition;
    [SerializeField] private RectTransform _denyPosition;
    private bool isBooted = false;
    [Header("Timer")]
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private float _postLoadingTime;
    [SerializeField] private float _iconShownTime;
    private int[] _impactsValue = new int[12];
    [SerializeField] private ValueIcon[] _valueIcons = new ValueIcon[12];

    private void Awake()
    {
        GlobalEventManager.OnSendAdminPost += UpdatePost;
        GlobalEventManager.OnSendLeftPostCount += SetLestPostCount;
        GlobalEventManager.OnUpdateTimerOfLoadPosts += UpdateTimer;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnSendAdminPost -= UpdatePost;
        GlobalEventManager.OnSendLeftPostCount -= SetLestPostCount;
        GlobalEventManager.OnUpdateTimerOfLoadPosts -= UpdateTimer;
    }

    private void OnEnable()
    {
        GlobalEventManager.CallOnInitAdminPost();
    }

    private void UpdateTimer(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string timeStr = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        _timer.SetText(timeStr);
    }

    private void UpdatePost(AdminPostsLoader.AdminPost post)
    {
        if (post == null)
        {
            OnNoPostsFound();
            return;
        }

        OnPostsFound();
        _postUI.SetPostData(post.nickname, post.content, post.date);
        _postUI.acceptImpact = post.acceptImpact;
        _postUI.denyImpact = post.denyImpact;

        if (isBooted) 
            StartCoroutine(WaitToLoadPosts());
        isBooted = true;
    }

    public void NextPost(bool isAccept)
    {
        AdminPostsLoader.Impact impact = isAccept ? _postUI.acceptImpact : _postUI.denyImpact;

        GlobalEventManager.CallOnSendImpact(impact);

        _valueContainer.position = isAccept ? _acceptPosition.position : _denyPosition.position;
        ShowIconsValue(impact);

        GlobalEventManager.CallOnLoadNextPost();
    }

    private IEnumerator WaitToLoadPosts()
    {
        _loading.SetActive(true);
        _postUI.gameObject.SetActive(false);

        Button[] buttons = _buttons.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }


        yield return new WaitForSeconds(_postLoadingTime);


        _loading.SetActive(false);
        _postUI.gameObject.SetActive(true);

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    private void ShowIconsValue(AdminPostsLoader.Impact impact)
    {
        _impactsValue[0] = GetNegativeValue(impact.conspiracyToScienceValue);
        _impactsValue[1] = GetPositiveValue(impact.conspiracyToScienceValue);
        _impactsValue[2] = GetNegativeValue(impact.conservatismToProgressValue);
        _impactsValue[3] = GetPositiveValue(impact.conservatismToProgressValue);
        _impactsValue[4] = GetNegativeValue(impact.communismToCapitalismValue);
        _impactsValue[5] = GetPositiveValue(impact.communismToCapitalismValue);
        _impactsValue[6] = GetNegativeValue(impact.authoritarianismToDemocracyValue);
        _impactsValue[7] = GetPositiveValue(impact.authoritarianismToDemocracyValue);
        _impactsValue[8] = GetNegativeValue(impact.pacifismToMilitarismValue);
        _impactsValue[9] = GetPositiveValue(impact.pacifismToMilitarismValue);
        _impactsValue[10] = impact.profit;
        _impactsValue[11] = impact.victims;

        StartCoroutine(StartShowingIcons());
    }

    private int GetNegativeValue(int value)
    {
        return Mathf.Max(-value, 0);
    }

    private int GetPositiveValue(int value)
    {
        return Mathf.Max(value, 0);
    }

    private IEnumerator StartShowingIcons()
    {
        int i = 0;
        while (i < _impactsValue.Length)
        {
            if (_impactsValue[i] > 0)
            {
                _valueIcons[i].Play();
                yield return new WaitForSeconds(_iconShownTime);
            }

            i++;
        }
    }

    private void OnNoPostsFound()
    {
        _postUI.gameObject.SetActive(false);
        _buttons.gameObject.SetActive(false);
        _noFoundPosts.gameObject.SetActive(true);
    }

    private void OnPostsFound()
    {
        _postUI.gameObject.SetActive(true);
        _buttons.gameObject.SetActive(true);
        _noFoundPosts.gameObject.SetActive(false);
    }

    private void SetLestPostCount(int count)
    {
        _counter.SetText(count.ToString());
    }
}
