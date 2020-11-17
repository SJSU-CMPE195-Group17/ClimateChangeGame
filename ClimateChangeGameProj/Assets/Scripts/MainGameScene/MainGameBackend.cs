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
    const string LOCAL_PATH = Serializer.path;
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

    private float glblTempVal = 1.78f; //in deg F
    private float oceanTempVal = 1.25f;//in deg F
    private float seaLvlVal = 94f;     //in mm
    private float iceSheetVal = 7200f; //in Gt
    private float co2Val = 425f;       //in ppm

    //estimate maxes for game over, we need to do research on what metrics would be catastrophic
    private const float GLOBAL_TEMP_MAX = 7.2f;
    private const float OCEAN_TEMP_MAX = 7.2f;
    private const float SEA_LEVEL_MAX = 2000f;
    private const float ICE_SHEET_MAX = 50000f;
    private const float CO2_MAX = 1000f;

    public Color MinBarColor = Color.yellow;
    public Color MaxBarColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        updateGui();
    }

    private void Update()
    {
        if (Input.GetKeyDown("f1"))
        {
            setResourceValues(40, 40, 40, 40);
        }
        if (Input.GetKeyDown("f2"))
        {
            setResourceValues(161, 125, 87, 108);
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
        glblTempVal += 0.01f * numOfSeasons;
        oceanTempVal += 0.01f * numOfSeasons;
        seaLvlVal += 0.8f * numOfSeasons;
        iceSheetVal += 37f * numOfSeasons;
        co2Val += 1.75f * numOfSeasons;

        //increase season and year by appropriate amount
        if (currSeason + numOfSeasons > 3)
        {
            currYear += (currSeason + numOfSeasons)/4; 
        }
        currSeason = (currSeason + numOfSeasons) % 4;
    }

    //Retrieve resource quantity from xml file
    private int getResourceValues(string resourceName) {
        XDocument doc = XDocument.Load(Application.persistentDataPath + LOCAL_PATH);
        
        //XDocument doc = Resources.Load<XDocument>("Resources.xml");
        List <XElement> allResources = doc.Root.Descendants().ToList();
        
        var result = allResources.Elements("Resource").
            First(x => x.Element("Name").Value.Equals(resourceName));

        string amountText = result.Element("Amount").Value;
        int parsedResourceAmount = Int32.Parse(amountText);

        //print("GET Resource: " + resourceName + " Amount: " + parsedResourceAmount);
        return parsedResourceAmount;
    }

    private int[] getResourceValues()
    {

        XDocument doc = XDocument.Load(Application.persistentDataPath + LOCAL_PATH);
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

    private void setResourceValues(int updatedMoneyVal, int updatedScienceVal, int updatedGlobalCoopVal, int updatedEducationVal)
    {
        print("Set Resources to: " + updatedMoneyVal + " " + updatedScienceVal + " " + updatedGlobalCoopVal + " " + updatedEducationVal);
        Serializer.updateXml(updatedMoneyVal, updatedScienceVal, updatedGlobalCoopVal, updatedEducationVal);
        updateGui();
    }
}
