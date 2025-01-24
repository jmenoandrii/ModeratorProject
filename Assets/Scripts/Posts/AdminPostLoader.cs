using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdminPostsLoader : MonoBehaviour
{
    [SerializeField] protected string _jsonFilePath;
    private PostWrapper _postWrapper;
    protected bool _isLoaded = false;

    private int _currentIdPost = 0;
    private int _maxIdPost = 0;

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

    private void Awake()
    {
        GlobalEventManager.OnInitAdminPost += SendCurrentPost;
        GlobalEventManager.OnLoadNextPost += TryLoadNextPost;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnInitAdminPost -= SendCurrentPost;
        GlobalEventManager.OnLoadNextPost -= TryLoadNextPost;
    }

    [System.Serializable]
    public class PostWrapper
    {
        public List<AdminPost> posts;
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

        return JsonUtility.FromJson<PostWrapper>(jsonData);
    }

    public void LoadPosts(string jsonFilePath)
    {
        if (_isLoaded && jsonFilePath == _jsonFilePath) return;
        _isLoaded = false;

        _postWrapper = LoadPostsFromFile(jsonFilePath);
        if (_postWrapper == null || _postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }
        
        _maxIdPost = _postWrapper.posts.Count - 1;
        _currentIdPost = 0;
        SendPost(_currentIdPost);
        SendLeftPostCount();
        _isLoaded = true;
    }

    public bool IsNoPostsFound()
    {
        return _currentIdPost >= _maxIdPost;
    }

    public void TryLoadNextPost()
    {
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

        if (post != null && !post.mail.IsVoid)
            GlobalEventManager.CallOnAddNewMail(post.mail);
    }

    public void SendCurrentPost()
    {
        SendPost(_currentIdPost);
        SendLeftPostCount();
    }

}
