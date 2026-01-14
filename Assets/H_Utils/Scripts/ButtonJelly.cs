using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonJelly : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Button _button;
    [SerializeField] float _toScale = 0.8f;
    [SerializeField] Transform _container;
    [SerializeField] bool _hasEffect = true;

    [Header("Settings")]
    public float timeTween = 0.2f;

    [Header("Delay Click")]
    public float clickDelay = 0.2f;
    bool canClick = true;

    Vector3 initScale;
    Tween tween;
    Tween delayTween;   // <-- tween thay cho coroutine

    void Awake()
    {
        if(_button == null) 
            _button = GetComponent<Button>();
        initScale = _container.localScale;

        _button.onClick.AddListener(OnClick);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canClick || !_hasEffect) return;

        tween?.Kill();
        _container.DOScale(initScale * _toScale, timeTween)
            .SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canClick || !_hasEffect) return;

        tween?.Kill();
        tween = _container.DOScale(initScale, timeTween)
            .SetEase(Ease.OutElastic, 1.2f, 0.5f);

    }

    void OnClick()
    {
        if (!canClick) return;
        
        StartDelay();
    }

    void StartDelay()
    {
        canClick = false;
        if (_button) _button.interactable = false;

        delayTween?.Kill();
        // ❗ Thay coroutine bằng DOTween delay
        delayTween = DOVirtual.DelayedCall(clickDelay, () =>
        {
            canClick = true;
            if (_button) _button.interactable = true;
        }).SetAutoKill(true);
    }
}
