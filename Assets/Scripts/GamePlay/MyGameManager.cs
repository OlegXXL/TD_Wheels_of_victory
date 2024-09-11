using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager Instance;

    public int gameHearts = 3;
    public int totalWaves = 5;
    private int currentWave = 0;

    public GameObject resultPanel;
    public Text resultText;
    public Image resultImage;
    public List<GameObject> heartObjects; // List of heart GameObjects
    public Sprite winSprite, loseSprite;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        resultPanel.SetActive(false);
    }

    public void ReduceGameHeart()
    {
        if (gameHearts > 0)
        {
            gameHearts--;
            if (heartObjects.Count > gameHearts)
            {
                heartObjects[gameHearts].SetActive(false); // Deactivate a heart GameObject
            }
            Debug.Log("Game hearts: " + gameHearts);
            if (gameHearts <= 0)
            {
                LoseGame();
            }
        }
    }

    public void NextWave()
    {
        currentWave++;
        Debug.Log("Current Wave: " + currentWave);
        if (currentWave >= totalWaves)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        Debug.Log("You win!");
        resultText.text = $"Level {PlayerPrefs.GetInt("CurrentLevel", 1)} Victory";
        resultImage.sprite = winSprite;
        resultPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        Debug.Log("You lose!");
        resultText.text = $"Level {PlayerPrefs.GetInt("CurrentLevel", 1)} Defeat";
        resultImage.sprite = loseSprite;
        resultPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}