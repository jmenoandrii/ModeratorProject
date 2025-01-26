using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostsLoader : MonoBehaviour
{
    [SerializeField] protected GameObject _postPrefab;
    [SerializeField] protected Transform _contentParent;

    [SerializeField] protected string _jsonFilePath;
    [SerializeField] private int _maxPostsToSelect = 5;
    [SerializeField] private Sprite[] _adSprites;
    [SerializeField] private Image[] _adObjects;


    protected PostWrapper _postWrapper;

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
        LoadPosts(_jsonFilePath);
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
        if (_isLoaded) return;

        PostWrapper postWrapper = LoadPostsFromFile(jsonFilePath);
        if (postWrapper == null || postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }

        postWrapper = SelectRandomPosts(postWrapper);
        
        foreach (var post in postWrapper.posts)
        {
            GameObject postObject = Instantiate(_postPrefab, _contentParent);
            if (postObject.TryGetComponent<PostUI>(out var postUI))
            {
                postUI.SetPostData(post.nickname, post.content, post.date);
            }
        }

        for (int i = 0; i < _adObjects.Length; i++)
        {
            _adObjects[i].sprite = _adSprites[Random.Range(0, _adSprites.Length)];
        }

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
}
