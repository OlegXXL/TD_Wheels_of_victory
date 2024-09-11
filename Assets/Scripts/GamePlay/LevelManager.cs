using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Text timerText;
    public Text coinText;

    private float gameTime;
    private bool isTimerRunning = true;
    private int currentCoins;

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
        ResetGame();
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        UpdateTimerUI();
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        gameTime = 0f;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60F);
            int seconds = Mathf.FloorToInt(gameTime % 60F);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogWarning("timerText is null. Make sure the UI Text component is not destroyed.");
        }
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    private void UpdateCoinUI()
    {
        coinText.text = currentCoins.ToString();
    }

    public void ResetGame()
    {
        ResetTimer();
        currentCoins = 0;
        UpdateCoinUI();
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }
}