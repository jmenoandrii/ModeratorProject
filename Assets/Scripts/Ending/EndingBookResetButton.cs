using UnityEngine;

public class EndingBookResetButton : MonoBehaviour
{
    [SerializeField] private GameObject _page;

    public void ResetEndings()
    {
        GlobalEventManager.CallOnResetEndings();
        Destroy(_page);
    }
}