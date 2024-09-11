using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteSwap : MonoBehaviour
{
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    public void SwapSprite()
    {
        if (GetComponent<Image>().sprite == sprite1)
        {
            GetComponent<Image>().sprite = sprite2;
        }
        else
        {
            GetComponent<Image>().sprite = sprite1;
        }
    }
}
