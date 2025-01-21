using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private TaskBarElement[] _elementList;

    protected void AddBarElement(App app)
    {
        foreach (TaskBarElement element in _elementList)
        {
            if (!element.IsActive)
            {
                element.SetApp(app);
                element.Show();
                break;
            }
        }
    }
}
