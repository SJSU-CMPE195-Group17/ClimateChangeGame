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
    public GameObject bgTint;
    public GameObject eventView;
    public GameObject statsButton;
    public GameObject optionsButton;

    public static MainGameUI instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ShowEvent()
    {
        playButton.SetActive(false);
        statsButton.SetActive(false);
        optionsButton.SetActive(false);
        bgTint.SetActive(true);
        eventView.SetActive(true);
    }

    public void EventEnded()
    {
        playButton.SetActive(true);
        statsButton.SetActive(true);
        optionsButton.SetActive(true);
        bgTint.SetActive(false);
        eventView.SetActive(false);
    }

    public void SaveAndQuit()
    {
        SceneManager.LoadScene(STARTUP_SCREEN);
    }

    public void StartPuzzle()
    {
        SceneManager.LoadScene(PUZZLE_SCREEN);   
    }

    public void onStatsTogglePress()
    {
        if(statsToggle.isOn)
        {
            statsView.SetActive(true);
            playButton.SetActive(false);
            bgTint.SetActive(true);
        }
        else
        {
            statsView.SetActive(false);
            playButton.SetActive(true);
            bgTint.SetActive(false);
        }
    }

}
