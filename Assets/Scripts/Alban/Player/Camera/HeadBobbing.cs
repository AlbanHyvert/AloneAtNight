using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private D_FpHeadBobbing _data = null;

    private float _defaultPosY = 0;
    private float _currentPosY = 0;
    private float _time;

    public float CurrentPosY { get { return _currentPosY; } set { _currentPosY= value; } }
    public float DefaultPosY { get { return _defaultPosY; } }

    private void Start()
    {
        _defaultPosY = transform.localPosition.y;

        _currentPosY = _defaultPosY;
    }

    public void OnWalk()
    {
        _time += Time.deltaTime * _data.WalkBobbingSpeed;

        if (_currentPosY != _defaultPosY)
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, _currentPosY, transform.localPosition.z), _time);

        transform.localPosition = new Vector3(transform.localPosition.x, _currentPosY + Mathf.Sin(_time) * _data.WalkBobbingAmount,
            transform.localPosition.z);
    }

    public void OnReset()
    {
        _time = 0;
    }

    public void OnIdle()
    {
        _time += Time.deltaTime * _data.IdleBobbingSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, _currentPosY + Mathf.Sin(_time) * _data.IdleBobbingAmount,
            transform.localPosition.z);
    }

    public void OnCrouch()
    {
        _time += Time.deltaTime * _data.CrouchBobbingSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, _currentPosY + Mathf.Sin(_time) * _data.CrouchBobbingAmount,
            transform.localPosition.z);
    }
}
