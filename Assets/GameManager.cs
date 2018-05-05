using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

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

    void InitGame()
    {
        Debug.Log("GAME INIT!");
        FindObjectOfType<AudioManager>().Play("Theme_1");
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
}
