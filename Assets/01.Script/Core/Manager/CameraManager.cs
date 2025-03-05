using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<string, CinemachineCamera> _cameraDictionary = new AYellowpaper.SerializedCollections.SerializedDictionary<string, CinemachineCamera>();
    [field: SerializeField] public Volume Volume { get; private set; }


    private Sequence _shakeSequence;
    private Sequence _aberrationSequence;

    private CinemachineVirtualCameraBase _currentCamera;
    public CinemachineVirtualCameraBase currentCamera
    {
        get
        {
            if (_currentCamera == null)
            {
                CinemachineVirtualCameraBase currentCam = CinemachineCore.GetVirtualCamera(0);
                _currentCamera = currentCam;
            }

            return _currentCamera;
        }
        private set
        {
            _currentCamera = value;
            currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private CinemachineBasicMultiChannelPerlin _currentMultiChannel;
    private CinemachineBasicMultiChannelPerlin currentMultiChannel
    {
        get
        {
            if (_currentMultiChannel == null)
                _currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            return _currentMultiChannel;
        }
        set => _currentMultiChannel = value;
    }

    public void MoveToPlayerPositionimmediately()
    {
        currentCamera.ForceCameraPosition(currentCamera.Follow.position, Quaternion.identity);
    }

    public void ChangeCamera(CinemachineCamera camera)
    {
        currentMultiChannel.AmplitudeGain = 0;
        currentMultiChannel.FrequencyGain = 0;

        currentCamera.Priority = 10;
        currentCamera = camera;
        currentCamera.Priority = 11;
        currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ChangeCamera(string cameraName)
    {
        currentMultiChannel.AmplitudeGain = 0;
        currentMultiChannel.FrequencyGain = 0;

        currentCamera.Priority = 10;
        currentCamera = _cameraDictionary[cameraName];
        currentCamera.Priority = 11;
        currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitude, float frequency, float time, AnimationCurve curve)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        _shakeSequence
            .Append(
                DOTween.To(() => amplitude,
                value => currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(curve))
            .Join(
                DOTween.To(() => frequency,
                value => currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(curve));
    }
    public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        _shakeSequence
            .Append(
                DOTween.To(() => amplitude,
                value => currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(ease))
            .Join(
                DOTween.To(() => frequency,
                value => currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(ease));
    }

    public void Aberration(float intensity, float time, AnimationCurve curve)
    {
        if (_aberrationSequence != null && _aberrationSequence.IsActive()) _aberrationSequence.Kill();
        _aberrationSequence = DOTween.Sequence();

        if (Volume.profile.TryGet(out ChromaticAberration chromaticAberration) && chromaticAberration.intensity.value < intensity)
        {
            _aberrationSequence
                .Append(
                    DOTween.To(() => intensity,
                    value => chromaticAberration.intensity.value = value,
                    0, time).SetEase(curve));
        }
    }
    public void Aberration(float intensity, float time, Ease ease = Ease.Linear)
    {
        if (_aberrationSequence != null && _aberrationSequence.IsActive()) _aberrationSequence.Kill();
        _aberrationSequence = DOTween.Sequence();

        if (Volume.profile.TryGet(out ChromaticAberration chromaticAberration) && chromaticAberration.intensity.value < intensity)
        {
            _aberrationSequence
                .Append(
                    DOTween.To(() => intensity,
                    value => chromaticAberration.intensity.value = value,
                    0, time).SetEase(ease));

        }
    }
}
