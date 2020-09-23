using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    private const int STARTUP_SCREEN = 0;
    private const int PUZZLE_SCREEN = 2;

    public Toggle statsToggle;
    public GameObject statsView;
    public GameObject playButton;
    public GameObject testButton;

    public void SaveAndQuit()
    {
        SceneManager.LoadScene(STARTUP_SCREEN);
    }

    public void StartPuzzle()
    {
        //Belle can uncomment this when she hooks up the puzzle screen
        //SceneManager.LoadScene(PUZZLE_SCREEN);
   
    }

    public void onStatsTogglePress()
    {
        if(statsToggle.isOn)
        {
            statsView.SetActive(true);
            playButton.SetActive(false);
            testButton.SetActive(false);
        }
        else
        {
            statsView.SetActive(false);
            playButton.SetActive(true);
            testButton.SetActive(true);
        }
    }

}
