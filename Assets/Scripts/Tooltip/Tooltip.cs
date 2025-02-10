using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _description;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int _charWrapLimit;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string text)
    {
        _description.text = text;
        _layoutElement.enabled = (_description.text.Length > _charWrapLimit) ? true : false;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            mousePosition,
            Camera.main,
            out anchoredPos
        );
        _rectTransform.anchoredPosition = anchoredPos;
    }


}
