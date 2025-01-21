using UnityEngine;

public class PopUp : MonoBehaviour
{
    private void Show()
    {
        gameObject.SetActive(false);
    }

    private void Hide()
    {
        gameObject.SetActive(true);
    }
}
