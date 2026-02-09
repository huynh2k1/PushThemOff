using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeMenu : MonoBehaviour, IEndDragHandler, IDragHandler
{
    int maxPage;

    [SerializeField] Transform _content;
    List<Transform> levelPages = new List<Transform>();

    [SerializeField] Vector2 _pageStep;
    [SerializeField] RectTransform pagesRect;
    [SerializeField] RectTransform _viewport; // Rect cha (vùng hiển thị)

    [SerializeField] int _currentPage = 0;
    [SerializeField] int _pageWidth = 0;
    private Vector2 _targetPos;

    [Header("Set Up Tween")]
    [SerializeField] float _tweenTime;
    [SerializeField] Ease _tweenType;

    [Header("Swipe Settings")]
    float _swipeThreshold;

    [Header("Scale Settings")]
    [SerializeField] float _maxScale = 1.2f;
    [SerializeField] float _minScale = 1.0f;
    [SerializeField] float _scaleRange = 500f; // khoảng cách để scale từ max → min

    private void Awake()
    {
        _currentPage = 1;
        _targetPos = pagesRect.anchoredPosition;
        _swipeThreshold = Screen.width / 20f;
        _scaleRange = -_pageStep.x;

    }


    [Button("Next")]
    public void Next()
    {
        if (_currentPage < maxPage)
        {
            _currentPage++;
            _targetPos += _pageStep;
            MovePage();
        }
    }

    [Button("Previous")]
    public void Previous()
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            _targetPos -= _pageStep;
            MovePage();
        }
    }

    public void InitPages(List<ShopElement> elements)
    {
        maxPage = elements.Count;
        for (int i = 0; i < elements.Count; ++i)
        {
            Transform e = elements[i].transform;
            levelPages.Add(e);
        }
        UpdatePageScaleInstant();
    }

    public void MovePage()
    {
        pagesRect.DOKill(); // Hủy tween đang chạy (nếu có)
        pagesRect
            .DOAnchorPos(_targetPos, _tweenTime)
            .SetEase(_tweenType)
            .OnUpdate(UpdatePageScaleByDistance)
            .OnComplete(UpdatePageScaleInstant);
    }

    // ================= DRAG =================
    public void OnDrag(PointerEventData eventData)
    {
        // Cho kéo content theo tay
        //levelPagesRect.anchoredPosition += new Vector2(eventData.delta.x, 0);

        // Scale theo khoảng cách realtime
        UpdatePageScaleByDistance();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > _swipeThreshold)
        {
            if (eventData.position.x >= eventData.pressPosition.x)
                Previous();
            else
                Next();
        }
        else
        {
            MovePage();
        }
    }

    // ================= SCALE BY DISTANCE =================
    private void UpdatePageScaleByDistance()
    {
        Vector3 viewportCenter = _viewport.TransformPoint(_viewport.rect.center);

        for (int i = 0; i < levelPages.Count; i++)
        {
            RectTransform pageRect = levelPages[i].GetComponent<RectTransform>();

            Vector3 pageCenter = pageRect.TransformPoint(pageRect.rect.center);
            float distance = Mathf.Abs(pageCenter.x - viewportCenter.x);
            float t = Mathf.Clamp01(distance / (_pageWidth / 2f - 100f));
            float scale = Mathf.Lerp(_maxScale, _minScale, t);

            pageRect.localScale = Vector3.one * scale;
        }
    }

    // ================= SNAP SAU KHI DỪNG =================
    private void UpdatePageScaleInstant()
    {
        for (int i = 0; i < levelPages.Count; i++)
        {
            Transform page = levelPages[i].transform;
            bool isSelected = (i == _currentPage - 1);
            float scale = isSelected ? _maxScale : _minScale;
            page.localScale = Vector3.one * scale;
        }
    }

    public int CurrentPage
    {
        get { return _currentPage; }
    }
}
