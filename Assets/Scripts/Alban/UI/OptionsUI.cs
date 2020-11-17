using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Slider _slider = null;

    private float _rotationSpeed = 5f;

    public void OnEnable()
    {
        _rotationSpeed = PlayerManager.Instance.GetPlayersPrefab.fpsPlayer.GetData.cameraController.GetMovementData.RotationSpeed;

        _slider.value = _rotationSpeed;
    }

    public void ChangeValueRotation(Slider slider)
    {
        _rotationSpeed = slider.value;

        PlayerManager.Instance.GetPlayersPrefab.fpsPlayer.GetData.cameraController.GetMovementData.RotationSpeed = _rotationSpeed;
    }
}