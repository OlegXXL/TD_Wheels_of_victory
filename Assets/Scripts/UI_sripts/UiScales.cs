using UnityEngine;
using DG.Tweening;

public class UiScales : MonoBehaviour
{
    private Vector3 startScale = Vector3.zero;
    private Vector3 endScale = new Vector3(1f, 1f, 1f);

    public float scaleUpDuration = 0.5f;

    void OnEnable()
    {
        transform.localScale = startScale;

        transform.DOScale(endScale, scaleUpDuration)
            .SetEase(Ease.OutQuad);
    }
}