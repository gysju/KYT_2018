using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Instance;

    [Header("PauseMenu")]
    [SerializeField] Transform _pauseMenu;
    [SerializeField] Button _pauseButton;

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

    public void DisplayPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
        _pauseButton.Select();
    }

    public void HidePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
    }
}
