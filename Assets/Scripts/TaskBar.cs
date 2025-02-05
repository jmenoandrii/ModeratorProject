using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskBar : MonoBehaviour
{
    [SerializeField] private PopUp popUp;
    [SerializeField] private TaskBarElement[] _elementList;
    public static TaskBar instance;
    private int _activeAppCount;
    public bool IsAbleToAddBarElement { get => _activeAppCount < _elementList.Length; }

    private void Start()
    {
        // singleton initialization
        if (instance == null)
            instance = this;
        else
            Debug.LogError($"ERR[{gameObject.name}]: Minesweeper must be a singleton");
    }

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
        if (!IsAbleToAddBarElement) return;

        foreach (TaskBarElement element in _elementList)
        {
            if (!element.IsActive)
            {
                IncreaseActiveAppCount();
                element.SetApp(app);
                element.Show();
                break;
            }
        }
    }

    private void IncreaseActiveAppCount()
    {
        if (IsAbleToAddBarElement)
            _activeAppCount++;
    }

    public void DecreaseActiveAppCount()
    {
        if (_activeAppCount > 0)
            _activeAppCount--;
    }

    public void CallOverflowPopUp() 
    { 
        popUp.Show(); 
    }
}
