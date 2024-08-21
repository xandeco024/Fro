using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //utiliza o standard inputsys, talvez quebre em breve.
    
    [SerializeField] private Cinemachine.CinemachineFreeLook freeLookCamera;

    void Start()
    {
        if (freeLookCamera == null) freeLookCamera = GetComponent<Cinemachine.CinemachineFreeLook>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
            freeLookCamera.m_XAxis.m_InputAxisValue = 0;
            freeLookCamera.m_YAxis.m_InputAxisValue = 0;
        }
    }
}
