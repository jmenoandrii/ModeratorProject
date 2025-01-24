using TMPro;
using UnityEngine;

public class MailUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _topic;
    [SerializeField] private TMP_Text _date;
    private Mail _mail;

    public void SetData(Mail mail)
    {
        _mail = mail;
        _nickname.SetText(mail.nickname);
        _topic.SetText(mail.topic);
        _date.SetText(mail.date);
    }

    public void ShowFull()
    {
        GlobalEventManager.CallOnShowFullMail(_mail);
    }
}
