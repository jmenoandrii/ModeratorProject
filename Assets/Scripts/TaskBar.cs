using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskBar : MonoBehaviour
{
    [SerializeField] private TaskBarElement[] _elementList;

    private void Awake()
    {
        GlobalEventManager.OnAppOpen += AddBarElement;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAppOpen -= AddBarElement;
    }

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
