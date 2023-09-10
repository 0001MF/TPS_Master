using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject AimCamera;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCamera;
    public GameObject ThirdPersonCanvas;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            ThirdPersonCamera.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCanvas.SetActive(true);
            AimCamera.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            ThirdPersonCamera.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCanvas.SetActive(true);
            AimCamera.SetActive(true);
        }
        else
        {
            ThirdPersonCamera.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCanvas.SetActive(false);
            AimCamera.SetActive(false);
        }
    }
}
