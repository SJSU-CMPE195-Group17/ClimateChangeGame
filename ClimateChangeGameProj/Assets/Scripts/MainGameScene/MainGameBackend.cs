using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.UI;

public class MainGameBackend : MonoBehaviour
{
    const string RESOURCE_PATH = ResourceSerializer.path;
    const string METRIC_PATH = ClimateMetricSerializer.path;
    //private XDocument doc = XDocument.Load(DATABASE_PATH);

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scienceText;
    public TextMeshProUGUI globalCoopText;
    public TextMeshProUGUI educationText;

    public TextMeshProUGUI glblTempText;
    public TextMeshProUGUI oceanTempText;
    public TextMeshProUGUI seaLvlText;
    public TextMeshProUGUI iceSheetText;
    public TextMeshProUGUI co2Text;
    public Slider glblTempSlider;
    public Slider oceanTempSlider;
    public Slider seaLvlSlider;
    public Slider iceSheetSlider;
    public Slider co2Slider;

    private static string[] seasons = { "Spring", "Summer", "Fall", "Winter" };

    public const float GLOBAL_TEMP_BASE = 1.78f;
    public const float OCEAN_TEMP_BASE = 1.25f;
    public const float SEA_LEVEL_BASE = 94f;
    public const float ICE_SHEET_BASE = 7200f;
    public const float CO2_BASE = 1050f;

    public const float GLOBAL_TEMP_DELTA_BASE = 0.01f;
    public const float OCEAN_TEMP_DELTA_BASE = 0.01f;
    public const float SEA_LEVEL_DELTA_BASE = 0.8f;
    public const float ICE_SHEET_DELTA_BASE = 37f;
    public const float CO2_DELTA_BASE = 1.75f;
    public const int START_YEAR = 2020;
    public const int START_SEASON = 0;

    //estimate maxes for game over, we need to do research on what metrics would be catastrophic
    public const float GLOBAL_TEMP_MAX = 10.68f;
    public const float OCEAN_TEMP_MAX = 7.5f;
    public const float SEA_LEVEL_MAX = 564f;
    public const float ICE_SHEET_MAX = 43200f;
    public const float CO2_MAX = 6300f;

    public Color MinBarColor = Color.yellow;
    public Color MaxBarColor = Color.red;

    public GameObject mymultiplechoicequestions;

    public static MainGameBackend instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        updateGui();
        //mymultiplechoicequestions.GetComponent<multiplechoicequestions>().AddQuestions("a text", "b text", "c text", "d text");
    }

    void update(){
        
    }

    private void Update()
    {

        if (Input.GetKeyDown("f1"))
        {
            setResourceValues(40, 40, 40, 40);
            setMetricValues(GLOBAL_TEMP_BASE, OCEAN_TEMP_BASE, SEA_LEVEL_BASE, ICE_SHEET_BASE, CO2_BASE, 
                GLOBAL_TEMP_DELTA_BASE, OCEAN_TEMP_DELTA_BASE, SEA_LEVEL_DELTA_BASE, ICE_SHEET_DELTA_BASE, CO2_DELTA_BASE, 
                START_YEAR, START_SEASON);
        }
        if (Input.GetKeyDown("f2"))
        {
            setResourceValues(161, 125, 87, 108);
            setMetricValues(8.1f, 6.2f, 405.6f, 30238.8f, 3552.3f, 
                0.03f, 0.03f, 2f, 60f, 6f, 2084, 3);
        }
        if (Input.GetKeyDown("space"))
        {
            print("PROGRESS");
            progressTime();
        }
    }

    private void updateGui()
    {
        //Loads all saved quantity into variables 
        int[] resourceValues = getResourceValues();
        int moneyVal = resourceValues[0];//getResourceValues("Money");
        int scienceVal = resourceValues[1];//getResourceValues("Science");
        int globalCoopVal = resourceValues[2];//getResourceValues("Global Cooperation");
        int educationVal = resourceValues[3];//getResourceValues("Education");

        float[] metricValues = getMetricValues();
        float glblTempVal = metricValues[0];
        float oceanTempVal = metricValues[1];
        float seaLvlVal = metricValues[2];
        float iceSheetVal = metricValues[3];
        float co2Val = metricValues[4];
        float date = metricValues[10];
        int currYear = (int)date;
        int currSeason = (int)((date - currYear) * 4);

        //update text values
        moneyText.text = "" + moneyVal + "M";
        scienceText.text = "" + scienceVal;
        globalCoopText.text = "" + globalCoopVal;
        educationText.text = "" + educationVal;
        dateText.text = seasons[currSeason] + ", " + currYear;

        glblTempText.text = "" + glblTempVal + " °F";
        oceanTempText.text = "" + oceanTempVal + " °F";
        seaLvlText.text = "" + seaLvlVal + " mm";
        iceSheetText.text = "" + iceSheetVal + " Gt";
        co2Text.text = "" + co2Val + " ppm";

        //update sliders in stats view, the second line of each pair changes the color of the bar from yellow-green to red. (these can be changed in the object
        //holding this script)
        glblTempSlider.value = glblTempVal / GLOBAL_TEMP_MAX;
        glblTempSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.Lerp(MinBarColor, MaxBarColor, glblTempSlider.value);

        oceanTempSlider.value = oceanTempVal / OCEAN_TEMP_MAX;
        oceanTempSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.Lerp(MinBarColor, MaxBarColor, oceanTempSlider.value);

        seaLvlSlider.value = seaLvlVal / SEA_LEVEL_MAX;
        seaLvlSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.Lerp(MinBarColor, MaxBarColor, seaLvlSlider.value);

        iceSheetSlider.value = iceSheetVal / ICE_SHEET_MAX;
        iceSheetSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.Lerp(MinBarColor, MaxBarColor, iceSheetSlider.value);

        co2Slider.value = (co2Val-300f) / CO2_MAX;
        co2Slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.Lerp(MinBarColor, MaxBarColor, co2Slider.value);
    }

    //called for debugging
    public void progressTime()
    {
        progressSeasons(13);
        updateGui();
    }

    //called when play button is pressed
    public void play()
    {
        progressSeasons(13);
    }

    //numOfSeasons = number of seasons to increase by
    private void progressSeasons(int numOfSeasons)
    {

        //climate change metrics here will change based off of climate change sim
        //in this extremely simple sim, it just increases by arbitrary values
        float[] metricValues = getMetricValues();
        float glblTempVal = metricValues[0];
        float oceanTempVal = metricValues[1];
        float seaLvlVal = metricValues[2];
        float iceSheetVal = metricValues[3];
        float co2Val = metricValues[4];

        float glblTempDelta = metricValues[5];
        float oceanTempDelta = metricValues[6];
        float seaLvlDelta = metricValues[7];
        float iceSheetDelta = metricValues[8];
        float co2Delta = metricValues[9];
        float date = metricValues[10];
        int currYear = (int)date;
        int currSeason = (int)((date - currYear) * 4);

        glblTempVal += glblTempDelta * numOfSeasons;
        oceanTempVal += oceanTempDelta * numOfSeasons;
        seaLvlVal += seaLvlDelta * numOfSeasons;
        iceSheetVal += iceSheetDelta * numOfSeasons;
        co2Val += co2Delta * numOfSeasons;

        //increase season and year by appropriate amount
        if (currSeason + numOfSeasons > 3)
        {
            currYear += numOfSeasons / 4;
        }
        currSeason = (currSeason + numOfSeasons) % 4;

        setMetricValues(glblTempVal, oceanTempVal, seaLvlVal, iceSheetVal, co2Val,
        glblTempDelta, oceanTempDelta, seaLvlDelta, iceSheetDelta, co2Delta,
        currYear, currSeason);
    }

    //Retrieve resource quantity from xml file
    public int getResourceValues(string resourceName) {
        XDocument doc = XDocument.Load(Application.persistentDataPath + RESOURCE_PATH);
        
        //XDocument doc = Resources.Load<XDocument>("Resources.xml");
        List <XElement> allResources = doc.Root.Descendants().ToList();
        
        var result = allResources.Elements("Resource").
            First(x => x.Element("Name").Value.Equals(resourceName));

        string amountText = result.Element("Amount").Value;
        int parsedResourceAmount = Int32.Parse(amountText);

        //print("GET Resource: " + resourceName + " Amount: " + parsedResourceAmount);
        return parsedResourceAmount;
    }

    public int[] getResourceValues()
    {
        XDocument doc = XDocument.Load(Application.persistentDataPath + RESOURCE_PATH);
        List<XElement> allResources = doc.Root.Descendants().ToList();
        int[] resourceValues = new int[4];
        string[] resourceNames = { "Money", "Science", "Global Cooperation", "Education" };
        for(int i = 0; i < resourceValues.Length; i++)
        {
            var result = allResources.Elements("Resource").
                First(x => x.Element("Name").Value.Equals(resourceNames[i]));

            string amountText = result.Element("Amount").Value;
            int parsedResourceAmount = Int32.Parse(amountText);

            //print("GET Resource: " + resourceNames[i] + " Amount: " + parsedResourceAmount);
            resourceValues[i] = parsedResourceAmount;
        }
        return resourceValues;
    }

    public void setResourceValues(int updatedMoneyVal, int updatedScienceVal, int updatedGlobalCoopVal, int updatedEducationVal)
    {
        print("Set Resources to: " + updatedMoneyVal + " " + updatedScienceVal + " " + updatedGlobalCoopVal + " " + updatedEducationVal);
        ResourceSerializer.updateXml(updatedMoneyVal, updatedScienceVal, updatedGlobalCoopVal, updatedEducationVal);
        updateGui();
    }

    public float[] getMetricValues()
    {
        XDocument doc = XDocument.Load(Application.persistentDataPath + METRIC_PATH);
        List<XElement> allMetrics = doc.Root.Descendants().ToList();
        float[] metricValues = new float[11];
        string[] metricNames = { "GlobalTemp", "OceanTemp", "SeaLevel", "IceSheets", "Co2",
        "GlobalTempDelta", "OceanTempDelta", "SeaLevelDelta", "IceSheetsDelta", "Co2Delta", "Date"};
        
        for (int i = 0; i < metricValues.Length; i++)
        {
            var result = allMetrics.Elements("ClimateMetric").
                First(x => x.Element("Name").Value.Equals(metricNames[i]));
            string valueText = result.Element("Value").Value;
            float parsedMetricAmount = float.Parse(valueText);

            //print("GET Metric: " + metricNames[i] + " Amount: " + parsedMetricAmount);
            metricValues[i] = parsedMetricAmount;
        }
        return metricValues;
    }


    public void setMetricValues(float updatedGlblTempVal, float updatedOceanTempVal, float updatedSeaLvlVal, float updatedIceSheetVal, float updatedCo2Val,
        float glblTempValDelta, float oceanTempValDelta, float seaLvlValDelta, float iceSheetValDelta, float co2ValDelta,
        int year, int season)
    {
        //print("Set Metrics to: " + updatedGlblTempVal + " " + updatedOceanTempVal + " " + updatedSeaLvlVal + " " + updatedIceSheetVal + " " + updatedCo2Val);
        //print("Set Deltas to: " + glblTempValDelta + " " + oceanTempValDelta + " " + seaLvlValDelta + " " + iceSheetValDelta + " " + co2ValDelta);
        //print("Set Date to: " + year + " " + season);
        ClimateMetricSerializer.updateXml(updatedGlblTempVal, updatedOceanTempVal, updatedSeaLvlVal, updatedIceSheetVal, updatedCo2Val,
            glblTempValDelta, oceanTempValDelta, seaLvlValDelta, iceSheetValDelta, co2ValDelta,
            year, season);
        updateGui();
    }
}
