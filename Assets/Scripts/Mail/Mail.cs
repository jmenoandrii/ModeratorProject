using UnityEngine;

[System.Serializable]
public class Mail
{
    public string nickname;
    public string topic;
    public string content;
    public string date;

    public bool IsVoid { get => (nickname == null && topic == null && content == null && date == null); }
}
