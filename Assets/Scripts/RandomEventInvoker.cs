using UnityEngine;
using UnityEngine.Events;

public class RandomEventInvoker : MonoBehaviour
{
    [SerializeField] private float _minInterval = 1f;
    [SerializeField] private float _maxInterval = 5f;
    [SerializeField] private UnityEvent _onRandomEvent;

    private void Start()
    {
        StartCoroutine(InvokeRandomEvent());
    }

    private System.Collections.IEnumerator InvokeRandomEvent()
    {
        while (true)
        {
            float randomInterval = Random.Range(_minInterval, _maxInterval);
            yield return new WaitForSeconds(randomInterval);
            _onRandomEvent?.Invoke();
        }
    }
}