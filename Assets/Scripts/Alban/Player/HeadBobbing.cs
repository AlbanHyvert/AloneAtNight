using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private Data _data;

    private float _defaultPosY = 0;
    private float _time;

    #region Structs
    [System.Serializable]
    private struct Data
    {
        public int walkBobbingSpeed;
        public float sprintBobbingSpeed;
        public float bobbingAmount;
    }
    #endregion Structs

    private void Start()
    {
        _defaultPosY = transform.localPosition.y;
    }

    public void OnWalk()
    {
        _time += Time.deltaTime * _data.walkBobbingSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, _defaultPosY + Mathf.Sin(_time) * _data.bobbingAmount,
            transform.localPosition.z);
    }

    public void OnIdle()
    {
        _time = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, _defaultPosY,
            Time.deltaTime * _data.walkBobbingSpeed), transform.localPosition.z);
    }
}
