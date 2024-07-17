using Cinemachine;
using Containers;
using Settings;
using Systems;
using UnityEngine;

/// <summary>
/// This is an example of a very simple and straightforward Unity script from a prototype project.
/// I added it to showcase the caching of Unity components and some basic Unity code structure.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerSettings _playerSettings;
    [SerializeField] PlayerInputSystem _inputSystem;
    [SerializeField] PlayerSystem _playerSystem;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] Transform _cameraTarget;

    void Start()
    {
        _noiseComponent = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _characterController = _playerSystem.CharacterController;
    }

    public void Shake()
    {
        _lastShakeTime = Time.time;
    }

    void Update()
    {
        var playerPosition = _characterController.Transform.position;
        var lookDirection = _inputSystem.GameplayInput.Look;
        var targetPosition = playerPosition + (Vector3)lookDirection * _playerSettings.Camera.CamDistance;
        _cameraTarget.position = targetPosition;

        _noiseComponent.m_AmplitudeGain = Mathf.Lerp(0, _playerSettings.Camera.MaxCamAmplitude, 1 - (Time.time - _lastShakeTime) / _playerSettings.Camera.CamShakeDuration);
        _noiseComponent.m_FrequencyGain = Mathf.Lerp(0, _playerSettings.Camera.MaxCamFrequency, 1 - (Time.time - _lastShakeTime) / _playerSettings.Camera.CamShakeDuration);
    }

    CinemachineBasicMultiChannelPerlin _noiseComponent;
    PlayerCharacterController _characterController;
    float _lastShakeTime = float.MinValue;
}