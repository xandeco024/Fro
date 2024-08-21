using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //utiliza o standard inputsys, talvez quebre em breve.

    private Vector2 lastAxisValue;
    
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
        else if (freeLookCamera.m_XAxis.Value != lastAxisValue.x || freeLookCamera.m_YAxis.Value != lastAxisValue.y)
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
            lastAxisValue = new Vector2(freeLookCamera.m_XAxis.Value, freeLookCamera.m_YAxis.Value);

        }
        else
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
    }
}
