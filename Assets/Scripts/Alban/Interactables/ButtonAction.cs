using UnityEngine;

public class ButtonAction : MonoBehaviour, IInteractive
{
    [SerializeField] private float _maxOutlineWidth = 0.10f;
    [SerializeField] private Color _outlineColor = Color.yellow;

    private MeshRenderer _meshRenderer = null;

    private void Start()
    {
        _meshRenderer = this.GetComponent<MeshRenderer>();

        foreach (Material material in _meshRenderer.materials)
        {
            material.SetFloat("_Outline", 0f);
        }
    }

    void IInteractive.Enter(Transform parent)
    {
        Debug.Log("Hi there");
    }

    void IInteractive.Exit()
    {
        
    }

    void IInteractive.OnSeen()
    {
        if (_meshRenderer != null)
        {
            foreach (Material material in _meshRenderer.materials)
            {
                material.SetFloat("_Outline", _maxOutlineWidth);
                material.SetColor("_OutlineColor", _outlineColor);
            }
        }
    }

    void IInteractive.OnUnseen()
    {
        if (_meshRenderer != null)
        {
            foreach (Material material in _meshRenderer.materials)
            {
                material.SetFloat("_Outline", 0f);
            }
        }
    }
}
