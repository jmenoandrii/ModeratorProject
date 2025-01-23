using System.Collections;
using UnityEngine;

public class PostManager : MonoBehaviour
{
    [SerializeField] private AdminPostsLoader _postsLoader;
    [SerializeField] private string[] _jsonFilePaths;
    [SerializeField] private float _timerDuration = 10f;
    private int _currentFileIndex = 0;
    private int _iterations = 0;
    private const int _maxIterations = 4;

    private bool _isLoaded = false;
    private bool _isStartedLoadNextBlock = false;

    private void Awake()
    {
        GlobalEventManager.OnNoPostsFound += OnPostComplete;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnNoPostsFound -= OnPostComplete;
    }

    private void Start()
    {
        LoadNextFile();
    }

    private void LoadNextFile()
    {
        if (_currentFileIndex >= _jsonFilePaths.Length)
        {
            Debug.LogError("No more files to load.");
            return;
        }

        _postsLoader.LoadPosts(_jsonFilePaths[_currentFileIndex]);
        _currentFileIndex++;
        _isLoaded = true;
        _isStartedLoadNextBlock = false;
    }

    public void OnPostComplete()
    {
        if (!_isStartedLoadNextBlock && _postsLoader.IsNoPostsFound())
        {
            StartCoroutine(StartTimer());
            _isStartedLoadNextBlock = true;
        }
    }

    private IEnumerator StartTimer()
    {
        _isLoaded = false;
        yield return new WaitForSeconds(_timerDuration);

        _iterations++;
        if (_iterations >= _maxIterations)
        {
            EndGame();
        }
        else
        {
            LoadNextFile();
        }
    }

    private void EndGame()
    {
        GlobalEventManager.CallOnEndInitiate();
    }
}
