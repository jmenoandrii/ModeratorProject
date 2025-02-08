using TMPro;
using UnityEngine;

public class MailUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _topic;
    [SerializeField] private GameObject _newEmailBadge;

    public bool IsRead { get => !_newEmailBadge.activeSelf; }
    
    private Mail _mail;

    public void SetData(Mail mail)
    {
        _mail = mail;
        _nickname.SetText(mail.nickname);
        _topic.SetText(mail.topic);
    }

    public void ShowFull()
    {
        _newEmailBadge?.SetActive(false);

        GlobalEventManager.CallOnShowFullMail(_mail, gameObject);
    }
}
