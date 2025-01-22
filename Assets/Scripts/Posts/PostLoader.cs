using System.Collections.Generic;
using UnityEngine;

public class PostsLoader : MonoBehaviour
{
    [SerializeField] private GameObject _postPrefab; // Prefab for post UI elements
    [SerializeField] private Transform _contentParent; // Parent object to hold the posts
    [SerializeField] private string _jsonFilePath; // Path to the JSON file in Resources

    private bool _isLoaded = false; // Flag to prevent reloading posts

    // Class to represent individual posts
    [System.Serializable]
    public class Post
    {
        public string title;
        public string content;
    }

    // Wrapper class to hold a list of posts
    [System.Serializable]
    public class PostWrapper
    {
        public List<Post> posts; // List of posts
    }

    // Called when the object is enabled
    private void OnEnable()
    {
        LoadPosts(); // Load posts when the object is enabled
    }

    // Method to load posts from the JSON file
    public void LoadPosts()
    {
        if (_isLoaded) return; // If posts are already loaded, exit the method

        TextAsset jsonFile = Resources.Load<TextAsset>(_jsonFilePath); // Load the JSON file from Resources
        if (jsonFile == null)
        {
            Debug.LogError("Cannot find JSON file: " + _jsonFilePath); // Log an error if the file is not found
            return;
        }

        // Deserialize the JSON data into a PostWrapper object
        string jsonData = jsonFile.text;
        PostWrapper postWrapper = JsonUtility.FromJson<PostWrapper>(jsonData);
        if (postWrapper == null || postWrapper.posts == null)
        {
            Debug.LogError("Failed to deserialize posts from JSON."); // Log an error if deserialization fails
            return;
        }

        // Instantiate a post object for each post in the list
        foreach (var post in postWrapper.posts)
        {
            GameObject postObject = Instantiate(_postPrefab, _contentParent); // Instantiate the prefab under the content parent

            // If the PostUI component exists, set the post data
            if (postObject.TryGetComponent<PostUI>(out var postUI))
            {
                postUI.SetPostData(post.title, post.content); // Set the title and content of the post
            }
        }

        _isLoaded = true; // Mark the posts as loaded
    }
}
