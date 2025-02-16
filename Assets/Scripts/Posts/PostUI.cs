using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostUI : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _accountImage;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text likesText;
    public AdminPostLoader.AcceptImpact acceptImpact;
    public AdminPostLoader.DenyImpact denyImpact;
    public bool isMail;

    public void SetPostData(string nickname, string content, string date, int likes)
    {
        _accountImage.sprite = _sprites[Random.Range(0, _sprites.Length)];
        nicknameText.SetText(nickname);
        contentText.SetText(content);
        dateText.SetText(date);
        likesText.SetText(likes.ToString());
    }
}
