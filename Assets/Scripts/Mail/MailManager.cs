using UnityEngine;

public class MailManager : MonoBehaviour
{
    [Header("Mail Settings")]
    [SerializeField] private GameObject _mailListBox;
    [SerializeField] private GameObject _mailPrefab;

    private void Awake()
    {
        GlobalEventManager.OnAddNewMail += AddNewMail;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAddNewMail -= AddNewMail;
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
}
