using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullMailUI : MonoBehaviour
{
    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _topic;
    [SerializeField] private TMP_Text _content;
    [SerializeField] private Button _acceptQuestButton;
    private GameObject _shortMailObj;
    private Mail _mail;

    public void SetData(Mail mail, GameObject shortMailObj)
    {
        _mail = mail;
        _nickname.SetText(mail.nickname);
        _topic.SetText(mail.topic);
        _content.SetText(mail.content);
        _shortMailObj = shortMailObj;
        _acceptQuestButton.gameObject.SetActive(mail.isQuestEmail && !mail.isQuestAccepted);
        _acceptQuestButton.interactable = AdminPostLoader.PostMailUI != null && AdminPostLoader.PostMailUI.IsMailBelongsToUI(_mail);
    }

    public void HideFull()
    {
        GlobalEventManager.CallOnHideFullMail();
    }

    public void DeleteMail()
    {
        Destroy(_shortMailObj);
        GlobalEventManager.CallOnHideFullMail();
    }

    public void AcceptQuest()
    {
        GlobalEventManager.CallOnAcceptQuest(_mail);
        _mail.isQuestAccepted = true;
        _acceptQuestButton.gameObject.SetActive(false);
    }
}
