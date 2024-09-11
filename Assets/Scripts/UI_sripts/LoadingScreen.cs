using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    public Image backgroundImage; 
    public Image character1; 
    public Image character2; 
    public float zoomDuration = 2.0f; 
    public float fadeDuration = 1.0f;
    public string nextSceneName;

    void Start()
    {
        StartLoadingSequence();
    }

    void StartLoadingSequence()
    {
        backgroundImage.rectTransform.DOScale(1.3f, zoomDuration).OnComplete(() =>
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(character1.DOFade(1.0f, fadeDuration));
            fadeSequence.Join(character2.DOFade(1.0f, fadeDuration));
            fadeSequence.OnComplete(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        });
    }
}