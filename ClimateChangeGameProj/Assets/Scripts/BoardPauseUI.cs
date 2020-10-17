using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardPauseUI : MonoBehaviour
{
    private const int MAINGAME_SCREEN = 1;
    private const int PUZZLE_SCREEN = 2;

    public Toggle pauseToggle;
    public GameObject pauseView;
    public GameObject pauseButton;
    public GameObject resumeButton;

    public void ExitToMainMenu()
    {
        //SceneManager.LoadScene(MAINGAME_SCREEN);
    }

    public void onPauseTogglePress()
    {
        if(pauseToggle.isOn)
        {
            pauseView.SetActive(true);
            pauseButton.SetActive(false);
            resumeButton.SetActive(true);
        }
        else
        {
            pauseView.SetActive(false);
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
        }
    }

}
