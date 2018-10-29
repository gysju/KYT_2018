using UnityEngine;
using UnityEngine.UI;

public class ButtonPosition : MonoBehaviour {

    public Button up_nextLevel;
    public Button nextLevel;
    public Button down_nextLevel;
    public float increaseY;
    public RectTransform[] uppperButtons;

    private bool _displayed = true;

	public void DisplayNextLevel()
    {
        if (_displayed) return;

        _displayed = true;
        nextLevel.gameObject.SetActive(true);

        for (int i = 0; i < uppperButtons.Length; i++)
            uppperButtons[i].anchoredPosition = new Vector2(uppperButtons[i].anchoredPosition.x, uppperButtons[i].anchoredPosition.y + increaseY);

        SetNavigationUp(down_nextLevel, nextLevel);
        SetNavigationDown(up_nextLevel, nextLevel);
    }
    public void HideNextLevel()
    {
        if (!_displayed) return;

        _displayed = false;
        nextLevel.gameObject.SetActive(false);

        for (int i = 0; i < uppperButtons.Length; i++)
            uppperButtons[i].anchoredPosition = new Vector2(uppperButtons[i].anchoredPosition.x, uppperButtons[i].anchoredPosition.y - increaseY);

        SetNavigationUp(down_nextLevel, up_nextLevel);
        SetNavigationDown(up_nextLevel, down_nextLevel);
    }

    private void SetNavigationUp(Button obj, Button target)
    {
        Navigation nav = obj.navigation;
        nav.selectOnUp = target;
        obj.navigation = nav;
    }
    private void SetNavigationDown(Button obj, Button target)
    {
        Navigation nav = obj.navigation;
        nav.selectOnDown = target;
        obj.navigation = nav;
    }
}
