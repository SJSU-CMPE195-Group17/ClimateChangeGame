using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameUI : MonoBehaviour
{

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scienceText;
    public TextMeshProUGUI globalCoopText;
    public TextMeshProUGUI educationText;

    private static string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
    private int moneyVal = 0;
    private int scienceVal = 0;
    private int globalCoopVal = 0;
    private int educationVal = 0;
    private int currYear = 2020;
    private int currSeason = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        updateGui();
    }

    private void updateGui()
    {
        moneyText.text = "" + moneyVal + "M";
        scienceText.text = "" + scienceVal;
        globalCoopText.text = "" + globalCoopVal;
        educationText.text = "" + educationVal;
        dateText.text = seasons[currSeason] + ", " + currYear;
    }

    public void progressTime()
    {
        moneyVal += 60;
        scienceVal += 50;
        globalCoopVal += 40;
        educationVal += 30;
        currSeason++;
        if (currSeason > 3)
        {
            currSeason = 0;
            currYear++;
        }
        updateGui();
    }
}
