using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdminPostsLoader : PostsLoader
{
    private int _currentIdPost = 0;
    private int _maxIdPost = 0;
    private PostWrapper _adminPostWrapper;

    [System.Serializable]
    public class Impact
    {
        public int conspiracyToScienceValue;
        public int conservatismToProgressValue;
        public int communismToCapitalismValue;
        public int authoritarianismToDemocracyValue;
        public int pacifismToMilitarismValue;
    }

    [System.Serializable]
    public class AdminPost
    {
        public string nickname;
        public string content;
        public string date;
        public Impact acceptImpact;
        public Impact denyImpact;
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
    public new class PostWrapper
    {
        public List<AdminPost> posts;
    }

    protected new PostWrapper LoadPostsFromFile(string jsonFilePath)
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

    public override void LoadPosts(string jsonFilePath)
    {
        if (_isLoaded && jsonFilePath == _jsonFilePath) return;
        _isLoaded = false;

        _adminPostWrapper = LoadPostsFromFile(jsonFilePath);
        if (_adminPostWrapper == null || _adminPostWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }
        
        _maxIdPost = _adminPostWrapper.posts.Count - 1;
        _currentIdPost = 0;
        SendPost(_currentIdPost);
        _isLoaded = true;
    }

    public bool IsNoPostsFound()
    {
        return _currentIdPost > _maxIdPost;
    }

    public void TryLoadNextPost()
    {
        if (IsNoPostsFound())
        {
            GlobalEventManager.CallOnNoPostsFound();
            SendPost(null);
            return;
        }

        _currentIdPost++;
        SendPost(_currentIdPost);
    }

    public void SendPost(int? postId)
    {
        // If postId == null or more than max, they will send null
        var post = (postId == null || postId > _maxIdPost) ? null : _adminPostWrapper.posts[postId.Value];
        GlobalEventManager.CallOnSendAdminPost(post);
    }

    public void SendCurrentPost()
    {
        SendPost(_currentIdPost);
    }

}
