using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ButtonEffectManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    /*
     DOTween은 보통 Update() 루프를 따라가기 때문에, Time.timeScale 영향을 받아서 Time.timeScale = 0 이면 멈춰버립니다.
    하지만 UI 애니메이션, 로딩 화면, 페이드 인/아웃 같은 건 TimeScale = 0이어도 돌아가야한다. 
    이럴 때 SetUpdate()를 사용

    SetUpdate(true) → TimeScale 무시하고 항상 실행(UI/페이드 등에 유용)
     */

    public void ClickEffect(Action compeleteAction)
    {
        // OnComplete : 효과 완료 후 람다함수 실행
        btnRectTransform.DOScale(shrinkScale, shrinkDuration).SetEase(shrinkEase).SetUpdate(true).OnComplete(() =>
        {
            btnRectTransform.DOScale(1f, shrinkDuration).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(() =>
            {
                // 재사용성을 높이기 위해 액션 매개변수 활용
                compeleteAction?.Invoke();
                // SceneManager.LoadScene("Game");
            });
        });

    }
}
