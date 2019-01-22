using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float _deltaTime = 0.0f;
    bool _diplay = false;

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.RightShift))
            _diplay = !_diplay;
    }

    void OnGUI()
    {
        if (!_diplay) return;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}