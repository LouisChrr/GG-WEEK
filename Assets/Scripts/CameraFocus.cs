using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{

    public Transform FocusCamera;
    

    void Update()
    {
        transform.LookAt(FocusCamera);
    }
}
