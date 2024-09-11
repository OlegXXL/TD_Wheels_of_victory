using UnityEngine;
using UnityEngine.UI;

public class CoinControllMenu : MonoBehaviour
{
    public Text coinCounter1;
    public Text coinCounter2;
    public Text coinCounter3;
    public Button addCoinsButton;

    private int coins;

    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateCoinCounters();

        addCoinsButton.onClick.AddListener(AddCoins);
    }

    void AddCoins()
    {
        coins += 250;
        PlayerPrefs.SetInt("Coins", coins);
        UpdateCoinCounters();
    }

    void UpdateCoinCounters()
    {
        coinCounter1.text = coins.ToString();
        coinCounter2.text = coins.ToString();
        coinCounter3.text = coins.ToString();
    }
}