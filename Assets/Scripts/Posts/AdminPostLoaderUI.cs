using System;
using TMPro;
using UnityEngine;

public class AdminPostLoaderUI : MonoBehaviour
{
    [SerializeField] private PostUI _postUI;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private GameObject _noFoundPosts;
    [SerializeField] private TMP_Text _counter;
    [Header("Timer")]
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private float _timerDuration = 10f;
    private float _curTimer = 0f;
    private TimeSpan _timeSpan;
    private string _timeStr;

    private void Awake()
    {
        GlobalEventManager.OnSendAdminPost += UpdatePost;
        GlobalEventManager.OnSendLeftPostCount += SetLestPostCount;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnSendAdminPost -= UpdatePost;
        GlobalEventManager.OnSendLeftPostCount -= SetLestPostCount;
    }

    private void OnEnable()
    {
        GlobalEventManager.CallOnInitAdminPost();
    }

    private void Update()
    {
        if (_curTimer != 0f)
        {
            _curTimer -= Time.deltaTime;
            _timeSpan = TimeSpan.FromSeconds(_curTimer);
            _timeStr = string.Format("{0:D2}:{1:D2}", _timeSpan.Minutes, _timeSpan.Seconds);
            _timer.SetText(_timeStr);
        }
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
    }

    public void NextPost(bool isAccept)
    {
        if (isAccept)
            GlobalEventManager.CallOnSendImpact(_postUI.acceptImpact);
        else
            GlobalEventManager.CallOnSendImpact(_postUI.denyImpact);

        GlobalEventManager.CallOnLoadNextPost();
    }

    private void OnNoPostsFound()
    {
        _postUI.gameObject.SetActive(false);
        _buttons.gameObject.SetActive(false);
        _noFoundPosts.gameObject.SetActive(true);

        _curTimer = _timerDuration;

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
