using UnityEngine;

public class VisibleAfterAllEnding : MonoBehaviour
{
    [SerializeField] private GameObject _targetObj;

    private void Start()
    {
        _targetObj.SetActive(EndingBookManager.instance.AreAllEndingsUnlocked());
    }
}
