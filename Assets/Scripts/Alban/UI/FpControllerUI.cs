using UnityEngine;
using UnityEngine.UI;

public class FpControllerUI : MonoBehaviour
{
    [SerializeField] private Image _pointerImage = null;
    [Space]
    [SerializeField] private Sprite[] _pointers = null;

    private Sprite _currentPointer = null;

    public Sprite SetCurrentPointer
    {
        set
        {
            _currentPointer = value;
            SetPointer(value);
        }
    }

    private void Start()
    {
        SetCurrentPointer = _pointers[0];

        gameObject.SetActive(false);
    }

    public void SetPointerToOpenHand()
    {
        SetCurrentPointer = _pointers[1];
    }

    public void SetPointerToClosedHand()
    {
        SetCurrentPointer = _pointers[2];
    }

    public void SetPointerToEye()
    {
        SetCurrentPointer = _pointers[3];
    }

    public void SetPointerToDefault()
    {
        SetCurrentPointer = _pointers[0];
    }

    private void SetPointer(Sprite sprite)
    {
        _pointerImage.sprite = sprite;
    }
}
