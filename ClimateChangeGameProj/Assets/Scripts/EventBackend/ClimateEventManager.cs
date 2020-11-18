using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.UI;

public class ClimateEventManager : MonoBehaviour
{
    public static ClimateEventManager instance;
    public ClimateEvent[] events;
    private int currentEventIndex;

    public TextMeshProUGUI title;
    public TextMeshProUGUI prompt;
    public GameObject[] buttons;

    public static int MAX_NUMBER_OF_EVENTS = 25;
    public const string path = "/Resources/EventFlags.xml";

    void Awake()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Resources"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Resources");
        if (!File.Exists(Application.persistentDataPath + path))
            initializeXml();
    }

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


    private void LoadEvent(int climateEventIndex)
    {
        //delay for x seconds
        //MainGameUI.hidestuff
        currentEventIndex = climateEventIndex;
        ClimateEvent ce = events[climateEventIndex];
        if(ce.location >= 0)
            StartCoroutine(MainGameUI.instance.ShowEvent(ce.location));
 

        print("Load Event:" + ce.title);
        title.text = ce.title;
        prompt.text = ce.prompt;
        int[] resourceValues = MainGameBackend.instance.getResourceValues();
        int moneyVal = resourceValues[0];
        int scienceVal = resourceValues[1];
        int globalCoopVal = resourceValues[2];
        int educationVal = resourceValues[3];

        for (int i = 0; i < buttons.Length; i++)
        {
            if(i < ce.choices.Length)
            {
                Choice c = ce.choices[i];
                buttons[i].SetActive(true);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = ce.choices[i].text;
                if(c.moneyRequired > moneyVal || c.scienceRequired > scienceVal
                    || c.globalCoopRequired > globalCoopVal || c.educationRequired > educationVal)
                {
                    buttons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    buttons[i].GetComponent<Button>().interactable = true;
                }

            }
            //if there is less than i choices, disable the unused buttons
            else
            {
                buttons[i].SetActive(false);
            }
        }

    }

    private void AnyEventsTriggered()
    {
        float[] metricValues = MainGameBackend.instance.getMetricValues();
        float glblTemp = metricValues[0];
        float oceanTemp = metricValues[1];
        float seaLvl = metricValues[2];
        float iceSheet = metricValues[3];
        float co2Cond = metricValues[4];

        bool[] triggered = getFlagValues();

        for (int i = 0; i < events.Length; i++)
        {
            ClimateEvent ce = events[i];
            if (glblTemp >= ce.glblTempCond && oceanTemp >= ce.oceanTempCond && seaLvl >= ce.seaLvlCond && iceSheet >= ce.iceSheetCond && co2Cond >= ce.co2Cond
                && !triggered[i])
            {
                LoadEvent(i);
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
        if(currentEventIndex < 0)
        {
            Debug.LogWarning("currentEventIndex out of bounds");
            return;
        }
        //set rotate of camera
        CameraController.instance.rotateDestination = -1;

        ClimateEvent currentEvent = events[currentEventIndex];
        //set updated resource values
        int[] resourceValues = MainGameBackend.instance.getResourceValues();
        int updatedMoneyVal = resourceValues[0] - currentEvent.choices[option].moneyRequired;
        int updatedScienceVal = resourceValues[1] - currentEvent.choices[option].scienceRequired;
        int updatedGlobalCoopVal = resourceValues[2] - currentEvent.choices[option].globalCoopRequired;
        int updatedEducationVal = resourceValues[3] - currentEvent.choices[option].educationRequired;
        MainGameBackend.instance.setResourceValues(updatedMoneyVal, updatedScienceVal, updatedGlobalCoopVal, updatedEducationVal);

        //update metrics if changed
        float[] metricValues = MainGameBackend.instance.getMetricValues();
        float glblTempVal = metricValues[0] + currentEvent.choices[option].globalTempDelta;
        float oceanTempVal = metricValues[1] + currentEvent.choices[option].oceanTempDelta;
        float seaLvlVal = metricValues[2] + currentEvent.choices[option].seaLevelDelta;
        float iceSheetVal = metricValues[3] + currentEvent.choices[option].iceSheetDelta;
        float co2Val = metricValues[4] + currentEvent.choices[option].co2Delta;

        float glblTempDelta = metricValues[5] + currentEvent.choices[option].globalTempDeltaDelta;
        float oceanTempDelta = metricValues[6] + currentEvent.choices[option].globalTempDeltaDelta;
        float seaLvlDelta = metricValues[7] + currentEvent.choices[option].globalTempDeltaDelta;
        float iceSheetDelta = metricValues[8] + currentEvent.choices[option].globalTempDeltaDelta;
        float co2Delta = metricValues[9] + currentEvent.choices[option].globalTempDeltaDelta;

        float date = metricValues[10];
        int currYear = (int)date;
        int currSeason = (int)((date - currYear) * 4);

        MainGameBackend.instance.setMetricValues(glblTempVal, oceanTempVal, seaLvlVal, iceSheetVal, co2Val,
            glblTempDelta, oceanTempDelta, seaLvlDelta, iceSheetDelta, co2Delta,
            currYear, currSeason);

        //update flag
        updateXML(currentEventIndex, true);
        //getFlagValues();

        currentEventIndex = -1;

        MainGameUI.instance.EventEnded();
    }

    private void initializeXml()
    {
        EventFlagContainer eventFlagContainer = new EventFlagContainer();
        for(int i = 0; i < MAX_NUMBER_OF_EVENTS; i++)
        {
            eventFlagContainer.EventFlagContainers.Add(new EventFlag
            {
                eventIndex = i,
                triggered = false
            });
        }

        print("File Created" + Application.persistentDataPath + path);
        XmlOperation.Serialize(eventFlagContainer, Application.persistentDataPath + path);
    }

    private void updateXML(int index, bool flag)
    {
        bool[] oldFlags = getFlagValues();
        EventFlagContainer eventFlagContainer = new EventFlagContainer();
        for (int i = 0; i < MAX_NUMBER_OF_EVENTS; i++)
        {
            if(i == index)
            {
                eventFlagContainer.EventFlagContainers.Add(new EventFlag
                {
                    eventIndex = i,
                    triggered = flag
                });
            }
            else
            {
                eventFlagContainer.EventFlagContainers.Add(new EventFlag
                {
                    eventIndex = i,
                    triggered = oldFlags[i]
                });
            }
        }
        
        if (!File.Exists(Application.persistentDataPath + path))
            Debug.LogWarning("File " + Application.persistentDataPath + path + " not found");
        XmlOperation.Serialize(eventFlagContainer, Application.persistentDataPath + path);
    }

    private void updateXML(bool[] newFlags)
    {
        EventFlagContainer eventFlagContainer = new EventFlagContainer();
        for (int i = 0; i < MAX_NUMBER_OF_EVENTS; i++)
        {
            eventFlagContainer.EventFlagContainers.Add(new EventFlag
            {
                eventIndex = i,
                triggered =newFlags[i]
            });
        }

        if (!File.Exists(Application.persistentDataPath + path))
            Debug.LogWarning("File " + Application.persistentDataPath + path + " not found");
        XmlOperation.Serialize(eventFlagContainer, Application.persistentDataPath + path);
    }

    public void resetXML()
    {
        EventFlagContainer eventFlagContainer = new EventFlagContainer();
        for (int i = 0; i < MAX_NUMBER_OF_EVENTS; i++)
        {
            eventFlagContainer.EventFlagContainers.Add(new EventFlag
            {
                eventIndex = i,
                triggered = false
            });
        }

        if (!File.Exists(Application.persistentDataPath + path))
            Debug.LogWarning("File " + Application.persistentDataPath + path + " not found");
        XmlOperation.Serialize(eventFlagContainer, Application.persistentDataPath + path);
    }

    private bool[] getFlagValues()
    {
        XDocument doc = XDocument.Load(Application.persistentDataPath + path);
        List<XElement> allMetrics = doc.Root.Descendants().ToList();
        bool[] flags = new bool[MAX_NUMBER_OF_EVENTS];

        for(int i = 0; i < MAX_NUMBER_OF_EVENTS; i++)
        {
            int index = i;
            var result = allMetrics.Elements("EventFlag").
                First(x => x.Element("Index").Value.Equals("" + index));
            string boolText = result.Element("Triggered").Value;
            bool flag = bool.Parse(boolText);
            //print("GET Flag:" + index + " " + flag);
            flags[i] = flag;
        }
        return flags;
    }

}
