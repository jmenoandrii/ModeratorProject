using UnityEngine;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{
    [SerializeField] private float _waitSeconds = 60f;
    [SerializeField] private int _nextSceneIndex = 1;

    private void Start()
    {
        // Запустити таймер для автоматичного переходу
        Invoke("LoadNextScene", _waitSeconds);
    }

    private void Update()
    {
        // Перевірити, чи натиснуто Enter
        if (Input.GetKeyDown(KeyCode.Return)) // Return — це Enter
        {
            // Скасувати автоматичний перехід
            CancelInvoke("LoadNextScene");
            // Завантажити наступну сцену
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_nextSceneIndex);
    }
}
