using UnityEngine;

public class PageLink : MonoBehaviour
{
    [SerializeField] protected PageParameter _pageParameter;

    private void Awake()
    {
        // protection
        if (_pageParameter.prefab == null)
            Debug.LogError($"'{gameObject.name}' doesn't have the setted page prefab");

        if (_pageParameter.icon == null)
            Debug.LogWarning($"'{gameObject.name}' doesn't have the setted page icon");

        if (_pageParameter.prefab == null)
            Debug.LogWarning($"'{gameObject.name}' doesn't have the setted page title");
    }

    public virtual void OpenPage()
    {
        GlobalEventManager.CallOnPageOpen(_pageParameter);
    }
}
