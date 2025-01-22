using UnityEngine;

public class AdminPostLoaderUI : MonoBehaviour
{
    [SerializeField] private PostUI _postUI;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private GameObject _noFoundPosts;

    private void Awake()
    {
        GlobalEventManager.OnSendAdminPost += UpdatePost;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnSendAdminPost -= UpdatePost;
    }

    private void OnEnable()
    {
        GlobalEventManager.CallOnInitAdminPost();
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
    }

    private void OnPostsFound()
    {
        _postUI.gameObject.SetActive(true);
        _buttons.gameObject.SetActive(true);
        _noFoundPosts.gameObject.SetActive(false);
    }
}
