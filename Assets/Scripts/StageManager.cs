using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _stageTwoEvents;
    [SerializeField] private UnityEvent _stageThreeEvents;

    private void Awake()
    {
        GlobalEventManager.OnPostComplete += CallEvents;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnPostComplete -= CallEvents;
    }
    
    private void CallEvents(int stage)
    {
        switch (stage)
        {
            case 2:
                _stageTwoEvents.Invoke();
                break;
            case 3:
                _stageThreeEvents.Invoke();
                break;
        }
    }
}
