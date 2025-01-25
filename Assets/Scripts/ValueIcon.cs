using UnityEngine;

public class ValueIcon : MonoBehaviour
{
    private Animation _anim;

    private void Awake()
    {
        _anim = GetComponent<Animation>();
    }

    public void Play()
    {
        gameObject.SetActive(true);
        _anim.Play();
    }

    public void Rewind()
    {
        gameObject.SetActive(false);
    }
}
