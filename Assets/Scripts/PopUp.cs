using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PopUp : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _audio.Play();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
