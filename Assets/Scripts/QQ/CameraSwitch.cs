using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public GameObject camera0, camera1, camera2, camera3, camera4, camera5, camera6, camera7, camera8;

    AudioListener camera0AudioLis, camera1AudioLis, camera2AudioLis, camera3AudioLis, camera4AudioLis, camera5AudioLis, camera6AudioLis, camera7AudioLis, camera8AudioLis;

    // Use this for initialization
    void Start()
    {

        //Get Camera Listeners
        camera0AudioLis = camera0.GetComponent<AudioListener>();
        camera1AudioLis = camera1.GetComponent<AudioListener>();
        camera2AudioLis = camera2.GetComponent<AudioListener>();
        camera3AudioLis = camera3.GetComponent<AudioListener>();
        camera4AudioLis = camera4.GetComponent<AudioListener>();
        camera5AudioLis = camera5.GetComponent<AudioListener>();
        camera6AudioLis = camera6.GetComponent<AudioListener>();
        camera7AudioLis = camera7.GetComponent<AudioListener>();
        camera8AudioLis = camera8.GetComponent<AudioListener>();

        //Camera Position Set
        cameraPositionChange(PlayerPrefs.GetInt("CameraPosition"));
    }

    // Update is called once per frame
    void Update()
    {
        //Change Camera Keyboard
        switchCamera();
    }

    //UI JoyStick Method
    public void cameraPositonM()
    {
        cameraIncCounter();
    }

    //Change Camera Keyboard
    void switchCamera()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            cameraIncCounter();
        }

        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            cameraDecCounter();
        }
    }

    //Camera Counter
    void cameraIncCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }

    void cameraDecCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter--;
        cameraPositionChange(cameraPositionCounter);
    }

    //Camera change Logic
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 8)
        {
            camPosition = 0;
        }
        else if (camPosition < 0)
        {
            camPosition = 8;
        }


        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        //Set camera position 1
        if (camPosition == 0)
        {
            camera0.SetActive(true);
            camera0AudioLis.enabled = true;

            camera8AudioLis.enabled = false;
            camera8.SetActive(false);

            camera1AudioLis.enabled = false;
            camera1.SetActive(false);
        }

        //Set camera position 2
        if (camPosition == 1)
        {
            camera1.SetActive(true);
            camera1AudioLis.enabled = true;

            camera0AudioLis.enabled = false;
            camera0.SetActive(false);

            camera2AudioLis.enabled = false;
            camera2.SetActive(false);
        }

        if (camPosition == 2)
        {
            camera2.SetActive(true);
            camera2AudioLis.enabled = true;

            camera1AudioLis.enabled = false;
            camera1.SetActive(false);

            camera3AudioLis.enabled = false;
            camera3.SetActive(false);
        }

        if (camPosition == 3)
        {
            camera3.SetActive(true);
            camera3AudioLis.enabled = true;

            camera2AudioLis.enabled = false;
            camera2.SetActive(false);

            camera4AudioLis.enabled = false;
            camera4.SetActive(false);
        }

        if (camPosition == 4)
        {
            camera4.SetActive(true);
            camera4AudioLis.enabled = true;

            camera3AudioLis.enabled = false;
            camera3.SetActive(false);

            camera5AudioLis.enabled = false;
            camera5.SetActive(false);
        }

        if (camPosition == 5)
        {
            camera5.SetActive(true);
            camera5AudioLis.enabled = true;

            camera4AudioLis.enabled = false;
            camera4.SetActive(false);

            camera6AudioLis.enabled = false;
            camera6.SetActive(false);
        }

        if (camPosition == 6)
        {
            camera6.SetActive(true);
            camera6AudioLis.enabled = true;

            camera5AudioLis.enabled = false;
            camera5.SetActive(false);

            camera7AudioLis.enabled = false;
            camera7.SetActive(false);
        }

        if (camPosition == 7)
        {
            camera7.SetActive(true);
            camera7AudioLis.enabled = true;

            camera6AudioLis.enabled = false;
            camera6.SetActive(false);

            camera8AudioLis.enabled = false;
            camera8.SetActive(false);
        }

        if (camPosition == 8)
        {
            camera8.SetActive(true);
            camera8AudioLis.enabled = true;

            camera7AudioLis.enabled = false;
            camera7.SetActive(false);

            camera0AudioLis.enabled = false;
            camera0.SetActive(false);
        }
    }
}