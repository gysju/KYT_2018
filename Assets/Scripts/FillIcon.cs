using UnityEngine;
using UnityEngine.UI;

public class FillIcon : MonoBehaviour {

    [SerializeField] protected Image _progressionUI, _progressionBackgroundUI;

	public virtual void Fill (float value) {
        if (value > 0)
        {
            _progressionUI.fillAmount = value;
            if (!gameObject.activeSelf)
                SetActive(true);
        }
        else SetActive(false);
    }

    public void SetProgressionColor(Color color)
    {
        if (_progressionUI != null)
            _progressionUI.color = color;
        if (_progressionBackgroundUI != null)
        {
            color.a = _progressionBackgroundUI.color.a;
            _progressionBackgroundUI.color = color;
        }
    }
    public void SetActive(bool active)
    {
        if (_progressionBackgroundUI != null)
            gameObject.SetActive(active);
    }
}
