using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Datas/Dialogue")]
public class D_DialogueText : ScriptableObject
{
    [SerializeField] private DialText _dialogue;

    #region Properties
    public DialText GetDialogues { get { return _dialogue; } }
    public string SetID { set { _dialogue.id = value; } }
    public string SetText { set { _dialogue.text = value; } }
    public float SetLifeTime { set { _dialogue.lifeTime = value; } }
    #endregion Properties

    [System.Serializable]
    public struct DialText
    {
        public string id;
        [TextArea]
        public string text;
        public float lifeTime;
    }
}