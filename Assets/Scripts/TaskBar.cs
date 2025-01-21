using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskBar : Bar
{
    private void Awake()
    {
        GlobalEventManager.OnAppOpen += AddBarElement;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAppOpen -= AddBarElement;
    }
}
