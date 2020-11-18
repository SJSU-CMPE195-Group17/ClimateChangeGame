using UnityEngine;

[System.Serializable]
public class Choice
{
    //choice description
    public string text;

    //resources required to make choice
    public int moneyRequired;
    public int scienceRequired;
    public int globalCoopRequired;
    public int educationRequired;

    //direct change in metrics if any
    public float globalTempDelta;
    public float oceanTempDelta;
    public float seaLevelDelta;
    public float iceSheetDelta;
    public float co2Delta;

    //indirect change in metrics (changes the delta value)
    public float globalTempDeltaDelta;
    public float oceanTempDeltaDelta;
    public float seaLevelDeltaDelta;
    public float iceSheetDeltaDelta;
    public float co2DeltaDelta;
}
