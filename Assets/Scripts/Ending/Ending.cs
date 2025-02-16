using UnityEngine;

[CreateAssetMenu(fileName = "NewEnding", menuName = "Game/Ending", order = 0)]
public class Ending : StandartEnding
{
    public EndingManager.AxisRange axisConspiracyToScience;
    public EndingManager.AxisRange axisConservatismToProgress;
    public EndingManager.AxisRange axisCommunismToCapitalism;
    public EndingManager.AxisRange axisAuthoritarianismToDemocracy;
    public EndingManager.AxisRange axisPacifismToMilitarism;

}
