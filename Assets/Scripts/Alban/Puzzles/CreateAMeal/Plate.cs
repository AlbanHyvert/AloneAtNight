using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour, IInteractive
{
    [SerializeField] private E_AlimentIndex[] _mealIndex = null;
    [SerializeField] private Transform[] _foodSpawnPosition = null;
    [SerializeField] private Camera _dishCamera = null;
    [Space]
    [SerializeField] private Animator _wardrobeDoor = null;

    private int _index = 0;
    private int _wrongIndex = 0;
    private int _indexSpawnPos = 0;
    private List<Food> _foodList = new List<Food>();

    private void Start()
    {
        _dishCamera.gameObject.SetActive(false);
        _index = 0;
        _wrongIndex = 0;
    }

    public void Enter(Transform parent = null)
    {
        _index = 0;
        _wrongIndex = 0;
        _indexSpawnPos = 0;

        FP_Controller player = PlayerManager.Instance.GetPlayer;
        Inventory inventory = player.GetInventory;

        _dishCamera.gameObject.SetActive(true);

        for (int i = 0; i < inventory.GetPlayerItems().Count; i++)
        {
            GameObject prefab = inventory.GetPlayerItems()[i].GetPrefab();

            if (prefab.TryGetComponent(out Food food))
            {
                GameObject gameObject = CreateObjectInstance(prefab, _foodSpawnPosition[_indexSpawnPos]);

                food = gameObject.GetComponent<Food>();

                _indexSpawnPos++;

                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;

                gameObject.AddComponent<DragObject>().Init(_dishCamera);

                _foodList.Add(food);
            }
        }
    }

    public void Exit()
    {
        for (int i = 0; i < _foodList.Count; i++)
        {
            if(_foodList[i] != null)
            {
                Destroy(_foodList[i].gameObject);
            }
        }

        _dishCamera.gameObject.SetActive(false);

        _foodList.Clear();
    }

    public void OnSeen()
    {
        
    }

    public void OnUnseen()
    {
        
    }

    private GameObject CreateObjectInstance(GameObject prefab, Transform objectAnchor)
    {
        GameObject gameObject = Instantiate(prefab, transform);

        gameObject.transform.position = objectAnchor.position;
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        return gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        int index = _index;
        int wrong = 0;

        if(other.TryGetComponent(out Food food))
        {
            for (int i = 0; i < _mealIndex.Length; i++)
            {
                if(food.GetIndex == _mealIndex[i])
                {
                    _index++;
                }
                else
                {
                    wrong++;
                }

                if(index == _index && wrong == _mealIndex.Length)
                {
                    _wrongIndex++;
                }
            }
        }

        if(_index >= _mealIndex.Length)
        {
            Debug.Log("Dish Done");
            _wardrobeDoor.SetBool("IsActive", true);
        }
        else if(_wrongIndex >= 3)
        {
            PlayerManager.Instance.GetPlayer.SetStopEveryMovement = false;
            Exit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int index = _index;
        int wrong = 0;

        if (other.TryGetComponent(out Food food))
        {
            for (int i = 0; i < _mealIndex.Length; i++)
            {
                if (food.GetIndex == _mealIndex[i])
                {
                    _index--;
                }
                else
                {
                    wrong++;
                }

                if (index == _index && wrong == _mealIndex.Length)
                {
                    _wrongIndex--;
                }
            }
        }
    }
}
