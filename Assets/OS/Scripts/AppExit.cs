using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppExit : MonoBehaviour
{
    public GameObject app;

    private void Awake()
    {
        // protection
        if (app == null)
            app = gameObject.GetComponent<GameObject>();
    }

    public void ExitApp()
    {
        app.SetActive(false);
    }
}
