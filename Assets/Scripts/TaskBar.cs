using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskBar : MonoBehaviour
{
    [SerializeField] private TaskBarElement[] _iconList;
    //private int _iconCount = 0;

    private void Awake()
    {
        GlobalEventManager.OnAppOpen += AddTaskBarElement;
        GlobalEventManager.OnSortTaskBar += RemoveTaskBarElement;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnAppOpen -= AddTaskBarElement;
        GlobalEventManager.OnSortTaskBar -= RemoveTaskBarElement;
    }

    private void AddTaskBarElement(App app)
    {
        //Debug.Log($"[+]: {_iconCount}->{_iconCount+1} | {app.name}");
        /*if (_iconCount == _iconList.Length) return;

        _iconList[_iconCount].SetApp(app);
        _iconList[_iconCount].Show();

        _iconCount++;*/
        foreach (TaskBarElement icon in _iconList)
        {
            if (!icon.IsActive)
            {
                icon.SetApp(app);
                icon.Show();
                break;
            }
        }
    }

    private void RemoveTaskBarElement()
    {
        //_iconCount--;
        //Debug.Log($"[-]: {_iconCount+1}->{_iconCount} | {app.name}");

        // sorting
        App app;
        bool foundElem = false;
        for (int i = 0; i < _iconList.Length; i++)
        {
            if (!_iconList[i].IsActive)
            {
                for (int k = i + 1; k < _iconList.Length; k++)
                {
                    if (_iconList[k].IsActive)
                    {
                        app = _iconList[k].GetApp();
                        _iconList[k].Hide();
                        _iconList[i].SetApp(app);
                        _iconList[i].Show();

                        foundElem = true;
                        break;
                    }
                }
                if (!foundElem)
                    break; // if the false was found and others are also false thus the list is sorted
            }
        }
    }
}
