using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _bootCanvas;
    [SerializeField] private GameObject _gameCanvas;

    public void LoadGameCanvas()
    {
        _bootCanvas.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
