using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList", menuName = "ScriptableObjects/ItemDataList", order = 1)]
public class ItemDataList : ScriptableObject
{
    public ItemData[] items;
}