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
    public GameObject puzzleView;
    public Button resumeButton;
    public Button exitButton;

    void Start() {
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(MAINGAME_SCREEN);
    }

    public void onPauseTogglePress()
    {
        if(pauseToggle.isOn)
        {
            pauseView.SetActive(true);
            puzzleView.SetActive(false);
        }
        else
        {
            pauseView.SetActive(false);
            puzzleView.SetActive(true);
        }
    }

    public void ResumeGame() {
        pauseToggle.isOn = false;
    }

}
