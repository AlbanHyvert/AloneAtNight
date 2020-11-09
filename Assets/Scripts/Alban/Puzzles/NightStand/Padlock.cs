using UnityEngine;

public class Padlock : MonoBehaviour
{
    [SerializeField] private Animator _nightstand = null;

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

    private void OnDestroy()
    {
        _nightstand.SetBool("IsActive", true);
    }
}
