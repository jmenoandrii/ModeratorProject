using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private TMP_Text dateText;
    public AdminPostsLoader.Impact acceptImpact;
    public AdminPostsLoader.Impact denyImpact;

    public void SetPostData(string nickname, string content, string date)
    {
        nicknameText.SetText(nickname);
        contentText.SetText(content);
        dateText.SetText(date);
    }
}
