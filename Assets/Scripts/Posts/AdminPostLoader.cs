using System.Collections.Generic;
using UnityEngine;

public class AdminPostLoader : MonoBehaviour
{
    private string _jsonFilePath;
    private PostWrapper _postWrapper;
    protected bool _isLoaded = false;
    private bool _isSentEmail = false;

    private int _maxPostsToSelect;

    private int _currentIdPost = 0;
    private int _maxIdPost = 0;

    public static MailUI PostMailUI { get; private set; }

    [System.Serializable]
    public class Impact
    {
        public int conspiracyToScienceValue;
        public int conservatismToProgressValue;
        public int communismToCapitalismValue;
        public int authoritarianismToDemocracyValue;
        public int pacifismToMilitarismValue;
        public int profit;
        public int victims;
    }

    [System.Serializable]
    public class AdminPost
    {
        public string nickname;
        public string content;
        public string date;
        public Impact acceptImpact;
        public Impact denyImpact;
        public Mail mail;
    }

    [System.Serializable]
    public class PostWrapper
    {
        public List<AdminPost> posts;
    }

    private void Awake()
    {
        GlobalEventManager.OnInitAdminPost += SendCurrentPost;
        GlobalEventManager.OnLoadNextPost += TryLoadNextPost;
        GlobalEventManager.OnAdminPostMailAdded += SetMailUI;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnInitAdminPost -= SendCurrentPost;
        GlobalEventManager.OnLoadNextPost -= TryLoadNextPost;
        GlobalEventManager.OnAdminPostMailAdded -= SetMailUI;
    }

    protected PostWrapper LoadPostsFromFile(string jsonFilePath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);
        if (jsonFile == null)
        {
            Debug.LogError("Cannot find JSON file: " + jsonFilePath);
            return null;
        }

        string jsonData = jsonFile.text;

        _jsonFilePath = jsonFilePath;

        return JsonUtility.FromJson<PostWrapper>(jsonData);
    }

    public void LoadPosts(string jsonFilePath, int maxPostsToSelect)
    {
        if (_isLoaded && jsonFilePath == _jsonFilePath) return;
        _isLoaded = false;

        _isSentEmail = false;

        _postWrapper = LoadPostsFromFile(jsonFilePath);
        if (_postWrapper == null || _postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }

        _maxPostsToSelect = maxPostsToSelect;

        _postWrapper = SelectRandomPosts(_postWrapper);
        
        _maxIdPost = _postWrapper.posts.Count - 1;
        _currentIdPost = 0;
        SendPost(_currentIdPost);
        SendLeftPostCount();
        _isLoaded = true;
    }

    private PostWrapper ShufflePosts(PostWrapper wrapper)
    {
        for (int i = wrapper.posts.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (wrapper.posts[j], wrapper.posts[i]) = (wrapper.posts[i], wrapper.posts[j]);
        }
        return wrapper;
    }

    private PostWrapper SelectRandomPosts(PostWrapper wrapper)
    {
        wrapper = ShufflePosts(wrapper);

        PostWrapper selectedWrapper = new()
        {
            posts = wrapper.posts.GetRange(0, Mathf.Min(_maxPostsToSelect, wrapper.posts.Count))
        };

        return selectedWrapper;
    }

    public bool IsNoPostsFound()
    {
        return _currentIdPost > _maxIdPost;
    }

    public void TryLoadNextPost()
    {
        if (_currentIdPost == _maxIdPost)
            _currentIdPost++;

        _isSentEmail = false;

        if (IsNoPostsFound())
        {
            GlobalEventManager.CallOnNoPostsFound();
            SendPost(null);
            GlobalEventManager.CallOnSendLeftPostCount(0);
            return;
        }

        _currentIdPost++;
        SendPost(_currentIdPost);
        SendLeftPostCount();
    }

    public void SendLeftPostCount()
    {
        GlobalEventManager.CallOnSendLeftPostCount(_maxIdPost + 1 - _currentIdPost);
    }

    public void SendPost(int? postId)
    {
        // If postId == null or more than max, they will send null
        var post = (postId == null || postId > _maxIdPost) ? null : _postWrapper.posts[postId.Value];
        GlobalEventManager.CallOnSendAdminPost(post);

        PostMailUI = null;

        if (post != null && !post.mail.IsVoid && !_isSentEmail)
        {
            post.mail.isQuestEmail = true;
            GlobalEventManager.CallOnAddNewMail(post.mail);
            _isSentEmail = true;
        }
    }

    public void SendCurrentPost()
    {
        SendPost(_currentIdPost);
        SendLeftPostCount();
    }

    private void SetMailUI(MailUI mailUI)
    {
        PostMailUI = mailUI;
    }
}
