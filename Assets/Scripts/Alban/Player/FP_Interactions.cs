using UnityEngine;

public class FP_Interactions : MonoBehaviour
{
    private Inventory _inventory = null;
    private FP_Controller _player = null;

    public void InitInteraction(FP_Controller player, Inventory inventory)
    {
        _inventory = inventory;
        _player = player;
    }

    public void PickUpAndDrop(bool handFull, Pickable pickable)
    {
        if(pickable == null)
        {
            pickable = _player.GetPickable;
        }

        FP_CameraController camera = _player.GetData.cameraController;

        if (handFull == false)
        {
            if(pickable != null)
            {
                pickable.GetComponent<IInteractive>().Enter(camera.GetData.camera.transform);
                _player.SetHandFull = true;
            }
        }
        else
        {
            if(pickable == null)
            {
                pickable = _player.GetPickable;
            }
           pickable.GetComponent<IInteractive>().Exit();
            
            camera.SetIsInteracting = false;
            _player.SetHandFull = false;
        }
    }

    public void Interact(Transform transform, bool handFull)
    {
        IInteractive interactive = null;
        Pickable pickable = null;

        if (transform != null)
        {
            interactive = transform.GetComponent<IInteractive>();
            pickable = transform.GetComponent<Pickable>();
        }
        

        if(interactive != null)
        {
            if(pickable == null)
            {
                interactive.Enter();
            }
            else
            {
                PickUpAndDrop(_player.GetHandFull, pickable);
            }
        }

        if(handFull == true)
        {
            PickUpAndDrop(handFull, _player.GetPickable);
        }
    }

    public void LookAt(Pickable pickable)
    {

    }

    public void AddToInventory(Pickable pickable, ObjectItem objectItem)
    {
        _player.SetHandFull = false;

        _inventory.AddItem(objectItem, 1);

        Destroy(pickable.gameObject);
    }

    public ObjectItem CreateObjectItem()
    {
        ObjectItem objectItem = _inventory.GetPlayerItems()[0].GetObjectItem();

        return objectItem;
    }
}