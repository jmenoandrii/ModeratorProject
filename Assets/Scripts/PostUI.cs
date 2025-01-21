using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;

    public void SetPostData(string title, string content)
    {
        titleText.SetText(title);
        contentText.SetText(content);
    }
}
