using UnityEngine;
using UnityEngine.Events;

public class Virus : RandomEventInvoker
{
    private bool _wasAntivirusOn;

    protected override void Start()
    {
        base.Start();
        _wasAntivirusOn = Antivirus.instance?.IsOn ?? false;
    }

    private void Update()
    {
        if (Antivirus.instance == null) return;

        if (Antivirus.instance.IsOn != _wasAntivirusOn)
        {
            _wasAntivirusOn = Antivirus.instance.IsOn;

            if (_wasAntivirusOn)
            {
                if (_eventCoroutine != null)
                {
                    StopCoroutine(_eventCoroutine);
                    _eventCoroutine = null;
                }
            }
            else
            {
                if (_eventCoroutine == null)
                {
                    _eventCoroutine = StartCoroutine(InvokeRandomEvent());
                }
            }
        }
    }
}
