using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image btnImage;
    private RectTransform btnRectTransform;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hoverSprite;

    [Header("Floating Effect")]
    [SerializeField] float downDist;
    [SerializeField] float upDuration;
    [SerializeField] float downDuration;
    [SerializeField] Ease upEase;
    [SerializeField] Ease downEase;
    private Vector3 originPos;

    [Header("ShinkEffect")]
    [SerializeField] float shrinkScale;
    [SerializeField] float shrinkDuration;
    [SerializeField] Ease shrinkEase;



    private void Awake()
    {
        btnImage = GetComponent<Image>();
        btnRectTransform = GetComponent<RectTransform>();

        originPos = btnRectTransform.anchoredPosition;
    }

    private void Start()
    {
        FloatingEffect();
    }



    // 버튼위로 마우스를 올렸을때 버튼에 변화를 주는 효과 구현
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btnImage != null)
            btnImage.sprite = hoverSprite;

        SoundManager.Instance.PlaySFX("Select");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btnImage != null)
            btnImage.sprite = normalSprite;
    }

    public void FloatingEffect()
    {
        // 아래, 위로 움직이는 움직임 시퀀스로 반복 재생
        Sequence floatingSeq = DOTween.Sequence();

        floatingSeq.Append(btnRectTransform.DOAnchorPosY(originPos.y - downDist, downDuration).SetEase(downEase));
        floatingSeq.Append(btnRectTransform.DOAnchorPosY(originPos.y, upDuration).SetEase(upEase));

        // -1 : 무한반복
        floatingSeq.SetLoops(-1);
    }

    public void ClickEffect()
    {
        // OnComplete : 효과 완료 후 람다함수 실행
        btnRectTransform.DOScale(shrinkScale, shrinkDuration).SetEase(shrinkEase).OnComplete(() =>
        {
            btnRectTransform.DOScale(1f, shrinkDuration).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                SceneManager.LoadScene("Game");
            });
        });

    }
}
