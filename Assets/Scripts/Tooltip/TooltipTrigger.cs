using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _description;
    public string Description { set { if (string.IsNullOrEmpty(_description)) _description = value; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(_description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}
