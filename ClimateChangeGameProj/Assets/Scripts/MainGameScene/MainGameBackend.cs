using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainGameBackend : MonoBehaviour
{
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
    private float co2Val = 1050f;       //in ppm

    //estimate maxes for game over, we need to do research on what metrics would be catastrophic
    private const float GLOBAL_TEMP_MAX = 10.68f;   
    private const float OCEAN_TEMP_MAX = 7.5f;  
    private const float SEA_LEVEL_MAX = 564f; 
    private const float ICE_SHEET_MAX = 43200f; 
    private const float CO2_MAX = 6300f;

    public Color MinBarColor = Color.yellow;
    public Color MaxBarColor = Color.red;

    public GameObject mymultiplechoicequestions;

    // Start is called before the first frame update
    void Start()
    {
        updateGui();
        //mymultiplechoicequestions.GetComponent<multiplechoicequestions>().AddQuestions("a text", "b text", "c text", "d text");
    }

    void update(){
        
    }

    private void updateGui()
    {
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
        glblTempVal += 0.356f * numOfSeasons;
        oceanTempVal += 0.25f * numOfSeasons;
        seaLvlVal += 18.8f * numOfSeasons;
        iceSheetVal += 1440f * numOfSeasons;
        co2Val += 210f * numOfSeasons;

        //increase season and year by appropriate amount
        if (currSeason + numOfSeasons > 3)
        {
            currYear += 15;
        }
        currSeason = (currSeason + numOfSeasons) % 4;
    
        if (glblTempVal > 2 && glblTempVal < 3.57)
        {
            mymultiplechoicequestions.SetActive(true);
        }
        if (glblTempVal > 4 && glblTempVal < 5.35)
        {
            mymultiplechoicequestions.SetActive(true);
            mymultiplechoicequestions.GetComponent<multiplechoicequestions>().AddAnswers(4);
        }
    }
}
