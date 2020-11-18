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
    private int moneyVal = 0;
    private int scienceVal = 0;
    private int globalCoopVal = 0;
    private int educationVal = 0;
    private int currYear = 2020;
    private int currSeason = 0;

    public float glblTempVal = 1.78f; //in deg F
    public float oceanTempVal = 1.25f;//in deg F
    public float seaLvlVal = 94f;     //in mm
    public float iceSheetVal = 7200f; //in Gt
    public float co2Val = 1050f;       //in ppm

    //estimate maxes for game over, we need to do research on what metrics would be catastrophic
    private const float GLOBAL_TEMP_MAX = 10.68f;   
    private const float OCEAN_TEMP_MAX = 7.5f;  
    private const float SEA_LEVEL_MAX = 564f; 
    private const float ICE_SHEET_MAX = 43200f; 
    private const float CO2_MAX = 6300f;

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
            setMetricValues(1.78f, 1.25f, 94f, 7200f, 1050f);
        }
        if (Input.GetKeyDown("f2"))
        {
            setResourceValues(161, 125, 87, 108);
            setMetricValues(8.1f, 6.2f, 405.6f, 30238.8f, 3552.3f);
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
        moneyVal = resourceValues[0];//getResourceValues("Money");
        scienceVal = resourceValues[1];//getResourceValues("Science");
        globalCoopVal = resourceValues[2];//getResourceValues("Global Cooperation");
        educationVal = resourceValues[3];//getResourceValues("Education");

        float[] metricValues = getMetricValues();
        glblTempVal = metricValues[0];
        oceanTempVal = metricValues[1];
        seaLvlVal = metricValues[2];
        iceSheetVal = metricValues[3];
        co2Val = metricValues[4];

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

    //would be called after a puzzle game is played
    public void progressTime()
    {
        progressSeasons(5);
        updateGui();
    }

    public void play()
    {
        progressSeasons(13);
    }

    //numOfSeasons = number of seasons to increase by
    private void progressSeasons(int numOfSeasons)
    {
        //resource addition values will be determined by resources gathered during puzzle game
        moneyVal += 60 * numOfSeasons;
        scienceVal += 50 * numOfSeasons;
        globalCoopVal += 40 * numOfSeasons;
        educationVal += 30 * numOfSeasons;

        //climate change metrics here will change based off of climate change sim
        //in this extremely simple sim, it just increases by arbitrary values
        float[] metricValues = getMetricValues();
        glblTempVal = metricValues[0];
        oceanTempVal = metricValues[1];
        seaLvlVal = metricValues[2];
        iceSheetVal = metricValues[3];
        co2Val = metricValues[4];

        glblTempVal += 0.01f * numOfSeasons; //0.356f
        oceanTempVal += 0.01f * numOfSeasons; //0.25f
        seaLvlVal += 0.8f * numOfSeasons; // 18.8f
        iceSheetVal += 37f * numOfSeasons; //1440f
        co2Val += 1.75f * numOfSeasons; // 210f

        setMetricValues(glblTempVal, oceanTempVal, seaLvlVal, iceSheetVal, co2Val);

        //increase season and year by appropriate amount
        if (currSeason + numOfSeasons > 3)
        {
            currYear += 1;
        }
        currSeason = (currSeason + numOfSeasons) % 4;
    }

    public void changeResourcesAndStatistics(int myMoneyVal, int myScienceVal, int myGlobalCoopVal, int myEducationVal)
    {
        moneyVal += myMoneyVal;
        scienceVal += myScienceVal;
        globalCoopVal += myGlobalCoopVal;
        educationVal += myEducationVal;
        
        updateGui();
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
        float[] metricValues = new float[5];
        string[] metricNames = { "GlobalTemp", "OceanTemp", "SeaLevel", "IceSheets", "Co2" };
        
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


    public void setMetricValues(float updatedGlblTempVal, float updatedOceanTempVal, float updatedSeaLvlVal, float updatedIceSheetVal, float updatedCo2Val)
    {
        print("Set Metrics to: " + updatedGlblTempVal + " " + updatedOceanTempVal + " " + updatedSeaLvlVal + " " + updatedIceSheetVal + " " + updatedCo2Val);
        ClimateMetricSerializer.updateXml(updatedGlblTempVal, updatedOceanTempVal, updatedSeaLvlVal, updatedIceSheetVal, updatedCo2Val);
        updateGui();
    }
}
