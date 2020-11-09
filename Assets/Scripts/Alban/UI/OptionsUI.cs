using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    private float _rotationSpeed = 5f;
   
    public void OnEnable()
    {
        _rotationSpeed = PlayerManager.Instance.GetPlayersPrefab.fpsPlayer.GetData.cameraController.GetMovementData.RotationSpeed;
    }

    public void ChangeValueRotation(Slider slider)
    {
        _rotationSpeed = slider.value;

        PlayerManager.Instance.GetPlayersPrefab.fpsPlayer.GetData.cameraController.GetMovementData.RotationSpeed = _rotationSpeed;
    }
}