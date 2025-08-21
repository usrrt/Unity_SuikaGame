using DG.Tweening;
using UnityEngine;

public class PopUpEffectManager : MonoBehaviour
{
    public Vector3 originPos;
    [SerializeField] RectTransform popUpRectTransform;
    [SerializeField] float downDuration;
    [SerializeField] float downDist;
    [SerializeField] Ease downEase;

    [SerializeField] ParticleSystem popUpParticle;

    private void Awake()
    {
        popUpRectTransform = GetComponent<RectTransform>();
        originPos = popUpRectTransform.anchoredPosition;
    }

    private void Start()
    {
        Debug.Log("start");
        PopUpDownEffect();
    }

    private void PopUpDownEffect()
    {
        popUpRectTransform.DOAnchorPosY(originPos.y - downDist, downDuration).SetEase(downEase).SetUpdate(true).OnComplete(() =>
        {
            if (popUpParticle != null)
            {
                popUpParticle.Play();
            };
        });
    }
}
