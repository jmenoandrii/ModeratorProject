using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapIdeologyPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _provinceCounter;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Image _ideologyImage;

    public void Initialize(string title, int count, Sprite image)
    {
        gameObject.SetActive(true);
        _ideologyImage.sprite = image;
        _title.SetText(title);
        SetCounterText(count);
    }

    public void UpdateCounterText(int count)
    {
        if (count < 0)
            count = 0;

        if (count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        SetCounterText(count);
    }

    private void SetCounterText(int count)
    {
        if (count == 0 || count == 1)
        {
            _provinceCounter.SetText(count.ToString() + " province");
        }
        else if (count > 1) {
            _provinceCounter.SetText(count.ToString() + " provinces");
        }
    }
}
