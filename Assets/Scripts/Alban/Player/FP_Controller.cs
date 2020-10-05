using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FP_Controller : FP_StateMachine
{
    [SerializeField] private Body _body;
    [Space]
    [SerializeField] private d_FPCharacter _data = null;
    [SerializeField] private d_FPInteractive _interaction = null;

    public Body GetBody { get { return _body; } }
    public d_FPCharacter GetData { get { return _data; } }
    public d_FPInteractive GetInteractive { get { return _interaction; } }

    [System.Serializable]
    public struct Body
    {
        public Rigidbody rb;
        public FP_Camera cameraController;
    }

    public void Awake()
    {
        if (_body.rb == null)
            _body.rb = this.GetComponent<Rigidbody>();

        if (_data == null)
        {
            _data = new d_FPCharacter();
        }

        SetState(new FP_Walking(this));
    }
}