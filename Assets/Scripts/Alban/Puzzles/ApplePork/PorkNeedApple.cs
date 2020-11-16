using UnityEngine;
using UnityEngine.UI;

public class PorkNeedApple : MonoBehaviour, IInteractive
{
    [SerializeField] private Animator _wardrobeDoor = null;
    [Space]
    [SerializeField] private Transform _applePosition = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _audioID = string.Empty;

    public void Enter(Transform parent = null)
    {
        FP_Controller controller = PlayerManager.Instance.GetPlayersInstance.fpsPlayer;

        for (int i = 0; i < controller.GetInventory.GetInventories().Count; i++)
        {
            InventoryItem apple = controller.GetInventory.GetInventories()[i];

            if(apple.GetPrefab().tag == "Apple")
            {
                Pickable pickable = Instantiate(apple.GetPrefab().GetComponent<Pickable>(), _applePosition.position, apple.GetLocalRotation());

                GetComponent<Collider>().enabled = false;

                pickable.GetComponent<Collider>().enabled = false;

                pickable.GetComponent<IInteractive>().Enter(_applePosition);
                
                Destroy(pickable.GetComponent<Outline>());
                Destroy(pickable);
                Destroy(this);

                if (!string.IsNullOrEmpty(_audioID))
                    _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioID));

                _wardrobeDoor.SetBool("IsActive", true);

                controller.GetInventory.RemoveItem(apple);
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

                FP_Controller fP_Controller = PlayerManager.Instance.GetPlayersInstance.fpsPlayer;

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