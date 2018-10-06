using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Instance;
    [Header("MainMenu")]
    [SerializeField] Transform _mainMenu;
    [SerializeField] Button _startButton;

    [Header("PauseMenu")]
    [SerializeField] Transform _pauseMenu;
    [SerializeField] Button _pauseButton;

    public enum FadeState { Black, Transparent, isFadingToBlack, isFadingToWhite}

    [Space(10)]
    public FadeState Fade = FadeState.Black;

    public Image FadeImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if ( Fade == FadeState.Black)
        {
            FadeImage.color = Color.black;
        }
    }

    public void DisplayPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
        _pauseButton.Select();
        GameManager.Instance.State = GameManager.GameState.Paused;

        Time.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();
    }

    public void HidePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.InGame;

        Time.timeScale = 1.0f;
        SoundManager.Instance.ResumeSound();
    }

    public void DisplayMainMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
        _mainMenu.gameObject.SetActive(true);

        _startButton.Select();
        GameManager.Instance.State = GameManager.GameState.Menu;

        Time.timeScale = 0.0f;
    }

    public void StartGame()
    {
        _mainMenu.gameObject.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.InGame;

        Time.timeScale = 1.0f;
    }

    public void QuiGame()
    {
        Application.Quit();
    }
}
