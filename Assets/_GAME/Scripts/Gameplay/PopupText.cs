using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : PooledObject
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text text;
    [SerializeField] float timeTween = 1f;
    [SerializeField] Ease ease = Ease.OutQuad;

    public void Init(Vector3 pos, int value, PopupTextConfig config)
    {
        transform.position = pos;
        text.color = config.textColor;
        text.text = (config.type == GameConfig.PopupTextType.DAMAGE ? "-" : "+") + value;

        PlayTween(pos);
    }

    void PlayTween(Vector3 startPos)
    {
        transform.DOKill();
        canvasGroup.DOKill();

        float x = Random.Range(0.8f, 0.2f);
        float y = Random.Range(0.5f, 1.5f);

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, 0.2f);

        transform.DOMove(startPos + new Vector3(x, 2, y), timeTween)
            .SetEase(ease)
            .OnComplete(() =>
            {
                canvasGroup.DOFade(0f, 0.2f)
                    .OnComplete(RequestDespawn); // 🔥 KHÔNG BIẾT POOL
            });
    }

    public override void OnSpawn()
    {
        canvasGroup.alpha = 1;
    }

    public override void OnDespawn()
    {
        transform.DOKill();
        canvasGroup.DOKill();
    }

}
