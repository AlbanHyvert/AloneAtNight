using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Datas/TileData")]
public class D_TileData : ScriptableObject
{
    [SerializeField] private Data _tileData;

    public Data GetData { get { return _tileData; } }

    [System.Serializable]
    public struct Data
    {
        public double idleSpeed;
        public double movingOutSpeed;
        public double rotatingSpeed;
        public double returningSpeed;
        public int triggerMaxDistance;
        public int minGoingOutDistance;
        public int maxGoingOutDistance;
        public int smoothTime;
    }
}
