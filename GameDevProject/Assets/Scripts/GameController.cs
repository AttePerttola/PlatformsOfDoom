using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider;

    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedLevelsCount;

    public static event Action OnReset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        PlayerHealth.OnPlayerDied += GameOverScreen;
        loadCanvas.SetActive(false);
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PauseBackgroundMusic();
        survivedText.text = "YOU SURVIVED " + survivedLevelsCount + " LEVEL";
        if (survivedLevelsCount != 1) survivedText.text += "S";
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true); // restart  music
        survivedLevelsCount = 0;
        LoadLevel(0, false);
        Gem[] gems = FindObjectsByType<Gem>(FindObjectsInactive.Include);
        foreach (Gem gem in gems)
        {
            gem.gameObject.SetActive(true);
        }
        OnReset?.Invoke();
        Time.timeScale = 1;
    }

    public void OnMenuClick()
    {
        Time.timeScale = 1;
        MusicManager.StopBackgroundMusic();
        SceneManager.LoadScene("StartScene");
    }

    private void OnDestroy()
    {
        //Debug.Log("GameController destroyed");

        Gem.OnGemCollect -= IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete -= LoadNextLevel;
        PlayerHealth.OnPlayerDied -= GameOverScreen;
    } 

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;

        if (progressAmount > 30) 
        {
            // Level complete
            loadCanvas.SetActive(true);
            Debug.Log("Level complete");
        }
    }

    void LoadLevel(int level, bool wantSurvivedIncrease)
    {
        loadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        player.transform.position = Vector3.zero;

        currentLevelIndex = level;
        progressAmount = 0;
        progressSlider.value = 0;
        if(wantSurvivedIncrease) survivedLevelsCount++;
    }

    void LoadNextLevel()
    {
        int nextlevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextlevelIndex, true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
