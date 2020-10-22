using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Camera _mirrorCamera = null;
    [SerializeField] private RenderTexture _viewTexture = null;
    [SerializeField] private MeshRenderer _screen = null;

    private MeshFilter _screenMeshFilter = null;
    private int _recursionLimit = 2;
    private Camera _playerCamera = null;

    void Start()
    {
        _screenMeshFilter = _screen.GetComponent<MeshFilter>();

        _playerCamera = PlayerManager.Instance.GetPlayer.GetData.cameraController.GetData.camera;
/*
        Vector3 cameraEulerAngles = _mirrorCamera.transform.rotation.eulerAngles;
        Quaternion quaternion = _mirrorCamera.transform.rotation;

        cameraEulerAngles = new Vector3(0, 90, 0);

        quaternion.eulerAngles = cameraEulerAngles;

        _mirrorCamera.transform.rotation = quaternion;
*/
        GameLoopManager.Instance.UpdateCamera += Tick;
    }

    private void Tick()
    {
        Render();
    }

    public void Render()
    {
        // Skip rendering the view from this portal if player is not looking at the linked portal
        if (!CameraUtility.VisibleFromCamera(_screen, _playerCamera))
        {
            _mirrorCamera.enabled = false;
            return;
        }
        else
        {
            _mirrorCamera.enabled = true;
        }

        CreateViewTexture();

        int startIndex = 0;

        _mirrorCamera.projectionMatrix = _playerCamera.projectionMatrix;

        /*Vector3 CameraPos = new Vector3(CameraPosition.x, _playerCamera.transform.position.y, CameraPosition.z);
        Vector3 cameraEulerAngles = _mirrorCamera.transform.rotation.eulerAngles;
        Quaternion quaternion = _mirrorCamera.transform.rotation;

        cameraEulerAngles = new Vector3(0, 90, 0);

        quaternion.eulerAngles = cameraEulerAngles;

        _mirrorCamera.transform.localRotation = quaternion;

        for (int i = 0; i < _recursionLimit; i++)
        {
            _mirrorCamera.transform.position = CameraPos;
            _mirrorCamera.transform.localRotation = quaternion;
        }

        // Hide screen so that camera can see through portal
        _screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        //_mainSettings.linkedPortal.GetMainSettings.screen.material.SetInt("displayMask", 0);

        for (int i = startIndex; i < _recursionLimit; i++)
        {
            _mirrorCamera.transform.position = CameraPos;
            _mirrorCamera.transform.localRotation = quaternion;

            _mirrorCamera.Render();

            if (i == startIndex)
            {
                //_mainSettings.linkedPortal.GetMainSettings.screen.material.SetInt("displayMask", 1);
            }
        }*/

        // Unhide objects hidden at start of render
        _screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    private void CreateViewTexture()
    {
        if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
        {
            if (_viewTexture != null)
            {
                _viewTexture.Release();
            }
            _viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            // Render the view from the portal camera to the view texture
            _mirrorCamera.targetTexture = _viewTexture;

            // Display the view texture on the screen of the linked portal
            _screen.material.SetTexture("_MainTex", _viewTexture);
        }
    }

    private Vector3 CameraPosition
    {
        get
        {
            return _mirrorCamera.transform.position;
        }
    }

    private Vector3 CameraRotation
    {
        get
        {
            return _mirrorCamera.transform.rotation.eulerAngles;
        }
    }

    private void OnDestroy()
    {
        if(GameLoopManager.Instance != null)
            GameLoopManager.Instance.UpdateCamera -= Tick;
    }
}
