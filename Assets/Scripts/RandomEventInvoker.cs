using UnityEngine;
using UnityEngine.Events;

public class RandomEventInvoker : MonoBehaviour
{
    [SerializeField] private bool _isOnce = false;
    [SerializeField] private float _minInterval = 1f;
    [SerializeField] private float _maxInterval = 5f;
    [SerializeField] private UnityEvent _onRandomEvent;

    protected Coroutine _eventCoroutine;

    protected virtual void Start()
    {
        _eventCoroutine = StartCoroutine(InvokeRandomEvent());
    }

    protected virtual System.Collections.IEnumerator InvokeRandomEvent()
    {
        while (true)
        {
            float randomInterval = Random.Range(_minInterval, _maxInterval);
            yield return new WaitForSeconds(randomInterval);
            _onRandomEvent?.Invoke();
            if (_isOnce)
                break;
        }
    }
}