using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndingResetButton : MonoBehaviour
{
    [SerializeField] private EndingCurtain[] _endingCurtains;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => GlobalEventManager.CallOnEndingReset(_endingCurtains)
        );
    }
}
