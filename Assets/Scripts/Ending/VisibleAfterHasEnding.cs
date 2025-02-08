using UnityEngine;

public class VisibleAfterHasEnding : MonoBehaviour
{
    [SerializeField] private GameObject _targetObj;
    
    private void Start()
    {
        _targetObj.SetActive(EndingBookManager.instance.HasAnyEndingUnlocked());
    }
}
