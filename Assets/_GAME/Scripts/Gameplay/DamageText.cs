using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TMP_Text _text;
    [SerializeField] float _timeTween;
    [SerializeField] Ease _typeTween;

    [Button("Test")]
    public void Test()
    {
        Init(Vector3.zero, 10);
    }

    public void Init(Vector3 Pos, int value)
    {
        transform.DOKill();
        _canvasGroup.DOKill();

        UpdateText(value);
        Show();

        // ===== RANDOM HƯỚNG BAY =====
        float randomX = Random.Range(0.8f, 1.5f);   // độ lệch ngang
        int dir = Random.value > 0.5f ? 1 : -1;     // trái hoặc phải
        float randomY = Random.Range(2.5f, 3.5f);   // độ cao bay

        Vector3 targetPos = Pos + new Vector3(randomX * dir, randomY, 0);

        // Fade in
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1f, 0.2f);

        // Move chéo + ease
        transform.position = Pos;
        transform.DOMove(targetPos, _timeTween)
                 .SetEase(_typeTween)
                 .OnComplete(() =>
                 {
                     _canvasGroup.DOFade(0f, 0.2f)
                         .OnComplete(Hide);
                 });
    }

    void UpdateText(int value)
    {
        _text.text = "-" + value.ToString();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
