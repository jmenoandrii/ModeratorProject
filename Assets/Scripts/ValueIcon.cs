using UnityEngine;

public class ValueIcon : MonoBehaviour
{
    private Animation _anim;

    private void Start()
    {
        _anim = GetComponent<Animation>();
    }

    public void Play()
    {
        _anim.Play();
    }
}
