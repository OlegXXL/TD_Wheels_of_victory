using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class BaseStore : MonoBehaviour
{
    public Text coinText;
    public Button buyButton;
    public Button useButton;
    public Image itemImage;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Sprite useSprite;
    public Sprite useSpriteInteracteble;
    public Sprite usedSprite;
    public Sprite[] itemSprites;
    public int[] itemPrices;
    public ItemDataList itemDataList; 

    private int currentItemIndex = 0;
    private int coins;
    public GameObject notEnougMoney_panel;
    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
        leftArrowButton.onClick.AddListener(OnLeftArrowClick);
        rightArrowButton.onClick.AddListener(OnRightArrowClick);
        buyButton.onClick.AddListener(OnBuyButtonClick);
        useButton.onClick.AddListener(OnUseButtonClick);
    }

    void UpdateUI()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coins.ToString();
        itemImage.sprite = itemSprites[currentItemIndex];
        ItemData currentItem = itemDataList.items[currentItemIndex];
        Debug.Log("Item Index: " + currentItem.index);
        if (currentItem.isBought)
        {
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);

            if (currentItem.isSelected)
            {
                useButton.image.sprite = usedSprite;
                useButton.interactable = false;
            }
            else
            {
                useButton.image.sprite = useSprite;
                useButton.interactable = true;
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            useButton.image.sprite = useSpriteInteracteble;
            useButton.interactable = false;
        }
    }

    void OnLeftArrowClick()
    {
        currentItemIndex = (currentItemIndex - 1 + itemSprites.Length) % itemSprites.Length;
        UpdateUI();
    }

    void OnRightArrowClick()
    {
        currentItemIndex = (currentItemIndex + 1) % itemSprites.Length;
        UpdateUI();
    }

void OnBuyButtonClick()
{
    ItemData currentItem = itemDataList.items[currentItemIndex];
    if (coins >= itemPrices[currentItemIndex])
    {
        coins -= itemPrices[currentItemIndex];
        PlayerPrefs.SetInt("Coins", coins);
        currentItem.isBought = true;

        // Unselect all items
        foreach (var item in itemDataList.items)
        {
            item.isSelected = false;
        }

        // Select the current item
        currentItem.isSelected = true;
        SaveItemData();
        UpdateUI();
    }
    else
    {
        notEnougMoney_panel.SetActive(true);
    }
}

    void OnUseButtonClick()
    {
        ItemData currentItem = itemDataList.items[currentItemIndex];
    if (currentItem.isBought)
    {
        foreach (var item in itemDataList.items)
        {
            item.isSelected = false;
        }

        currentItem.isSelected = true;
        SaveItemData();
        UpdateUI();
    }
    }

    void SaveItemData()
    {
#if UNITY_EDITOR
       
        UnityEditor.EditorUtility.SetDirty(itemDataList);
#endif
    }
}