using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AdminPostsLoader : PostsLoader
{
    private int _currentIdPost = 0;
    private int _maxIdPost = 0;

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

        var post = _postWrapper.posts[postId];
        GlobalEventManager.CallOnSendAdminPost(post);
    }
}
