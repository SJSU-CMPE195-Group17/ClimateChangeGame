using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClimateEvent
{
    public string title;
    public string prompt;
    public string isPositiveEvent;

    public float glblTempCond; //in deg F
    public float oceanTempCond;//in deg F
    public float seaLvlCond;     //in mm
    public float iceSheetCond; //in Gt
    public float co2Cond;       //in ppm

    public int location;
    public Choice[] choices;
    public string articleLink;
}
