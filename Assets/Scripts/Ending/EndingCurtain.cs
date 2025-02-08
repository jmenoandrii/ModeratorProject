using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingCurtain : MonoBehaviour
{
    [SerializeField] StandartEnding _ending;
    [SerializeField] TMP_Text _title;
    [SerializeField] Image _image;
    [SerializeField] GameObject _curtain;
    [SerializeField] TooltipTrigger _trigger;

    private void Awake()
    {
        if (EndingBookManager.instance.IsEndingUnlocked(_ending.id))
        {
            _title.SetText(_ending.name);
            _image.sprite = _ending.image;
            _curtain.SetActive(false);
            _trigger.enabled = true;
            _trigger.Description = _ending.description;
        }
    }

    public void Hide()
    {
        _title.SetText("~TITLE~");
        _image.sprite = null;
        _curtain.SetActive(true);
        _trigger.enabled = false;
    }
}
