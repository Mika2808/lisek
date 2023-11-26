using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER , GS_OPTIONS}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public TMP_Text scoreText;
    public TMP_Text ScoreText;
    public TMP_Text highScoreText;
    private int score = 0;
    public Image[] keysTab;
    private int keysFound = 0;
    public Canvas pauseMenuCanvas;
    public Canvas optionsCanvas;
    public Canvas levelCompletedCanvas;
    const string keyHighScore = "HighScoreLevel1";    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
                Time.timeScale = 1f;
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
                Time.timeScale = 0;

            }

        }
    }
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
    public void AddKeys(string napis)
    {
        if (napis == "Gem R (UnityEngine.CapsuleCollider2D)")
        {
            keysTab[keysFound].color = Color.red;
        }
        else if (napis == "Gem G (UnityEngine.CapsuleCollider2D)")
        {
            keysTab[keysFound].color = Color.green;
        }
        else if (napis == "Gem B (UnityEngine.CapsuleCollider2D)")
        {
            keysTab[keysFound].color = Color.blue;
        }
        keysFound++;     
    }
    public void Meta( int lives)
    {
        if (keysFound == keysTab.Length)
        {         
            Debug.Log("Zwyciêstwo. Znalaz³eœ wszystkie klucze!");
            AddPoints(100*lives);
            GameManager.instance.LevelCompleted();
                
        }
        else
        {
            Debug.Log("Nie uda³o Ci siê znaleŸæ wszystkich kluczy, przegra³eœ.");
        }
    }
    private void Awake()
    {
        instance = this;
        scoreText.text = score.ToString();
        InGame();
        if (!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
    }
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
        
        if(currentGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        }
        else if (currentGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name == "Level1")
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if(highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }

                highScoreText.text ="Highscore = " + highScore.ToString();
                ScoreText.text ="Your score = " + score.ToString();
            }
        }
    }
    public void OnResumeButtonClicked() {
        InGame();
        Time.timeScale = 1f;
    }
    public void OnOptionsClicked()
    {
        Time.timeScale = 0;

        this.Options();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnIncreaseClicked()
    {
        QualitySettings.IncreaseLevel();
    }
    public void OnDecreaseClicked()
    {
        QualitySettings.DecreaseLevel();
    }
    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }
    public void OnRestartToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }
    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }
    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }
    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }
}
