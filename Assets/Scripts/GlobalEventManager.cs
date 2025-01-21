using System;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static event Action<App> OnAppOpen;
    public static event Action<App> OnAppClose;
    public static event Action OnSortTaskBar;

    public static void CallOnAppOpen(App app)
    {
        OnAppOpen?.Invoke(app);
    }

    public static void CallOnAppClose(App app)
    {
        OnAppClose?.Invoke(app);
    }

    public static void SendSortTaskBar()
    {
        OnSortTaskBar?.Invoke();
    }
}
