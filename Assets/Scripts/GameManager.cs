using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Security.Cryptography;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public TMP_Text scoreText;
    private int score = 0;
    public Image[] keysTab;
    private int keysFound = 0;

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
                currentGameState = GameState.GS_GAME;
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                currentGameState = GameState.GS_PAUSEMENU;
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
    public void Meta()
    {
        if (keysFound == keysTab.Length)
        {
            Debug.Log("Zwyciêstwo. Znalaz³eœ wszystkie klucze!");
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
    }
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if(currentGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        }
    }
    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
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
