using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static void DeathScreen()
    {
        SceneManager.LoadScene("DeathScreen");
    }

    public static void WinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }

    public static void startGame()
    {
        SceneManager.LoadScene("Enviroment");
    }
}
