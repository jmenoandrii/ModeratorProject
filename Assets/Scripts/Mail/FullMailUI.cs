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
    private GameObject _shortMailObj;

    public void SetData(Mail mail, GameObject shortMailObj)
    {
        _nickname.SetText(mail.nickname);
        _topic.SetText(mail.topic);
        _content.SetText(mail.content);
        _shortMailObj = shortMailObj;
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
}
