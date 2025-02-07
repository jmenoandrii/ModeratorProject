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

    private bool _isBooted = false;
    [Header("Timer")]
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private float _postLoadingTime;
    [SerializeField] private float _iconShownTime;
    [SerializeField] private AudioSource _noPostsFindAudio;
    private int[] _impactsValue = new int[12];
    [SerializeField] private ValueIcon[] _valueIcons = new ValueIcon[12];

    private Coroutine _currentCoroutine;

    private void Awake()
    {
        GlobalEventManager.OnSendAdminPost += UpdatePost;
        GlobalEventManager.OnSendLeftPostCount += SetLestPostCount;
        GlobalEventManager.OnUpdateTimerOfLoadPosts += UpdateSlider;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnSendAdminPost -= UpdatePost;
        GlobalEventManager.OnSendLeftPostCount -= SetLestPostCount;
        GlobalEventManager.OnUpdateTimerOfLoadPosts -= UpdateSlider;
    }

    private void OnEnable()
    {
        foreach (var icon in _valueIcons)
        {
            icon.Rewind();
        }

        if (_currentCoroutine != null)
        {
            _currentCoroutine = StartCoroutine(WaitToLoadPosts());
        }

        GlobalEventManager.CallOnInitAdminPost();
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
    }

    private void UpdateSlider(float time)
    {
        _progressSlider.value = time;
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

        if (!_isBooted && _currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(WaitToLoadPosts());
        }
    }

    public void NextPost(bool isAccept)
    {
        _isBooted = false;
        
        AdminPostsLoader.Impact impact = isAccept ? _postUI.acceptImpact : _postUI.denyImpact;

        GlobalEventManager.CallOnSendImpact(impact);

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

        _isBooted = true;
        _currentCoroutine = null;
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
        _loading.gameObject.SetActive(false);
        _noFoundPosts.gameObject.SetActive(true);
        _noPostsFindAudio.Play();
    }

    private void OnPostsFound()
    {
        if (!_isBooted && _currentCoroutine != null)
            return; 
        _postUI.gameObject.SetActive(true);
        _buttons.gameObject.SetActive(true);
        _noFoundPosts.gameObject.SetActive(false);
    }

    private void SetLestPostCount(int count)
    {
        _counter.SetText(count.ToString());
    }
}
