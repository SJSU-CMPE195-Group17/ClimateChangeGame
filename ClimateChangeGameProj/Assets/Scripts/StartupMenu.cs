using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupMenu : MonoBehaviour
{
    private const int MAIN_GAME_SCREEN = 1;

    public void PlayGame ()
    {
        SceneManager.LoadScene(MAIN_GAME_SCREEN);
    }
}
