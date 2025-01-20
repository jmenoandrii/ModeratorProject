using System;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static event Action<App> OnAppOpen;
    public static event Action<App> OnAppClose;
    public static event Action OnSortTaskBar;

    public static void SendAppToTaskBarIcon(App app)
    {
        if (OnAppOpen != null) 
            OnAppOpen.Invoke(app);
    }

    public static void SendAppClose(App app)
    {
        if (OnAppClose != null)
            OnAppClose.Invoke(app);
    }

    public static void SendSortTaskBar()
    {
        if (OnSortTaskBar != null)
            OnSortTaskBar.Invoke();
    }
}
