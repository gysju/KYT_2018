using UnityEngine;
using UnityEngine.UI;

public class KeyboardView : MonoBehaviour {

    [SerializeField] private InputField _input = null;
    [SerializeField] private GameObject _backgroundOverlayed = null;
    [SerializeField] private Selectable _selectable = null;
    private Selectable _backStack = null;

    public void Show(Selectable backStack)
    {
        _input.text = HandleTextFile.ReadString(GameManager.username_path);

        CanvasManager.inst.keyboardDisplay = true;

        _backStack = backStack;
        _selectable.Select();
        _backgroundOverlayed.SetActive(false);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        if (_backStack != null)
            _backStack.Select();
        _backgroundOverlayed.SetActive(true);
        gameObject.SetActive(false);

        CanvasManager.inst.keyboardDisplay = false;
        CanvasManager.inst.SetGameOverScoreName(string.IsNullOrEmpty(_input.text) ? "anonymous" : _input.text);

        HandleTextFile.WriteRString(GameManager.username_path, _input.text);
    }
}
