using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public enum CameraMode { FirstPerson, ThirdPerson };
public class CameraManager : MonoBehaviour
{
    public CameraMode DefaultCameraMode = CameraMode.ThirdPerson;

    [Serializable]
    public struct CamItem
    {
        public CameraMode Mode;
        public CinemachineVirtualCamera Cam;
    }

    public CamItem[] CamItems;
    private Dictionary<CameraMode, CinemachineVirtualCamera> m_Cameras = new Dictionary<CameraMode, CinemachineVirtualCamera>();

    public UnityAction<CameraMode> OnSwitchCam;

    public CameraMode CurrentCameraMode { get; private set; }
    public CinemachineVirtualCamera CurrentCamera
    {
        get
        {
            return m_Cameras[CurrentCameraMode];
        }
    }

    public void SwitchCam()
    {
        switch (CurrentCameraMode)
        {
            case CameraMode.FirstPerson:
                CurrentCameraMode = CameraMode.ThirdPerson;
                break;
            case CameraMode.ThirdPerson:
                CurrentCameraMode = CameraMode.FirstPerson;
                break;
        }

        foreach (var cam in m_Cameras)
        {
            cam.Value.Priority = cam.Key == CurrentCameraMode ? 1 : 0;
        }

        if (OnSwitchCam != null)
            OnSwitchCam.Invoke(CurrentCameraMode);
    }

    void Awake()
    {
        foreach (var cam in CamItems)
        {
            m_Cameras.Add(cam.Mode, cam.Cam);
        }

        CurrentCameraMode = DefaultCameraMode;
        foreach (var cam in m_Cameras)
        {
            cam.Value.Priority = cam.Key == CurrentCameraMode ? 1 : 0;
        }
    }

    void Start()
    {
        // initial switch to default camera mode
        if (OnSwitchCam != null)
            OnSwitchCam.Invoke(CurrentCameraMode);
    }

}
