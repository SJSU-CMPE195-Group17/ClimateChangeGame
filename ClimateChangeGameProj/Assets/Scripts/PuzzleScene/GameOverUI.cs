using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    private const int MAINGAME_SCREEN = 1;
    private const int PUZZLE_SCREEN = 2;

    public GameObject gameOverView;
    public GameObject puzzleView;
    public Button mainMenuButton;
    public TextMeshProUGUI finalScoreText;

    private GameObject bm;
    private BoardManager bmScript;
    float timeLeft;
    int totalScore;

    void Start() {
        mainMenuButton.onClick.AddListener(ExitToMainMenu);
        bm = GameObject.Find("Puzzle");
        bmScript = bm.GetComponent<BoardManager>();
        timeLeft = bmScript.timeRemaining;
    }

    void Update()
    {
        timeLeft = bmScript.timeRemaining;

        if (timeLeft == 0) {
            totalScore = bmScript.totalScore;
            finalScoreText.text = "Final Score: " + totalScore;
            gameOverView.SetActive(true);
            puzzleView.SetActive(false);
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(MAINGAME_SCREEN);
    }
}
