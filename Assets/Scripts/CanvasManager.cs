using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine.Events;

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

    [SerializeField] Compatibility _compatibility;

    public Image _fade;

    private Button _crtReturnButton;

    [SerializeField] private ButtonPosition buttonPositionGameOver;

    private bool _isTransitionningGameOver = false;
    [SerializeField] private UnityEvent _gameOverReplay, _gameOverNextLevel, _gameOverQuit;

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
        //_data = GameManager.Instance.RequestData();

        GameManager.Instance.State = GameManager.GameState.Menu;

        _fade.gameObject.SetActive(true);
        _mainMenu.gameObject.SetActive(true);
        _mainMenu.OpenMainMenu(true);        
    }

    public void RequestData()
    {
        _data = GameManager.Instance.RequestData();
    }

    private void Update()
    {
        _time = Mathf.Max(0.0f, _time - TimeManager.deltaTime);
        TimeText.text = _time.ToString("0.0");

        if (_time <= 0.0F && GameManager.Instance.State == GameManager.GameState.InGame)
        {
            DisplayGameOverMenu();
        }
        else if (Input.GetButtonDown("Start") && GameManager.Instance.State != GameManager.GameState.Menu)
        {
            if (GameManager.Instance.State == GameManager.GameState.InGame)
            {
                DisplayPauseMenu();
            }
            else if (GameManager.Instance.State == GameManager.GameState.Paused)
            {
                HidePauseMenu();
            }
        }
        else if (Input.GetButtonDown("B"))
        {
            if (GameManager.Instance.State == GameManager.GameState.Paused)
                HidePauseMenu();
            else if (_crtReturnButton != null) //is in submenu
            {
                _crtReturnButton.onClick.Invoke();
                _crtReturnButton = null;
            }
        }
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
        GameManager.Instance.State = GameManager.GameState.Paused;

        TimeManager.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();

        _compatibility.gameObject.SetActive(true);
        _compatibility.Open();
    }

    public void HidePauseMenu()
    {
        _pauseMenu.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.InGame;

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
        _gameOverMenu.SetActive(false);
        _mainMenu.SetActive(true);
        _hud.gameObject.SetActive(false);
        _compatibility.gameObject.SetActive(false);

        _startButton.Select();
        GameManager.Instance.State = GameManager.GameState.Menu;

        TimeManager.timeScale = 0.0f;
    }

    public void DisplayGameOverMenu()
    {
        _isTransitionningGameOver = true;

        _gameOverMenu.SetActive(true);
        _gameOverMenu.Open();

        _pauseButton.FindSelectableOnDown().Select();
        _gameOverButton.Select();
        GameManager.Instance.State = GameManager.GameState.GameOver;

        TimeManager.timeScale = 0.0f;
        SoundManager.Instance.PauseSound();

        _compatibility.gameObject.SetActive(true);
        _compatibility.Open();

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(.5f);
        sequence.AppendCallback(() => {
            _isTransitionningGameOver = false;
        });
        sequence.Play();
    }

    public void StartGame(bool resetHUD = true)
    {
        _mainMenu.SetActive(false);
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
            _pauseMenu.SetActive(false);
            _gameOverMenu.SetActive(false);
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
    }



    //Conservations
    public void GameManagerClearAllInstance()
    {
        GameManager.Instance.ClearAllInstance();
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
            _gameOverMenu.SetActive(false);
            _mainMenu.SetActive(false);
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
        sequence.AppendCallback(() => { StartGame(); });
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
