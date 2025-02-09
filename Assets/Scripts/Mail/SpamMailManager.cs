using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AdminPostLoader;

public class SpamMailManager : MonoBehaviour
{
    [SerializeField] private string _jsonFilePath;
    private MailWrapper _maiWrapper;
    private int _iterator;
    [SerializeField] private float _startWaitTime;
    [SerializeField] private float _waitIntervalMin;
    [SerializeField] private float _waitIntervalMax;
    private float _waitTime;

    [System.Serializable]
    public class MailWrapper
    {
        public List<Mail> mails;
    }

    private void Start()
    {
        LoadMailsFromFile();

        _waitTime = _startWaitTime;
        StartCoroutine(StartTimer());
    }

    protected void LoadMailsFromFile()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(_jsonFilePath);
        if (jsonFile == null)
        {
            Debug.LogError("Cannot find JSON file: " + _jsonFilePath);
            return;
        }

        string jsonData = jsonFile.text;

        _maiWrapper = JsonUtility.FromJson<MailWrapper>(jsonData);
    }

    private void SendMail()
    {
        if (_iterator < _maiWrapper.mails.Count)
        {
            GlobalEventManager.CallOnAddNewMail(_maiWrapper.mails[_iterator]);
            StartCoroutine(StartTimer());
            _waitTime = Random.Range(_waitIntervalMin, _waitIntervalMax);
        }
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_waitTime);

        SendMail();
        _iterator++;
    }
}
