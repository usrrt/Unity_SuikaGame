using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour, IPointerEnterHandler,  IPointerExitHandler
{
    private Image btnImage;
    
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hoverSprite;

    private void Awake()
    {
        btnImage = GetComponent<Image>();
    }

    // 버튼위로 마우스를 올렸을때 버튼에 변화를 주는 효과 구현
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(btnImage != null)
            btnImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(btnImage != null)
            btnImage.sprite = normalSprite;
    }
}
