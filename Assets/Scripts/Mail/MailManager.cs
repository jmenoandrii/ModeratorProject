using UnityEngine;

public class MailManager : MonoBehaviour
{
    [SerializeField] private GameObject _mailListBox;
    [SerializeField] private FullMailUI _fullMailUI;
    [SerializeField] private GameObject _mailPrefab;

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

        rectTransform.SetParent(_mailListBox.transform, false);

        /*rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;*/

        // Set mail data
        MailUI mailUI = newMail.GetComponent<MailUI>();

        mailUI.SetData(mail);
    }

    private void ShowFullMail(Mail mail)
    {
        _fullMailUI.SetData(mail);

        _mailListBox.SetActive(false);
        _fullMailUI.gameObject.SetActive(true);
    }

    private void HideFullMail()
    {
        _mailListBox.SetActive(true);
        _fullMailUI.gameObject.SetActive(false);
    }
}
