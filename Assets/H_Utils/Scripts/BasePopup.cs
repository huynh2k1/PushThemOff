using UnityEngine;
using H_Utils;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Linq;
using DG.Tweening;
using System;
public class BasePopup : BaseUI
{
    public override UIType Type => throw new System.NotImplementedException();

    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected Image _mask;
    [SerializeField] protected GameObject _main;
    [SerializeField] private Button _btnClose;

    [Header("Tween Setup")]
    [SerializeField] bool _moveEffect;
    [SerializeField] protected Ease tweenType = Ease.Linear; 
    [SerializeField] protected float timeTween = 0.3f;


    [Button("Test Show")]
    public void TestShow()
    {
        _canvasGroup.alpha = 1;
        _main.SetActive(true);
    }

    [Button("Load Components")]
    public void LoadAllComponents()
    {
        //Load Mask
        Transform maskTf = transform.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "Mask");
        if (maskTf != null)
            _mask = maskTf.GetComponent<Image>();

        //Load mainGroup
        _canvasGroup = transform.GetComponent<CanvasGroup>();

        //Load main
        Transform mainTf = transform.GetComponentsInChildren<Transform>(true)
                            .FirstOrDefault(t => t.name == "Main");

        if (mainTf != null)
            _main = mainTf.gameObject;
    }

    [Button("Initialize")]
    public void Initialize()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 0;
        _mask.raycastTarget = false;

        _main.SetActive(false);
    }


    protected virtual void Awake()
    {
        Prewarm();

        if (_btnClose != null)
            _btnClose.onClick.AddListener(OnClickClose);
    }

    void Prewarm()
    {
        _main.SetActive(true);
        Canvas.ForceUpdateCanvases();   // �p rebuild UI
        _main.SetActive(false);
    }

    public virtual void OnClickClose()
    {
        Hide();
    }

    public override void Show()
    {
        _canvasGroup.DOKill();
        _canvasGroup.interactable = true;
        _canvasGroup.DOFade(1, timeTween).SetEase(tweenType);
        _mask.raycastTarget = true;

        if(_moveEffect)
            _main.GetComponent<RectTransform>().DOAnchorPosY(0, timeTween).From(new Vector2(0, 500f)).SetEase(tweenType);

        _main.SetActive(true);
    }

    public void Hide(Action actionDone = default)
    {
        _canvasGroup.DOKill();
        //_canvasGroup.interactable = false;
        if(_moveEffect)
            _main.GetComponent<RectTransform>().DOAnchorPosY(500f, timeTween).From(Vector2.zero).SetEase(tweenType);

        _canvasGroup.DOFade(0, timeTween).From(1).SetEase(tweenType).OnComplete(() =>
        {
            _mask.raycastTarget = false;
            _main.SetActive(false);
            actionDone?.Invoke();
        });
    }
}
