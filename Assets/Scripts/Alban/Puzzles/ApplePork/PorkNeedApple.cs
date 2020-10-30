using UnityEngine;
using UnityEngine.UI;

public class PorkNeedApple : MonoBehaviour, IInteractive
{
    [SerializeField] private Animator _wardrobeDoor = null;
    [Space]
    [SerializeField] private Transform _applePosition = null;

    public void Enter(Transform parent = null)
    {
        FP_Controller controller = PlayerManager.Instance?.GetPlayer;

        for (int i = 0; i < controller.GetInventory.GetPlayerItems().Count; i++)
        {
            InventoryItem apple = controller.GetInventory.GetPlayerItems()[i];

            if(apple.GetPrefab().tag == "Apple")
            {
                Pickable pickable = Instantiate(apple.GetPrefab().GetComponent<Pickable>(), _applePosition.position, apple.GetLocalRotation());

                GetComponent<Collider>().enabled = false;

                pickable.GetComponent<Collider>().enabled = false;

                pickable.GetComponent<IInteractive>().Enter(_applePosition);
                
                Destroy(pickable.GetComponent<Outline>());
                Destroy(pickable);
                Destroy(this);

                _wardrobeDoor.SetBool("IsActive", true);

                controller.GetInventory.RemoveItem(apple, 1);
            }
            else
            {
                Debug.Log("The pork seems to not be complete.");
            }
        }
    }

    public void Exit()
    {   
    }
    public void OnSeen()
    {
    }
    public void OnUnseen()
    {  
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Pickable pickable))
        {
            if(pickable.transform.tag == "Apple")
            {
                pickable.transform.position = _applePosition.position;

                FP_Controller fP_Controller = PlayerManager.Instance?.GetPlayer;

                fP_Controller.Drop();

                pickable.Enter(_applePosition);

                _wardrobeDoor.SetBool("IsActive", true);

                Destroy(pickable.GetComponent<Outline>());
                Destroy(pickable);

                Destroy(this);
            }
        }
    }
}