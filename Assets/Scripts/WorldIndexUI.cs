using UnityEngine;
using UnityEngine.UI;

public class WorldIndexUI : MonoBehaviour
{
    [SerializeField] private Slider _conspiracyToScienceSlider, _conservatismToProgressSlider, _communismToCapitalismSlider, _authoritarianismToDemocracySlider, _pacifismToMilitarismSlider;

    private void Awake()
    {
        GlobalEventManager.OnChangeAxis += UpdateAxisSliders;
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnChangeAxis -= UpdateAxisSliders;
    }

    private void UpdateAxisSliders(int _conspiracyToScienceValue, int _conservatismToProgressValue, int _communismToCapitalismValue, int _authoritarianismToDemocracyValue, int _pacifismToMilitarismValue)
    {
        _conspiracyToScienceSlider.value = _conspiracyToScienceValue / 2;
        _conservatismToProgressSlider.value = _conservatismToProgressValue / 2;
        _communismToCapitalismSlider.value = _communismToCapitalismValue / 2;
        _authoritarianismToDemocracySlider.value = _authoritarianismToDemocracyValue / 2;
        _pacifismToMilitarismSlider.value = _pacifismToMilitarismValue / 2;
    }
}
