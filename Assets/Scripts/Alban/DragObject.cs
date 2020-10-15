using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset = Vector3.zero;
    private float mZCoord = 0f;
    private Camera _camera = null;

    public void Init(Camera camera)
    {
        _camera = camera;
    }

    void OnMouseDown()
    {

        mZCoord = _camera.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen

        mousePoint.z = mZCoord;

        // Convert it to world points

        return _camera.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            transform.Rotate(new Vector3(0, 0, 10));
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            transform.Rotate(new Vector3(0, 0, -10));
        }

        transform.position = transform.position = new Vector3(GetMouseAsWorldPoint().x + mOffset.x, GetMouseAsWorldPoint().y + mOffset.y, transform.position.z);
    }
}
