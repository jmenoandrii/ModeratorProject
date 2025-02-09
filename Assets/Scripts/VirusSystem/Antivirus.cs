using UnityEngine;
using UnityEngine.UI;

public class Antivirus : MonoBehaviour
{
    [Header("Antivirus Components")]
    [SerializeField] private GameObject _onButton;
    [SerializeField] private GameObject _offButton;
    [SerializeField] private GameObject _linkObj;
    public static Antivirus instance;
    public bool IsOn {  get; private set; }

    private void Start()
    {
        // singleton initialization
        if (instance == null)
            instance = this;
        else
            Debug.LogError($"ERR[{gameObject.name}]: Antivirus must be a singleton");

        // settings
        bool state = PlayerPrefs.GetInt("antivirus", 0) == 1;
        SetState(state);
        _linkObj.SetActive(state);
    }

    public void Unlock()
    {
        PlayerPrefs.SetInt("antivirus", 1);
        _linkObj.SetActive(true);
        SetState(true);
    }

    public void SetState(bool state)
    {
        IsOn = state;
        _onButton.SetActive(IsOn);
        _offButton.SetActive(!IsOn);
    }
}
