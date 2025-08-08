using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject gameOverPopup;
    
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private RawImage screenShot;

    [SerializeField] private Button retryBtn;
    [SerializeField] private Button lobbyBtn;

    private Coroutine runningCoroutine;

    private void Awake()
    {
        backGround.SetActive(false);
        gameOverPopup.SetActive(false);

        retryBtn.onClick.AddListener(OnClickRetryButton);
        lobbyBtn.onClick.AddListener(OnClickLobbyButton);
    }

    public void ActivateGameOverUI(int score)
    {
        scoreTxt.text = score.ToString();

        if(runningCoroutine != null)
            StopCoroutine(runningCoroutine);
        
        runningCoroutine = StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D capturedScreen = ScreenCapture.CaptureScreenshotAsTexture();
        
        Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        // 밉맵은 하나의 텍스처를 여러버전으로 저장해서 카메라에서 멀어질때 적절한 텍스처를 선택할 수 있게 만들어줌
        // 2d환경에선 필요없으므로 false로 설정해서 불필요한 연산을 방지
        newScreenShot.SetPixels(capturedScreen.GetPixels());
        newScreenShot.Apply(false);
        
        screenShot.texture = newScreenShot;
        
        backGround.SetActive(true);
        gameOverPopup.SetActive(true);
    }

    private void OnClickRetryButton()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnClickLobbyButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}
