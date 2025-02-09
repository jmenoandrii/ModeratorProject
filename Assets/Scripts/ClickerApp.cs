using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerApp : App
{
    [SerializeField]
    private TMP_Text _counterText;
    private int _counter = 0;
    [SerializeField] private AudioSource _clickSound;

    public void Click() {
        _counter++;
        _counterText.SetText(_counter.ToString());
        if (!_clickSound.isPlaying)
        {
            _clickSound.pitch = Random.Range(0.75f, 1.2f);
            _clickSound.Play();
        }
    }
}
