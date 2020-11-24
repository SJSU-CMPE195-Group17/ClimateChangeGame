using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialBackend : MonoBehaviour
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

    private int tutorialState;
    public TextMeshProUGUI tutorialTitle;
    public TextMeshProUGUI tutorialText;
    public GameObject dateView;
    public GameObject statsButton;
    public GameObject optionsButton;
    public GameObject bottomResources;
    public GameObject playButton;
    public GameObject loading;

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

    public static TutorialBackend instance;

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
    // Start is called before the first frame update
    void Start()
    {
        TutorialEventManager.instance.initializeXml();
        ClimateMetricSerializer.instance.initializeXml();
        ResourceSerializer.instance.initializeXml();

        tutorialState = 0;
        dateView.SetActive(true);
        tutorialTitle.text = "Climate Change Concerns on the Rise";
        tutorialText.text = "Scientists around the world have published concerning studies on the path of climate change. Despite rising concerns in the 2010's, " +
            "little has been done to mitigate climate change. Now, with increased worries about our future, the United Nations has appointed you as the new head " +
            "of the Intergovernmental Panel on Climate Change (IPCC). It will be up to you to guide the world through the dangers of climate change.";
    }

    // Update is called once per frame
    void Update()
    {
        updateGui();
    }

    public void progressTutorial()
    {
        tutorialState++;

        switch (tutorialState)
        {
            case 1:
                statsButton.SetActive(true);
                tutorialTitle.text = "Statistics and Objectives";
                tutorialText.text = "You can view the current state of the climate with the Statistics button on the top right. If the measured conditions get beyond " +
                    "the thresholds indicated, we can expect to see apocalyptic consequences around the world, so do your best to keep these conditions low. If you " +
                    "manage to keep these metrics below the designated thresholds until 2100, we can expect to keep the Earth habitable for future centuries.";
                break;
            case 2:
                bottomResources.SetActive(true);
                tutorialTitle.text = "Resource Spending";
                tutorialText.text = "On the bottom of the screen, you can see the four resources needed to combat climate change: capital, research, " +
                    "global cooperation, and education. Throughout this game, events caused by climate change will occur. You will be presented with several measures " +
                    "you can take as the head of the IPCC. Each measure will require one of these resources.";
                break;
            case 3:
                playButton.SetActive(true);
                tutorialTitle.text = "Resource Earning";
                tutorialText.text = "You can collect resources by pressing the play button on the bottom right. You will be presented with a puzzle board where you can " +
                    "match icons to collect resources. Note that you will get more resources by creating longer chains of matches! You will have a limited amount of time " +
                    "to collect these resources, so be quick.";
                break;
            case 4:
                tutorialTitle.text = "Good Luck";
                tutorialText.text = "Do your best to collect enough resources throughout the puzzle games so you can successfully mitigate the impact of climate change! Note " +
                    "that all events that can occur are grounded in reality. It's likely that you'll see some of these events during your lifetime. Visit ndrc.org, sierraclub.org, " +
                    "or ipcc.ch to find out what you can do to help protect our planet.";
                break;
            case 5:
                StartCoroutine(nextScene());
                break;
        }
    }

    private IEnumerator nextScene()
    {
        loading.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1); //index of MainGame
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

        co2Slider.value = (co2Val - 300f) / CO2_MAX;
        co2Slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.Lerp(MinBarColor, MaxBarColor, co2Slider.value);
    }

    public int getResourceValues(string resourceName)
    {
        XDocument doc = XDocument.Load(Application.persistentDataPath + RESOURCE_PATH);

        //XDocument doc = Resources.Load<XDocument>("Resources.xml");
        List<XElement> allResources = doc.Root.Descendants().ToList();

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
        for (int i = 0; i < resourceValues.Length; i++)
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
