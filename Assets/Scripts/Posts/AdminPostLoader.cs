using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdminPostsLoader : PostsLoader
{
    private int _currentIdPost = 0;
    private int _maxIdPost = 0;
    protected new PostWrapper _postWrapper;


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

    [System.Serializable]
    public new class PostWrapper
    {
        public List<AdminPost> posts;
    }

    protected new PostWrapper CreatePostWrapper()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(_jsonFilePath);
        if (jsonFile == null)
        {
            Debug.LogError("Cannot find JSON file: " + _jsonFilePath);
            return null;
        }

        string jsonData = jsonFile.text;

        return JsonUtility.FromJson<PostWrapper>(jsonData);
    }


    private void Awake()
    {
        GlobalEventManager.OnInitAdminPost += ReturnCurrentPost;
        GlobalEventManager.OnLoadNextPost += LoadNextPost;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnInitAdminPost -= ReturnCurrentPost;
        GlobalEventManager.OnLoadNextPost -= LoadNextPost;
    }

    private void Start()
    {
        LoadPosts();
    }

    public override void LoadPosts()
    {
        if (_isLoaded) return;

        _postWrapper = CreatePostWrapper();
        if (_postWrapper == null || _postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }
        
        _maxIdPost = _postWrapper.posts.Count - 1;
        _currentIdPost = 0;

        ReturnCurrentPost();

        _isLoaded = true;
    }

    public void LoadNextPost() {
        _currentIdPost++;
        HandlePostAction(_currentIdPost);
    }

    private void ReturnCurrentPost()
    {
        HandlePostAction(_currentIdPost);
    }

    private void HandlePostAction(int postId)
    {
        if (_maxIdPost < postId)
        {
            GlobalEventManager.CallOnSendAdminPost(null);
            return;
        }

        AdminPost post = _postWrapper.posts[postId];
        GlobalEventManager.CallOnSendAdminPost(post);
    }
}
