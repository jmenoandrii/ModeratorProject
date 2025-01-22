using System.Collections.Generic;
using UnityEngine;

public class AdminPostsLoader : PostsLoader
{
    [SerializeField] private PostUI _postUI;

    private PostWrapper _postWrapper;

    private int _currentIdPost = -1;
    private int _maxIdPost = 0;

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
        _currentIdPost = -1;

        LoadNextPost();

        _isLoaded = true;
    }

    public void LoadNextPost() {
        _currentIdPost++;

        if (_maxIdPost == _currentIdPost)
            return;

        var post = _postWrapper.posts[_currentIdPost];

        _postUI.SetPostData(post.nickname, post.content, post.date);
    }
}
