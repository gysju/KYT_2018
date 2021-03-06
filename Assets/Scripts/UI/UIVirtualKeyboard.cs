﻿using UnityEngine;
using System;
using UnityEngine.UI;

public class UIVirtualKeyboard : MonoBehaviour
{
    private bool _uppercase = false;
    private bool _qwerty = true;

    [SerializeField] private ButtonArray[] _keys_button = null;
    [SerializeField] private Button _shift = null, _backSpace = null, _return = null;

    private static readonly char[][] _keys_azerty = new char[4][];

    private static readonly char[][] _keys_qwerty = new char[4][];

    [SerializeField] private InputField inputField = null;
    private AudioSource _source = null;

    private Button _crtSelected = null;

    public void Init()
    {
        _keys_azerty[0] = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        _keys_azerty[1] = new char[] { 'a', 'z', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' };
        _keys_azerty[2] = new char[] { 'q', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm' };
        _keys_azerty[3] = new char[] { 'w', 'x', 'c', 'v', 'b', 'n' };//, '^' };//, '<', 'O' };

        _keys_qwerty[0] = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        _keys_qwerty[1] = new char[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' };
        _keys_qwerty[2] = new char[] { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', };// '^' };
        _keys_qwerty[3] = new char[] { 'z', 'x', 'c', 'v', 'b', 'n', 'm' };//, '<', 'O' };

        char[][] keys = _qwerty ? _keys_qwerty : _keys_azerty;

        //InitText(keys);
        InitFunction(keys);
        //InitNavigation(keys);
    }

    private void InitText(char[][] keys)
    {
        //set text
        for (int i = 0; i < keys.Length; i++)
        {
            for (int j = 0; j < keys[i].Length; j++)
            {
                if (i < _keys_button[i].buttons.Length)
                {
                    string s = _uppercase ? ("" + keys[i][j]).ToUpper() : "" + keys[i][j];
                    _keys_button[i].buttons[j].GetComponentInChildren<Text>().text = s;
                }
            }
        }
    }
    private void InitFunction(char[][] keys)
    {
        //set function
        for (int i = 0; i < keys.Length; i++)
        {
            for (int j = 0; j < keys[i].Length; j++)
            {
                string c = String.Copy(keys[i][j].ToString());
                if (i < _keys_button[i].buttons.Length)
                    _keys_button[i].buttons[j].onClick.AddListener(() => KeyBoardOutput(c[0]));
            }
        }
    }
    private void InitNavigation(char[][] keys)
    {
        //set navigation
        for (int i = 0; i < keys.Length; i++)
        {
            for (int j = 0; j < keys[i].Length; j++)
            {
                Navigation nav = /*new Navigation();//*/_keys_button[i].buttons[j].navigation;
                nav.mode = Navigation.Mode.Explicit;
                if (j > 0)
                    nav.selectOnLeft = _keys_button[i].buttons[j - 1];
                if (j + 1 < _keys_button[i].buttons.Length)
                    nav.selectOnRight = _keys_button[i].buttons[j + 1];

                if (i > 0)
                    nav.selectOnUp = _keys_button[i - 1].buttons[j];
                if (i + 1 < _keys_button.Length && j < _keys_button[i + 1].buttons.Length)
                    nav.selectOnDown = _keys_button[i + 1].buttons[j];

                _keys_button[i].buttons[j].navigation = nav;
            }
        }
    }

    public void KeyBoardOutput(char key)
    {
        if (inputField.characterLimit > inputField.text.Length)
            inputField.text += (_uppercase ? (""+key).ToUpper() : ""+key);
    }
    public void DeleteButton()
    {
        if (inputField.text.Length > 0)
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }
    public void UpperCaseButton()
    {
        UpperCaseButton(!_uppercase);
    }
    public void UpperCaseButton(bool upperCase)
    {
        _uppercase = upperCase;
        char[][] keys = _qwerty ? _keys_qwerty : _keys_azerty;
        InitText(keys);
    }

    private void Start()
    {
        Init();
    }

    private int _capsLockState = -1;
    private bool capsLockState
    {
        get { return _capsLockState == 1; }
        set
        {
            if (_capsLockState == -1)
            {
                _capsLockState = value ? 1 : 0;
                if (value)
                    UpperCaseButton();
            }
            else if (_capsLockState == 1 != value)
            {
                _capsLockState = value ? 1 : 0;
                InvokeSelectable(_shift);
            }
        }
    }

    private void InvokeSelectable(Button selectable)
    {
        if (_crtSelected == selectable)
            PlaySound();
        else
        {
            _crtSelected = selectable;
            _crtSelected.Select();
        }
        _crtSelected.onClick.Invoke();
    }

    [System.Runtime.InteropServices.DllImport("USER32.dll")]
    public static extern short GetKeyState(int nVirtKey);

    private void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            DeleteButton();
            PlaySound();
        }

        capsLockState = (GetKeyState(0x14) & 1) > 0;

        if ((Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
         || (Input.GetKeyDown(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift))
         || (Input.GetKeyUp(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
         || (Input.GetKeyUp(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift)))
        {
            InvokeSelectable(_shift);
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            InvokeSelectable(_backSpace);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            InvokeSelectable(_return);
        }

        for (int i = 0; i < _keys_qwerty.Length; i++)
        {
            for (int j = 0; j < _keys_qwerty[i].Length; j++)
            {
                if (Input.GetKeyDown(_keys_qwerty[i][j].ToString()))
                {
                    if (i < _keys_button[i].buttons.Length)
                    {
                        InvokeSelectable(_keys_button[i].buttons[j]);
                        //_keys_button[i].buttons[j].onClick.Invoke();
                    }
                }
            }
        }
    }

    public void PlaySound()
    {
        if (_source == null)
            _source = GetComponentInParent<AudioSource>();

        _source.pitch = UnityEngine.Random.Range(0.7f, 1);
        _source.loop = false;
        _source.Play();
    }

    [System.Serializable]
    public struct ButtonArray
    {
        public Button[] buttons;
    }
}