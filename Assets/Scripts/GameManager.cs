using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject mainMenuUI;

    bool gameHasEnded = false;
    public float restartDelay = 2f;

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
        InitGame();
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

    void InitGame()
    {
        Debug.Log("GAME INIT!");
        FindObjectOfType<AudioManager>().Play("MainMenuTheme");
    }

    public void PlayGame()
    {
        mainMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        FindObjectOfType<AudioManager>().Stop("MainMenuTheme");
        FindObjectOfType<AudioManager>().Play("Theme_1");
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        Debug.Log("MAIN MENU!");
        Resume();
        FindObjectOfType<AudioManager>().Stop("Theme_1");
        SceneManager.LoadScene(0);
        FindObjectOfType<AudioManager>().Play("MainMenuTheme");
        mainMenuUI.SetActive(true);
    }
}
