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

        transform.localPosition = new Vector3(transform.localPosition.x, _defaultPosY + Mathf.Sin(_time) * _data.BobbingAmount,
            transform.localPosition.z);
    }

    public void OnIdle()
    {
        _time = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, _defaultPosY,
            Time.deltaTime * _data.WalkBobbingSpeed), transform.localPosition.z);
    }
}
