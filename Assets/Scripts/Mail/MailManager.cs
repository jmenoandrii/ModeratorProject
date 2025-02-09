using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject Browser;

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

    private void AddNewMail(Mail mail, bool isAdminPostMail = false)
    {
        if (!_isWelcomedUser)
        {
            StartCoroutine(PostponedAddNewMail(mail, isAdminPostMail));
            return;
        }

        // Spawn new mail obj
        GameObject newMailObj = Instantiate(_mailPrefab);

        RectTransform rectTransform = newMailObj.GetComponent<RectTransform>();

        rectTransform.SetParent(_mailListsContainer, false);

        newMailObj.transform.SetAsFirstSibling();

        // Set mail data
        _newMailUI = newMailObj.GetComponent<MailUI>();

        _newMailUI.SetData(mail);

        if (isAdminPostMail)
            GlobalEventManager.CallOnAdminPostMailAdded(_newMailUI);

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
        if (_newMailUI != null && _newMailUI.gameObject != null)
            _newMailUI.ShowFull();
    }

    public void HideFullMail()
    {
        _mailLists.SetActive(true);
        _fullMailUI.gameObject.SetActive(false);
    }

    private IEnumerator PostponedAddNewMail(Mail mail, bool isAdminPostMail)
    {
        if (isAdminPostMail)
            yield return new WaitUntil(() => Browser.activeSelf);
        else
            yield return new WaitUntil(() => _isWelcomedUser);

        AddNewMail(mail, isAdminPostMail);
    }
}
