using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem current;

    public Tooltip tooltip;

    private void Start()
    {
        current = this;
    }

    public static void Show(string text)
    {
        current.tooltip.SetText(text);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
