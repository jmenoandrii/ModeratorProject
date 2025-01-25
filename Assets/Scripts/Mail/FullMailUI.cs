using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullMailUI : MonoBehaviour
{
    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _date;
    [SerializeField] private TMP_Text _topic;
    [SerializeField] private TMP_Text _content;
    [SerializeField] private Image _img;

    public void SetData(Mail mail)
    {
        _nickname.SetText(mail.nickname);
        _topic.SetText(mail.topic);
        _content.SetText(mail.content);
    }

    public void HideFull()
    {
        GlobalEventManager.CallOnHideFullMail();
    }
}
