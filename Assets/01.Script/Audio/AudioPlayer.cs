using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _startTime;
    private AudioClipDataSO _sound;
    private AudioClip _audioClip;

    public EAudioName audioName;

    public void Init(AudioClipDataSO sound)
    {
        _audioSource = GetComponent<AudioSource>();
        audioName = sound.audioName;

        _sound = sound;
        AudioManager.Instance.OnVolumeChanged += HandleVolumeChanged;
        HandleVolumeChanged(AudioManager.Instance.volumeSaveData);

        float _3dValue = _sound.is3D ? 1.0f : 0.0f;
        _audioSource.spatialBlend = _3dValue;
        _audioClip = _sound.clip;

        _audioSource.loop = _sound.isLoop;
        if (_audioSource.loop == false)
            _startTime = Time.time;

        _audioSource.clip = _sound.clip;
        _audioSource.Play();

        if (_sound.isDonDestroy)
            DontDestroyOnLoad(gameObject);
    }

    private void HandleVolumeChanged(VolumeSaveData data)
    {
        float volume = data[EAudioType.Master] * _sound.volume;

        if (_sound.audioType != EAudioType.Master)
            volume *= data[_sound.audioType];

        _audioSource.volume = volume;
    }

    private void Update()
    {
        if (_audioSource.loop) return;

        StartCoroutine(DurationCoroutine(_startTime + _sound.duration));
    }

    private IEnumerator DurationCoroutine(float dieTime)
    {
        yield return new WaitUntil(() => dieTime < Time.time);
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        AudioManager.Instance.OnVolumeChanged -= HandleVolumeChanged;
    }
}
