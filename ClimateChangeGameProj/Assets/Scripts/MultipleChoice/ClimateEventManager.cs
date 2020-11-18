﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClimateEventManager : MonoBehaviour
{
    public static ClimateEventManager instance;
    public ClimateEvent[] events;
    private ClimateEvent currentEvent;

    public TextMeshProUGUI title;
    public TextMeshProUGUI prompt;
    public GameObject buttonA, buttonB, buttonC, buttonD;

    // Start is called before the first frame update
    void Start()
    {
        print("Event Manager Start");
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        AnyEventsTriggered();
    }

    private void LoadEvent(ClimateEvent ce)
    {
        MainGameUI.instance.ShowEvent();
        currentEvent = ce;

        print("Load Event:" + ce.title);
        title.text = ce.title;
        prompt.text = ce.prompt;

        buttonA.GetComponentInChildren<TextMeshProUGUI>().text = ce.choices[0].text;
        if (ce.choices.Length > 1)
            buttonB.GetComponentInChildren<TextMeshProUGUI>().text = ce.choices[1].text;
        if (ce.choices.Length > 2)
            buttonC.GetComponentInChildren<TextMeshProUGUI>().text = ce.choices[2].text;
        if (ce.choices.Length > 3)
            buttonD.GetComponentInChildren<TextMeshProUGUI>().text = ce.choices[3].text;
    }

    private void AnyEventsTriggered()
    {
        float glblTemp = MainGameBackend.instance.glblTempVal;
        float oceanTemp = MainGameBackend.instance.oceanTempVal;
        float seaLvl = MainGameBackend.instance.seaLvlVal;
        float iceSheet = MainGameBackend.instance.iceSheetVal;
        float co2Cond = MainGameBackend.instance.co2Val;

        foreach (ClimateEvent ce in events)
        {
            if(glblTemp >= ce.glblTempCond && oceanTemp >= ce.oceanTempCond && seaLvl >= ce.seaLvlCond && iceSheet >= ce.iceSheetCond && co2Cond >= ce.co2Cond)
            {
                LoadEvent(ce);
            }
        }
    }

    public void optionA()
    {
        resolveOptionSelect(0);
    }

    public void optionB()
    {
        resolveOptionSelect(1);
    }

    public void optionC()
    {
        resolveOptionSelect(2);
    }

    public void optionD()
    {
        resolveOptionSelect(3);
    }

    private void resolveOptionSelect(int option)
    {
        int[] resourceValues = MainGameBackend.instance.getResourceValues();
        int updatedMoneyVal = resourceValues[0] - currentEvent.choices[option].moneyRequired;
        int updatedScienceVal = resourceValues[1] - currentEvent.choices[option].scienceRequired;
        int updatedGlobalCoopVal = resourceValues[2] - currentEvent.choices[option].globalCoopRequired;
        int updatedEducationVal = resourceValues[3] - currentEvent.choices[option].educationRequired;
        MainGameBackend.instance.setResourceValues(updatedMoneyVal, updatedScienceVal, updatedGlobalCoopVal, updatedEducationVal);
        MainGameUI.instance.EventEnded();
    }
}
