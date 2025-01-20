using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerApp : App
{
    [SerializeField]
    private TMP_Text _counterText;
    private int _counter = 0;

    public void Click() {
        _counter++;
        _counterText.SetText(_counter.ToString());
    }
}
