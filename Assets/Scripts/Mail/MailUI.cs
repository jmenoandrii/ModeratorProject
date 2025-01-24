using TMPro;
using UnityEngine;

public class MailUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _topic;
    [SerializeField] private TMP_Text _date;

    public void SetData(Mail mailData)
    {
        _nickname.SetText(mailData.nickname);
        _topic.SetText(mailData.topic);
        _date.SetText(mailData.date);
    }
}
