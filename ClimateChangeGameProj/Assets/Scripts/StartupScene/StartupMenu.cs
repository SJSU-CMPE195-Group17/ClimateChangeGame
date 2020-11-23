using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupMenu : MonoBehaviour
{
    private const int MAIN_GAME_SCREEN = 1;
    private const int TUTORIAL_SCREEN = 3;

    public void NewGame()
    {
        SceneManager.LoadScene(TUTORIAL_SCREEN);
    }

    public void LoadGame ()
    {
        SceneManager.LoadScene(MAIN_GAME_SCREEN);
    }
}
