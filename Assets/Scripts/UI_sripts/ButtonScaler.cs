using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float scaleFactor = 0.9f;
    [SerializeField] private float animationDuration = 0.1f;

    private Vector3 originalScale;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
            transform.DOScale(originalScale * scaleFactor, animationDuration);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(originalScale, animationDuration);
    }
}