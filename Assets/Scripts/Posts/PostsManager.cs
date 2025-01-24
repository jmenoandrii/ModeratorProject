using System.Collections;
using UnityEngine;

public class PostManager : MonoBehaviour
{
    [SerializeField] private AdminPostsLoader _postsLoader;
    [SerializeField] private string[] _jsonFilePaths;
    [SerializeField] private float _timerDuration = 10f;
    [SerializeField] private Mail _adminPanelMail;
    private float _curTimer = 0;
    private int _currentFileIndex = 0;
    private int _iterations = 0;
    private const int _maxIterations = 4;

    private bool _isLoaded = false;
    private bool _isStartedLoadNextBlock = false;
    private bool _isTimerStarted = false;

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
            return;
        }

        _postsLoader.LoadPosts(_jsonFilePaths[_currentFileIndex]);

        if (_isStartedLoadNextBlock)
            GlobalEventManager.CallOnAddNewMail(_adminPanelMail);

        _currentFileIndex++;
        _isLoaded = true;
        _isStartedLoadNextBlock = false;
    }

    public void OnPostComplete()
    {
        if (!_isStartedLoadNextBlock && _postsLoader.IsNoPostsFound())
        {
            _isTimerStarted = true;
            _curTimer = _timerDuration;
            _isStartedLoadNextBlock = true;
        }
    }

    private void Update()
    {
        if (_isTimerStarted)
        {
            _isLoaded = false;
            GlobalEventManager.CallOnUpdateTimerOfLoadPosts(_curTimer);

            _curTimer -= Time.deltaTime;

            if (_curTimer <= 0)
            {
                _isTimerStarted = false;
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
        }
    }

    private void EndGame()
    {
        GlobalEventManager.CallOnEndInitiate();
    }
}
