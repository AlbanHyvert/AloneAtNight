using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private D_FpHeadBobbing _data = null;

    private float _defaultPosY = 0;
    private float _time;

    private void Start()
    {
        _defaultPosY = transform.localPosition.y;
    }

    public void OnWalk()
    {
        _time += Time.deltaTime * _data.WalkBobbingSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, _defaultPosY + Mathf.Sin(_time) * _data.WalkBobbingAmount,
            transform.localPosition.z);
    }

    public void OnReset()
    {
        _time = 0;
    }

    public void OnIdle()
    {
        _time += Time.deltaTime * _data.IdleBobbingSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, _defaultPosY + Mathf.Sin(_time) * _data.IdleBobbingAmount,
            transform.localPosition.z);
    }

    public void OnCrouch()
    {
        _time += Time.deltaTime * _data.CrouchBobbingSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, _defaultPosY + Mathf.Sin(_time) * _data.CrouchBobbingAmount,
            transform.localPosition.z);
    }
}
