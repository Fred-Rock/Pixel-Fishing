using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cine;
    [SerializeField] private CinemachineVirtualCamera _defaultCam;
    [SerializeField] private CinemachineVirtualCamera _searchAreaFollowCam;
    [SerializeField] private CinemachineVirtualCamera _cutsceneCam;
    [SerializeField] private CinemachineVirtualCamera _boatCam;
    [SerializeField] private CinemachineVirtualCamera _catchCam;
    private List<CinemachineVirtualCamera> cams = new List<CinemachineVirtualCamera>();

    public enum CameraState
    {
        Menu,
        Cutscene,
        Search,
        Boat,
        Catch
    }

    private void OnEnable()
    {
        EventHandler.CameraStateEvent += SetLiveCam;
    }

    private void OnDisable()
    {
        EventHandler.CameraStateEvent -= SetLiveCam;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        cams.Add(_defaultCam);
        cams.Add(_searchAreaFollowCam);
        cams.Add(_cutsceneCam);
        cams.Add(_boatCam);
        cams.Add(_catchCam);

        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(false);
        }

        _defaultCam.gameObject.SetActive(true);
    }

    private void SetLiveCam(CameraState gameState)
    {
        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(false);
        }

        switch (gameState)
        {
            case (CameraState.Cutscene):
                _cutsceneCam.gameObject.SetActive(true);
                break;
            case (CameraState.Search):
                _searchAreaFollowCam.gameObject.SetActive(true);
                break;
            case (CameraState.Boat):
                _boatCam.gameObject.SetActive(true);
                break;
            case (CameraState.Catch):
                _catchCam.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
