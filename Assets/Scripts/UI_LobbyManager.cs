using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LobbyManager : MonoBehaviour
{
    [SerializeField] Button startBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(OnClickStartBtn);
    }

    private void OnClickStartBtn()
    {
        SoundManager.Instance.PlaySFX("Click");

        startBtn.GetComponent<StartBtn>().ClickEffect();
    }
}
