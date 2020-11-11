using UnityEngine;

public class Padlock : MonoBehaviour
{
    [SerializeField] private Animator _nightstand = null;
    [Space]
    [SerializeField] private Transform _snapPosition = null;

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

    private void OnMouseDown()
    {
        FP_Controller controller = PlayerManager.Instance.GetPlayersInstance.fpsPlayer;

        if(controller.GetInventory.GetInventories().Count > 0)
        {
            for (int i = 0; i < controller.GetInventory.GetInventories().Count; i++)
            {
                if(controller.GetInventory.GetInventories()[i].GetPrefab().tag == "Key")
                {
                    GameObject key = CreateObjectInstance(controller.GetInventory.GetInventories()[i].GetPrefab(), _snapPosition);

                    key.transform.SetParent(transform);

                    key.transform.position = _snapPosition.position;
                    key.transform.rotation = _snapPosition.rotation;

                    key.gameObject.layer = 0;

                    Destroy(this.gameObject);
                }
            }
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
}
