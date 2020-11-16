using UnityEngine;

public class UtensilShadow : MonoBehaviour
{
    [SerializeField] private E_Tableware _tableware = E_Tableware.FORK;
    [Space]
    [SerializeField] private Transform _snapPosition = null;
    [Space]
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private string _audioSpawnID = string.Empty;

    private bool _hasTableware = false;
    private FP_Controller _controller = null;

    public bool HasTableware { get { return _hasTableware; } }

    private void Start()
    {
        _controller = PlayerManager.Instance.GetPlayersInstance.fpsPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Tableware tableware))
        {
            if(tableware.Type == _tableware)
            {
                _controller.Drop();

                tableware.transform.SetParent(transform);

                tableware.transform.position = _snapPosition.position;
                tableware.transform.rotation = _snapPosition.rotation;

                tableware.GetComponent<Rigidbody>().isKinematic = false;

                _hasTableware = true;

                Destroy(tableware);
            }
        }
    }

    public void OnMouseDown()
    {
        if(_controller.GetInventory.GetInventories().Count > 0)
        {
            for (int i = 0; i < _controller.GetInventory.GetInventories().Count; i++)
            {
                if (_controller.GetInventory.GetInventories()[i].GetPrefab().TryGetComponent(out Tableware tableware))
                {
                    if (tableware.Type == _tableware)
                    {
                        if (!string.IsNullOrEmpty(_audioSpawnID))
                            _audioSource.PlayOneShot(SoundManager.Instance.GetAudio(_audioSpawnID));

                        GameObject tablewareInstance = CreateObjectInstance(tableware.gameObject, _snapPosition);

                        tablewareInstance.transform.SetParent(transform);

                        tablewareInstance.transform.position = _snapPosition.position;
                        tablewareInstance.transform.rotation = _snapPosition.rotation;

                        tableware.GetComponent<Rigidbody>().isKinematic = false;

                        _hasTableware = true;

                        _controller.GetInventory.RemoveItem(_controller.GetInventory.GetInventories()[i]);

                        tableware.gameObject.layer = 0;

                        Destroy(this);
                    }
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
}