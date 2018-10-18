using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Instance;
    private GameData _data;
    [Header("HUD")]

    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    private float _time = 0.0f;
    private int _score = 0;

    [Header("MainMenu")]
    [SerializeField] Transform _mainMenu;
    [SerializeField] Button _startButton;

    [Header("PauseMenu")]
    [SerializeField] Transform _pauseMenu;
    [SerializeField] Button _pauseButton;

    [Header("GameOverMenu")]
    [SerializeField] Transform _gameOverMenu;
    [SerializeField] Button _gameOverButton;

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
        _data = GameManager.Instance.RequestData();
        if ( Fade == FadeState.Black)
        {
            FadeImage.color = Color.black;
        }
    }

    private void Update()
    {
        _time = Mathf.Max(0.0f, _time - Time.deltaTime);
        TimeText.text = _time.ToString("0.0");

        if (_time <= 0.0F && GameManager.Instance.State == GameManager.GameState.InGame)
        {
            DisplayGameOverMenu();
        }
    }

    public void AddScore(int value)
    {
        _score += value;
        ScoreText.text = _score.ToString();
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
        _gameOverMenu.gameObject.SetActive(false);

        _mainMenu.gameObject.SetActive(true);

        _startButton.Select();
        GameManager.Instance.State = GameManager.GameState.Menu;

        Time.timeScale = 0.0f;
    }

    public void DisplayGameOverMenu()
    {
        _gameOverMenu.gameObject.SetActive(true);

        _gameOverButton.Select();
        GameManager.Instance.State = GameManager.GameState.Paused;

        Time.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();
    }

    public void StartGame()
    {
        _mainMenu.gameObject.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.InGame;

        Time.timeScale = 1.0f;
        _time = _data.GameDuration;
        _score = 0;
        ScoreText.text = "0";
    }

    public void QuiGame()
    {
        Application.Quit();
    }
}
