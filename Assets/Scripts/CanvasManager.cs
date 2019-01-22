using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine.Events;
using System.Text;
using System.Text.RegularExpressions;

public class CanvasManager : MonoBehaviour {

    //old plaquelet color : E5CB00

    public static CanvasManager inst;
    private GameData _data;

    //HUD
    private TextMeshProUGUI _timerText = null;
    private TextMeshProUGUI _scoreText = null;

    private float _time = 0.0f;
    private int _score = 0;

    [Header("MainMenu")]
    [SerializeField] UIAnimation _mainMenu = null;
    [SerializeField] Button _startButton = null;

    [Header("PauseMenu")]
    [SerializeField] UIAnimation _pauseMenu = null;
    [SerializeField] Button _pauseButton = null;

    [Header("GameOverMenu")]
    [SerializeField] UIAnimation _gameOverMenu = null;
    [SerializeField] Button _gameOverButton = null;

    [SerializeField] Compatibility _compatibility = null;

    [SerializeField] private Image _fade_background = null;
    public Image _fade_forground = null;

    private Button _crtReturnButton = null;

    [SerializeField] private ButtonPosition buttonPositionGameOver = null;

    private bool _isTransitionningGameOver = false;
    [SerializeField] private UnityEvent _gameOverReplay = null, _gameOverNextLevel = null, _gameOverQuit = null;

    private int[] _bestScores = { 0, 0, 0, 0, 0 };

    [SerializeField] private TextMeshProUGUI _yourScore_Score = null;
    [SerializeField] private TextMeshProUGUI _yourName_Score = null;

    [SerializeField] private TextMeshProUGUI[] _bestScore_Scores = null;
    [SerializeField] private TextMeshProUGUI[] _bestScore_Names = null;

    [HideInInspector] public bool keyboardDisplay = false;

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

        _fade_forground.gameObject.SetActive(true);
        _mainMenu.gameObject.SetActive(true);
        _fade_background.gameObject.SetActive(true);
        _mainMenu.OpenMainMenu(true);
    }

    public void RequestData()
    {
        _data = GameManager.inst.RequestData();
    }

    private void Update()
    {
        if (GameManager.inst.State == GameManager.GameState.InGame)
        {
            _time = Mathf.Max(0.0f, _time - TimeManager.deltaTime);
            _timerText.text = _time.ToString("0.0");

            if (_time <= .0f)
            {
                int index = SceneManager.GetActiveScene().buildIndex;
                if (_bestScores[index] < _score)
                    _bestScores[index] = _score;

                _yourScore_Score.text = "" + _score;
                //if (_score > 0)
                {
                    _yourName_Score.gameObject.SetActive(true);
                    string username = HandleTextFile.ReadString(GameManager.username_path);
                    username = Regex.Replace(username, @"[^A-Za-z0-9]+", "");
                    _yourName_Score.text = string.IsNullOrEmpty(username) ? "anonymous" : username;
                    //_keyBoard.SetActive(true);
                    //HSController.inst.PostScores("anonymous", _score);
                }
                //else _yourName_Score.gameObject.SetActive(false);

                DisplayGameOverMenu();
            }
            else if ((Input.GetButtonDown("Back0") && !Input.GetButton("Back1")) || (Input.GetButtonDown("Back1") && !Input.GetButton("Back0")))
            {
                _compatibility.SetVisibleInGame();
            }
            else if ((Input.GetButtonUp("Back0") && !Input.GetButton("Back1")) || (Input.GetButtonUp("Back1") && !Input.GetButton("Back0")))
            {
                _compatibility.Hide();
            }
        }
        if ((Input.GetButtonUp("Start") || Input.GetKeyDown(KeyCode.Escape)) && GameManager.inst.State != GameManager.GameState.Menu)
        {
            if (GameManager.inst.State == GameManager.GameState.InGame)
                DisplayPauseMenu();
            else if (GameManager.inst.State == GameManager.GameState.Paused)
                HidePauseMenu();
        }
        else if (Input.GetButtonUp("B") && GameManager.inst.State != GameManager.GameState.InGame && !keyboardDisplay)
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
        _scoreText.text = _score.ToString();
    }

    public void DisplayPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _fade_background.gameObject.SetActive(true);
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
        _fade_background.gameObject.SetActive(false);
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
        _fade_background.gameObject.SetActive(true);
        _compatibility.gameObject.SetActive(false);

        _startButton.Select();
        GameManager.inst.State = GameManager.GameState.Menu;

        TimeManager.timeScale = 0.0f;
    }

    public void DisplayGameOverMenu()
    {
        _isTransitionningGameOver = true;

        _gameOverMenu.SetActive(true);
        _fade_background.gameObject.SetActive(true);
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
        _fade_background.gameObject.SetActive(false);
        //_keyBoard.SetActive(false);
    }

    public void StartGame(bool resetHUD = true)
    {
        _mainMenu.SetActive(false);
        _fade_background.gameObject.SetActive(false);

        GameManager.inst.State = GameManager.GameState.InGame;

        TimeManager.timeScale = 1.0f;

        if (resetHUD)
            ResetHUD();

        AskHighscore();
    }

    public void ResetHUD()
    {
        _time = _data.GameDuration;
        _timerText.text = _time.ToString("0.0");
        _score = 0;
        _scoreText.text = "0";
    }

    public void ReplayGame()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fade_forground.DOFade(1, .3f));
        sequence.AppendCallback( () => {
            GameManager.inst.ClearAllInstance();
            ResetHUD();
            _pauseMenu.SetActive(false);
            HideGameOverMenu();
            _compatibility.gameObject.SetActive(false);
        });
        sequence.Append(_fade_forground.DOFade(0, .3f));
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
    public void PostHighScore(InputField field)
    {
        if (_score > 0)
            HSController.inst.PostScores(field.text, _score);
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
    [SerializeField] private AudioMixer _audioMixer = null;
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
        sequence.Append(_fade_forground.DOFade(1, .3f));
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
        GetHUDText();

        _compatibility.SetCompatibility(_data.BloodTypes[0]);

        if (!_transitioning) return;

        SoundManagerPlayMusic("GameMusic");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fade_forground.DOFade(0, .3f));
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

    private void GetHUDText()
    {
        TextMeshProUGUI[] texts = GameManager.inst.GetHUDText();
        _timerText = texts[0];
        _timerText.text = _data.GameDuration.ToString("0.0");
        _scoreText = texts[1];
        _scoreText.text = "0";
    }

    public void SetGameOverScoreName(string value)
    {
        _yourName_Score.text = value;
    }
}
