using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour, IInteractive
{
    [SerializeField] private E_AlimentIndex[] _mealIndex = null;
    [SerializeField] private Transform[] _foodSpawnPosition = null;
    [SerializeField] private Camera _dishCamera = null;

    private int _index = 0;
    private int _wrongIndex = 0;
    private int _indexSpawnPos = 0;
    private List<Food> _foodList = new List<Food>();

    private void Start()
    {
        _dishCamera.gameObject.SetActive(false);
        _index = 0;

        if(_mealIndex.Length != _foodSpawnPosition.Length)
        {
            Debug.LogError("Missing Reference, for the Dish Puzzle");
        }
    }

    public void Enter(Transform parent = null)
    {
        _index = 0;
        _indexSpawnPos = 0;

        FP_Controller player = PlayerManager.Instance.GetPlayer;
        List<InventoryItem> inventory = player.GetInventory.GetPlayerItems();

        _dishCamera.gameObject.SetActive(true);

        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItem item = inventory[i];

            if (item.GetPrefab().TryGetComponent(out Food food))
            {
                GameObject gameObject = player.CreateObjectInstance(item.GetObjectItem(), _foodSpawnPosition[_indexSpawnPos]);

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

    private void OnTriggerEnter(Collider other)
    {
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
                    _wrongIndex++;
                }
            }
        }

        if(_index >= _mealIndex.Length)
        {
            Debug.Log("Dish Done");
        }
        else if(_wrongIndex >= 3)
        {
            Exit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
                    _wrongIndex--;
                }
            }
        }
    }
}
