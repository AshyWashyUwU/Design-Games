using UnityEngine;
using Unity.Cinemachine;

public class CameraPositionHandler : MonoBehaviour
{
    private static CameraPositionHandler Instance;
    public static CameraPositionHandler _instance { get => Instance; }

    private bool _cameraType = true;

    private CinemachineCamera _camera;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _labTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _camera = GetComponent<CinemachineCamera>();

        ToggleCameraType();
    }

    public void ToggleCameraType()
    {
        _cameraType = !_cameraType;

        if (_cameraType)
        {
            _camera.Follow = _playerTransform;
            _camera.LookAt = _playerTransform;
        }
        else
        {
            _camera.Follow = _labTransform;
            _camera.LookAt = _labTransform;
        }
    }
}
