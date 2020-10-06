using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    private Vector3 _lastValidPosition = Vector3.zero;

    public Vector3 GetLastValidPosition { get { return _lastValidPosition; } }
    public Vector3 SetLastValidPosition { set { _lastValidPosition = value; } }

    private void Start()
    {
        if(_lastValidPosition == Vector3.zero)
        {
            _lastValidPosition = this.transform.position;
        }
    }
}
