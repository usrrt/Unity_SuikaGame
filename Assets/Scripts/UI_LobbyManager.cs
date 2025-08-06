using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LobbyManager : MonoBehaviour
{
    [SerializeField] Button starBtn;

    private void Awake()
    {
        starBtn.onClick.AddListener(OnClickStartBtn);
    }

    private void OnClickStartBtn()
    {
        SceneManager.LoadScene("Game");
    }
}
