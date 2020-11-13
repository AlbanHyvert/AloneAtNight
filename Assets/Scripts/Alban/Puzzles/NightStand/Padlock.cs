using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Padlock : MonoBehaviour, IInteractive
{
    [SerializeField] private Animator _nightstand = null;
    [Space]
    [SerializeField] private Transform _snapPosition = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _id = string.Empty;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Key")
        {
            PlayerManager.Instance.GetPlayersInstance.fpsPlayer.Drop();

            Rigidbody rigidbody = other.GetComponent<Rigidbody>();

            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

            rigidbody.transform.SetParent(this.transform);

            Destroy(this.gameObject, 1);
        }
    }

    private GameObject CreateObjectInstance(GameObject prefab, Transform objectAnchor)
    {
        GameObject gameObject = Instantiate(prefab, transform);

        gameObject.transform.position = objectAnchor.position;
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        return gameObject;
    }

    private void OnDestroy()
    {
        _nightstand.SetBool("IsActive", true);
    }

    public void Enter(Transform parent = null)
    {
        FP_Controller controller = PlayerManager.Instance.GetPlayersInstance.fpsPlayer;

        if (controller.GetInventory.GetInventories().Count > 0)
        {
            for (int i = 0; i < controller.GetInventory.GetInventories().Count; i++)
            {
                if (controller.GetInventory.GetInventories()[i].GetPrefab().tag == "Key")
                {
                    GameObject key = CreateObjectInstance(controller.GetInventory.GetInventories()[i].GetPrefab(), _snapPosition);

                    key.transform.SetParent(transform);

                    key.transform.position = _snapPosition.position;
                    key.transform.rotation = _snapPosition.rotation;

                    key.gameObject.layer = 0;

                    if (_id != string.Empty)
                        _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_id));

                    Destroy(this.gameObject);
                }
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
}
