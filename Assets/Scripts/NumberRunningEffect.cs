using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text))]
public class NumberTypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private float _delayBeforeAction = 0.05f;
    [SerializeField] private UnityEvent _actionOnComplete;
    [SerializeField] private bool _isAutoStart = true;

    private bool _isStarted = false;
    private int _targetNumber = 0;
    private int _currentNumber = 0;

    private void Start()
    {
        if (_textComponent == null)
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        if (int.TryParse(_textComponent.text, out _targetNumber))
        {
            _textComponent.text = "";
            if (_isAutoStart)
                StartCoroutine(TypeNumbers());
        }
        else
        {
            Debug.LogError("Text does not contain a valid number!");
        }
    }

    public void StartEffect()
    {
        if (!_isStarted)
            StartCoroutine(TypeNumbers());
    }

    private IEnumerator TypeNumbers()
    {
        _isStarted = true;

        while (_currentNumber < _targetNumber)
        {
            _currentNumber++;
            _textComponent.text = _currentNumber.ToString();
            yield return new WaitForSeconds(_typingSpeed);
        }

        yield return new WaitForSeconds(_delayBeforeAction);
        _actionOnComplete?.Invoke();
    }
}

