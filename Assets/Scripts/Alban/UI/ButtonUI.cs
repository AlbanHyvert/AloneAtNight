using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private E_Key _keyValue = E_Key.FORWARD;
    [SerializeField] private TMP_InputField _inputField = null;

    private KeyCode _currentKey = KeyCode.None;

    private void OnEnable()
    {
        switch (_keyValue)
        {
            case E_Key.FORWARD:
                _currentKey = InputManager.Instance.GetKeyboard.forward;
                break;
            case E_Key.LEFT:
                _currentKey = InputManager.Instance.GetKeyboard.left;
                break;
            case E_Key.RIGHT:
                _currentKey = InputManager.Instance.GetKeyboard.right;
                break;
            case E_Key.BACKWARD:
                _currentKey = InputManager.Instance.GetKeyboard.backward;
                break;
            case E_Key.CROUCH:
                _currentKey = InputManager.Instance.GetKeyboard.crouch;
                break;
            case E_Key.INTERACT:
                _currentKey = InputManager.Instance.GetKeyboard.interact;
                break;
            case E_Key.LOOKAT:
                _currentKey = InputManager.Instance.GetKeyboard.lookAt;
                break;
            case E_Key.THROW:
                _currentKey = InputManager.Instance.GetKeyboard.launch;
                break;
        }

        _inputField.text = _currentKey.ToString();
    }

    public void SetKey()
    {
        foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vkey))
            {
                if (vkey != KeyCode.Return)
                {
                    _currentKey = vkey;

                    InputManager.Instance.UpdateKeys(_keyValue, vkey);

                    _inputField.text = _currentKey.ToString();
                }
            }
        }
    }
}
