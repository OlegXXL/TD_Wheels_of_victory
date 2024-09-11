using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDataList itemDataList;

    void Start()
    {
        foreach (var item in itemDataList.items)
        {
            Debug.Log("Item Index: " + item.index);
            Debug.Log("Is Bought: " + item.isBought);
            Debug.Log("Is Selected: " + item.isSelected);
        }
    }
}