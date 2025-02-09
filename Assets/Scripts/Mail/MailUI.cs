using TMPro;
using UnityEngine;

public class MailUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _topic;
    [SerializeField] private GameObject _newEmailBadge;
    private bool _isQuestAccepted = false;

    public bool IsQuestAccepted { get => _isQuestAccepted; }
    
    private Mail _mail;

    public bool IsMailBelongsToUI(Mail mail)
    {
        return mail == _mail;
    }

    private void Awake()
    {
        GlobalEventManager.OnAcceptQuest += AcceptQuest;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAcceptQuest -= AcceptQuest;
    }

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

    public void AcceptQuest(Mail mail)
    {
        if (mail == _mail)
        {
            mail.isQuestAccepted = true;
            _isQuestAccepted = true;
        }
    }
}
