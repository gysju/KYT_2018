using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Instance;
    private GameData _data;
    [Header("HUD")]
    [SerializeField] private RectTransform _hud;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    private float _time = 0.0f;
    private int _score = 0;

    [Header("MainMenu")]
    [SerializeField] UIAnimation _mainMenu;
    [SerializeField] Button _startButton;

    [Header("PauseMenu")]
    [SerializeField] UIAnimation _pauseMenu;
    [SerializeField] Button _pauseButton;

    [Header("GameOverMenu")]
    [SerializeField] UIAnimation _gameOverMenu;
    [SerializeField] Button _gameOverButton;

    public Image _fade;

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

        _fade.gameObject.SetActive(true);
        _mainMenu.gameObject.SetActive(true);
        _mainMenu.OpenMainMenu(true);
    }

    private void Update()
    {
        _time = Mathf.Max(0.0f, _time - TimeManager.deltaTime);
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
        _pauseMenu.Open();
        _pauseButton.FindSelectableOnDown().Select();
        _pauseButton.Select();
        GameManager.Instance.State = GameManager.GameState.Paused;

        TimeManager.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();
    }

    public void HidePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.InGame;

        TimeManager.timeScale = 1.0f;
        SoundManager.Instance.ResumeSound();
    }

    public void DisplayMainMenu()
    {
        _mainMenu.OpenMainMenu(DisplayMainMenuCallBack);
    }
    private void DisplayMainMenuCallBack()
    {
        _pauseMenu.gameObject.SetActive(false);
        _gameOverMenu.gameObject.SetActive(false);
        _mainMenu.gameObject.SetActive(true);
        _hud.gameObject.SetActive(false);

        _startButton.Select();
        GameManager.Instance.State = GameManager.GameState.Menu;

        TimeManager.timeScale = 0.0f;
    }

    public void DisplayGameOverMenu()
    {
        _gameOverMenu.gameObject.SetActive(true);
        _gameOverMenu.Open();

        _pauseButton.FindSelectableOnDown().Select();
        _gameOverButton.Select();
        GameManager.Instance.State = GameManager.GameState.Paused;

        TimeManager.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();
    }

    public void StartGame(bool resetHUD = true)
    {
        _mainMenu.gameObject.SetActive(false);
        _hud.gameObject.SetActive(true);

        GameManager.Instance.State = GameManager.GameState.InGame;

        TimeManager.timeScale = 1.0f;

        if (resetHUD)
            ResetHUD();
    }

    private void ResetHUD()
    {
        _time = _data.GameDuration;
        TimeText.text = _time.ToString("0.0");
        _score = 0;
        ScoreText.text = "0";
    }

    public void ReplayGame()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fade.DOFade(1, .3f));
        sequence.AppendCallback( () => {
            GameManager.Instance.ClearAllInstance();
            ResetHUD();
            _pauseMenu.gameObject.SetActive(false);
            _gameOverMenu.gameObject.SetActive(false);
        });
        sequence.Append(_fade.DOFade(0, .3f));
        sequence.AppendCallback(() => {
            StartGame(false);
        });
        sequence.Play();
    }

    public void QuiGame()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        LoadLevel(index);
    }
    public void LoadLevel(int index)
    {
        if (index < GameManager.numberOfLevel)
            SceneManager.LoadScene(index);
    }
}
