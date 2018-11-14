﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CanvasManager : MonoBehaviour {

    //old plaquelet color : E5CB00

    public static CanvasManager inst;
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

    [SerializeField] Compatibility _compatibility;

    public Image _fade;

    private Button _crtReturnButton;

    [SerializeField] private ButtonPosition buttonPositionGameOver;

    private bool _isTransitionningGameOver = false;
    [SerializeField] private UnityEvent _gameOverReplay, _gameOverNextLevel, _gameOverQuit;

    private int[] _bestScores = { 0, 0, 0, 0, 0 };

    [SerializeField] private TextMeshProUGUI _yourScore_Score;

    [SerializeField] private TextMeshProUGUI[] _bestScore_Scores;
    [SerializeField] private TextMeshProUGUI[] _bestScore_Names;

    [SerializeField] private TMP_InputField tMP_InputField;

    [SerializeField] private GameObject _keyBoard;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //_data = GameManager.Instance.RequestData();

        GameManager.inst.State = GameManager.GameState.Menu;

        AskHighscore();

        _fade.gameObject.SetActive(true);
        _mainMenu.gameObject.SetActive(true);
        _mainMenu.OpenMainMenu(true);
    }

    public void RequestData()
    {
        _data = GameManager.inst.RequestData();
    }

    private void Update()
    {
        _time = Mathf.Max(0.0f, _time - TimeManager.deltaTime);
        TimeText.text = _time.ToString("0.0");

        if (_time <= 0.0F && GameManager.inst.State == GameManager.GameState.InGame)
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (_bestScores[index] < _score)
                _bestScores[index] = _score;

            _yourScore_Score.text = "" + _score;
            if (_score > 0)
            {
                tMP_InputField.gameObject.SetActive(true);
                _keyBoard.SetActive(true);
                //HSController.inst.PostScores("anonymous", _score);
            } else tMP_InputField.gameObject.SetActive(false);

            _hud.gameObject.SetActive(false);

            DisplayGameOverMenu();
        }
        else if ((Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Escape)) && GameManager.inst.State != GameManager.GameState.Menu)
        {
            if (GameManager.inst.State == GameManager.GameState.InGame)
                DisplayPauseMenu();
            else if (GameManager.inst.State == GameManager.GameState.Paused)
                HidePauseMenu();
        }
        else if (Input.GetButtonDown("B"))
        {
            if (GameManager.inst.State == GameManager.GameState.Paused)
                HidePauseMenu();
            else if (_crtReturnButton != null) //is in submenu
            {
                _crtReturnButton.onClick.Invoke();
                _crtReturnButton = null;
            }
        }

        #if UNITY_EDITOR
            if (Input.GetKeyDown("x"))
                _score += 2;
        #endif
    }

    public void AddScore(int value)
    {
        _score += value;
        ScoreText.text = _score.ToString();
    }

    public void DisplayPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _pauseMenu.Open();
        _pauseButton.FindSelectableOnDown().Select();
        _pauseButton.Select();
        GameManager.inst.State = GameManager.GameState.Paused;

        TimeManager.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();

        _compatibility.gameObject.SetActive(true);
        _compatibility.Open();
    }

    public void HidePauseMenu()
    {
        _pauseMenu.SetActive(false);
        GameManager.inst.State = GameManager.GameState.InGame;

        TimeManager.timeScale = 1.0f;
        SoundManager.Instance.ResumeSound();

        _compatibility.gameObject.SetActive(false);
    }

    public void DisplayMainMenu()
    {
        _mainMenu.OpenMainMenu(DisplayMainMenuCallBack);
    }
    private void DisplayMainMenuCallBack()
    {
        _pauseMenu.SetActive(false);
        HideGameOverMenu();
        _mainMenu.SetActive(true);
        _hud.gameObject.SetActive(false);
        _compatibility.gameObject.SetActive(false);

        _startButton.Select();
        GameManager.inst.State = GameManager.GameState.Menu;

        TimeManager.timeScale = 0.0f;
    }

    public void DisplayGameOverMenu()
    {
        _isTransitionningGameOver = true;

        _gameOverMenu.SetActive(true);
        _gameOverMenu.Open();        

        _pauseButton.FindSelectableOnDown().Select();
        _gameOverButton.Select();
        GameManager.inst.State = GameManager.GameState.GameOver;

        TimeManager.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();

        //_compatibility.gameObject.SetActive(true);
        //_compatibility.Open();

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(.5f);
        sequence.AppendCallback(() => {
            _isTransitionningGameOver = false;
        });
        sequence.Play();
    }

    public void HideGameOverMenu()
    {
        _gameOverMenu.SetActive(false);
        tMP_InputField.gameObject.SetActive(true);
        _keyBoard.SetActive(false);
    }

    public void StartGame(bool resetHUD = true)
    {
        _mainMenu.SetActive(false);
        _hud.gameObject.SetActive(true);

        GameManager.inst.State = GameManager.GameState.InGame;

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
            GameManager.inst.ClearAllInstance();
            ResetHUD();
            _pauseMenu.SetActive(false);
            HideGameOverMenu();
            _compatibility.gameObject.SetActive(false);
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
        LoadLevelTransition(index);
    }
    public void LoadLevel(int index)
    {
        if (index < GameManager.numberOfLevel)
            SceneManager.LoadScene(index);
        else DisplayMainMenu();
    }

    private void AskHighscore()
    {
        HSController.inst.GetScores(new System.Action<HSController.Highscore[]>((highscores) => { SetHighscore(highscores); }));
    }
    private void SetHighscore(HSController.Highscore[] highscores)
    {
        /*if (highscores == null)
        {
            for (int i = 0; i < _bestScore_Scores.Length; i++)
            {
                _bestScore_Scores[i].text = "";
                _bestScore_Names[i].text = "";
            }
            _bestScore_Scores[0].text = "...";
            return;
        }*/

        for (int i = 0; i < _bestScore_Scores.Length; i++)
        {
            if (highscores.Length > i)
            {
                _bestScore_Scores[i].text = "" + highscores[i].score;
                _bestScore_Names[i].text = highscores[i].name;
            }
            else
            {
                _bestScore_Scores[i].text = "";
                _bestScore_Names[i].text = "";
            }
        }
    }
    public void PostHighScore()
    {
        HSController.inst.PostScores(tMP_InputField.text, _score);
        _keyBoard.SetActive(false);
        tMP_InputField.interactable = false;
        _gameOverButton.Select();
    }

    //Conservations
    public void GameManagerClearAllInstance()
    {
        GameManager.inst.ClearAllInstance();
    }
    public void SoundManagerPlayMusic(string title)
    {
        SoundManager.Instance.PlayMusic(title);
    }
    public void SoundManagerResumeSoud()
    {
        SoundManager.Instance.ResumeSound();
    }
    public void SoundManagerRestart()
    {
        SoundManager.Instance.Restart();
    }
    public void SoundManagerSetVolume(Slider slider)
    {
        SoundManager.Instance.SetVolume(slider.value * .1f);
    }
    [SerializeField] private AudioMixer _audioMixer;
    public void SFXSetVolume(Slider slider)
    {
        _audioMixer.SetFloat("VomueMasterSFX", slider.value <= 0 ? -80 : slider.value * 5f - 30f);
    }
    private bool _transitioning = false;
    public void LoadLevelTransition(int index)
    {
        if (index == GameManager.numberOfLevel - 1)
            buttonPositionGameOver.HideNextLevel();
        else buttonPositionGameOver.DisplayNextLevel();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fade.DOFade(1, .3f));
        sequence.AppendCallback(() => {
            HideGameOverMenu();
            _mainMenu.SetActive(false);
            _compatibility.gameObject.SetActive(false);
            LoadLevel(index);
        });
        _transitioning = true;
    }
    public void LevelLoaded()
    {
        RequestData();

        _compatibility.SetCompatibility(_data.BloodTypes[0]);

        if (!_transitioning) return;

        SoundManagerPlayMusic("GameMusic");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fade.DOFade(0, .3f));
        sequence.AppendCallback(() => { AskHighscore(); StartGame(); });
    }
    public void SetCrtReturnButton(Button button)
    {
        _crtReturnButton = button;
    }

    public void GameOverReplay()
    {
        if (!_isTransitionningGameOver)
            _gameOverReplay.Invoke();
    }
    public void GameOverNextLevel()
    {
        if (!_isTransitionningGameOver)
            _gameOverNextLevel.Invoke();
    }
    public void GameOverQuit()
    {
        if (!_isTransitionningGameOver)
            _gameOverQuit.Invoke();
    }
}
