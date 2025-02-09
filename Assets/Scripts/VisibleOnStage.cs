using UnityEngine;

public class VisibleOnStage : MonoBehaviour
{
    [SerializeField, Range(1, 2)] private int _stage;
    [SerializeField] private GameObject _target;

    private void Awake()
    {
        GlobalEventManager.OnPostComplete += Show;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnPostComplete -= Show;
    }

    private void Show(int completePostCount)
    {
        if (completePostCount == _stage)
            _target.SetActive(true);
    }
}
