using UnityEngine;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{
    [SerializeField] private float _waitSeconds = 60f;
    [SerializeField] private int _nextSceneIndex = 1;

    private void Start()
    {
        // ��������� ������ ��� ������������� ��������
        Invoke("LoadNextScene", _waitSeconds);
    }

    private void Update()
    {
        // ���������, �� ��������� Enter
        if (Input.GetKeyDown(KeyCode.Return)) // Return � �� Enter
        {
            // ��������� ������������ �������
            CancelInvoke("LoadNextScene");
            // ����������� �������� �����
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_nextSceneIndex);
    }
}
