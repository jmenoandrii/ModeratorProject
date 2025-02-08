using UnityEngine;

public class MailManager : MonoBehaviour
{
    [SerializeField] private Transform _mailListsContainer;
    [SerializeField] private GameObject _mailLists;
    [SerializeField] private FullMailUI _fullMailUI;
    [SerializeField] private GameObject _mailPrefab;
    [SerializeField] private GameObject _mailApp;
    [SerializeField] private PopUp _popUpApp;
    [SerializeField] private MailUI _popUpMailUI;

    [SerializeField] private Mail _welcomeMail;

    private bool _isWelcomedUser = false;
    private MailUI _newMailUI;

    private void Awake()
    {
        GlobalEventManager.OnAddNewMail += AddNewMail;
        GlobalEventManager.OnShowFullMail += ShowFullMail;
        GlobalEventManager.OnHideFullMail += HideFullMail;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAddNewMail -= AddNewMail;
        GlobalEventManager.OnShowFullMail -= ShowFullMail;
        GlobalEventManager.OnHideFullMail -= HideFullMail;
    }

    public void WelcomeMail()
    {
        _isWelcomedUser = true;
        AddNewMail(_welcomeMail);
    }

    private void AddNewMail(Mail mail)
    {
        if (!_isWelcomedUser)
            return;

        // Spawn new mail obj
        GameObject _newMailObj = Instantiate(_mailPrefab);

        RectTransform rectTransform = _newMailObj.GetComponent<RectTransform>();

        rectTransform.SetParent(_mailListsContainer, false);

        /*rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;*/

        _newMailObj.transform.SetAsFirstSibling();

        // Set mail data
        _newMailUI = _newMailObj.GetComponent<MailUI>();

        _newMailUI.SetData(mail);

        // PopUp
        _popUpMailUI.SetData(mail);
        _popUpApp.Show();
    }

    private void ShowFullMail(Mail mail, GameObject shortMailObj)
    {
        _fullMailUI.SetData(mail, shortMailObj);

        _mailLists.SetActive(false);
        _fullMailUI.gameObject.SetActive(true);
    }

    public void ShowFullNewMail()
    {
        _newMailUI.ShowFull();
    }

    public void HideFullMail()
    {
        _mailLists.SetActive(true);
        _fullMailUI.gameObject.SetActive(false);
    }
}
