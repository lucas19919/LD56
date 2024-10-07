using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    public GameObject winText;
    public GameObject deathText;
    public GameObject mainMenuText;
    public GameObject healthbar;

    private bool inGame = false;
    private bool isDead = false;

    public Camera activeCam;
    public Camera staticCam;

    private void Update()
    {
        if (!inGame)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startGame();
            }
        }    

        if (isDead)
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Enviroment");
            }
    }

    private void Start()
    {
        activeCam.gameObject.SetActive(false);
    }

     public void DeathScreen()
    {
        healthbar.gameObject.SetActive(false);
        isDead = true;
        deathText.gameObject.SetActive(true);

        activeCam.gameObject.SetActive(false);
        staticCam.gameObject.SetActive(true);
    }

     public void WinScreen()
    {
        healthbar.gameObject.SetActive(false);
        activeCam.gameObject.SetActive(false);
        staticCam.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);
    }

     public void startGame()
    {
        inGame = true;
        mainMenuText.gameObject.SetActive(false);
        activeCam.gameObject.SetActive(true);
        staticCam.gameObject.SetActive(false);
        healthbar.gameObject.SetActive(true);
    }
}
