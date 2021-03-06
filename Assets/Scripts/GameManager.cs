﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject mainMenuUI;

    bool gameHasEnded = false;
    public float restartDelay = 2f;

    public GameObject hudPlayer;

    public float transitionTime = 1.5f;
    public Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //MainMenu(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If we can pause on current scene
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }
    
    public void PlayGame()
    {
        StartCoroutine(DoTransitionMainMenuToGame());
    }

    IEnumerator DoTransitionMainMenuToGame()
    {
        transitionAnim.SetTrigger("fadeIn");
        AudioManager.instance.Stop("MainMenuTheme");
        mainMenuUI.SetActive(false);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.instance.Play("Theme_1");
        hudPlayer.SendMessage("ShowHUD");
        transitionAnim.SetTrigger("fadeOut");
    }
    
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    void EndGame()
    {
        if (!gameHasEnded)
        {
            Debug.Log("GAME OVER !");
            gameHasEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        Debug.Log("RESTART!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        AudioManager.instance.Play("UI_UnPause");
        AudioManager.instance.Play("Theme_1");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        AudioManager.instance.Pause("Theme_1");
        AudioManager.instance.Play("UI_Pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu(bool firstTime)
    {
        Debug.Log("MAIN MENU!");
        if (!firstTime)
        {
            StartCoroutine(DoTransitionToMainMenu());
        } else
        {
            AudioManager.instance.StopGlobal();
            hudPlayer.SendMessage("HideHUD");
            SceneManager.LoadScene(0);
            AudioManager.instance.Play("MainMenuTheme");
            mainMenuUI.SetActive(true);
        }
    }

    IEnumerator DoTransitionToMainMenu()
    {
        transitionAnim.SetTrigger("fadeIn");
        Resume();
        AudioManager.instance.StopGlobal();
        yield return new WaitForSeconds(transitionTime);
        hudPlayer.SendMessage("HideHUD");
        SceneManager.LoadScene(0);
        AudioManager.instance.Play("MainMenuTheme");
        mainMenuUI.SetActive(true);
        transitionAnim.SetTrigger("fadeOut");
    }
}
