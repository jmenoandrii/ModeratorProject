using System.Collections.Generic;
using UnityEngine;

public class PostsLoader : MonoBehaviour
{
    [SerializeField] protected string _jsonFilePath;

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

    public virtual void LoadPosts(string jsonFilePath)
    {
        if (_isLoaded) return;

        PostWrapper postWrapper = LoadPostsFromFile(jsonFilePath);
        if (postWrapper == null || postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON.");
            return;
        }

        _isLoaded = true;
    }
}
