using System.Collections.Generic;
using UnityEngine;

public class PostsLoader : MonoBehaviour
{
    [SerializeField] protected GameObject _postPrefab;
    [SerializeField] protected Transform _contentParent;
    [SerializeField] protected string _jsonFilePath;

    protected bool _isLoaded = false;

    [System.Serializable]
    public class Post
    {
        public string nickname;
        public string content;
        public string date;
    }

    [System.Serializable]
    public class PostWrapper
    {
        public List<Post> posts;
    }

    private void OnEnable()
    {
        LoadPosts();
    }

    protected PostWrapper CreatePostWrapper()
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

    public virtual void LoadPosts()
    {
        if (_isLoaded) return;

        PostWrapper postWrapper = CreatePostWrapper();
        if (postWrapper == null || postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }

        foreach (var post in postWrapper.posts)
        {
            GameObject postObject = Instantiate(_postPrefab, _contentParent);

            if (postObject.TryGetComponent<PostUI>(out var postUI))
            {
                postUI.SetPostData(post.nickname, post.content, post.date);
            }
        }

        _isLoaded = true;
    }
}
