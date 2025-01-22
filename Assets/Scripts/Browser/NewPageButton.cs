using UnityEngine;

public class NewPageButton : PageLink
{
    public override void OpenPage()
    {
        GlobalEventManager.CallOnNewPage(_pageParameter);
    }
}
