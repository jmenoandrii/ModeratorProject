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

    private void AddNewMail(Mail mail)
    {
        // Spawn new mail obj
        GameObject newMail = Instantiate(_mailPrefab);

        RectTransform rectTransform = newMail.GetComponent<RectTransform>();

        rectTransform.SetParent(_mailListsContainer, false);

        /*rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;*/

        newMail.transform.SetAsFirstSibling();

        // Set mail data
        MailUI mailUI = newMail.GetComponent<MailUI>();

        mailUI.SetData(mail);

        // PopUp
        if (!_mailApp.activeSelf)
        {
            _popUpMailUI.SetData(mail);
            _popUpApp.Show();
        }
    }

    private void ShowFullMail(Mail mail)
    {
        _fullMailUI.SetData(mail);

        _mailLists.SetActive(false);
        _fullMailUI.gameObject.SetActive(true);
    }

    private void HideFullMail()
    {
        _mailLists.SetActive(true);
        _fullMailUI.gameObject.SetActive(false);
    }
}
