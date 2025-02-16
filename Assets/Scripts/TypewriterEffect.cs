using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField] private float _typingSpeed = 0.05f;

    [SerializeField] private float _delayBeforeAction = 0.05f;

    [SerializeField] private UnityEvent _actionOnComplete;

    [SerializeField] private bool _isAutoStart = true; 
    private bool _isStarted = false; 

    private string _fullText;
    private string _currentText = "";

    private void Start()
    {
        if (_textComponent == null)
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        _fullText = _textComponent.text;
        _textComponent.text = "";

        if (_isAutoStart)
            StartCoroutine(TypeText());
    }

    public void StartEffect()
    {
        if (!_isStarted)
            StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        _isStarted = true;
        for (int i = 0; i <= _fullText.Length; i++)
        {
            _currentText = _fullText.Substring(0, i);
            _textComponent.text = _currentText;
            yield return new WaitForSeconds(_typingSpeed);
        }
        yield return new WaitForSeconds(_delayBeforeAction);
        _actionOnComplete?.Invoke();
    }
}
