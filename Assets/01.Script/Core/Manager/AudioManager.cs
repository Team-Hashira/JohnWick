using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class VolumeSaveData
{
    private Dictionary<EAudioType, float> volumeDictionary;

    public VolumeSaveData()
    {
        volumeDictionary = new Dictionary<EAudioType, float>()
        {
            {EAudioType.Master, 1f},
            {EAudioType.BGM, 0.5f},
            {EAudioType.SFX, 0.5f},
        };
    }

    public float this[EAudioType eAudioType]
    {
        get => volumeDictionary[eAudioType];
    }

    public bool ChangeVolume(EAudioType eAudioType, float volume)
    {
        bool flag = false;
        if (volumeDictionary[eAudioType] != volume)
        {
            volumeDictionary[eAudioType] = volume;
            flag = true;
        }
        return flag;
    }

    public void OnLoadData(VolumeSaveData classData)
    {
        volumeDictionary = classData.volumeDictionary;
    }

    public void OnSaveData(string savedFileName)
    {

    }
}

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioClipListSO _audios;
    [SerializeField] private AudioPlayer _soundPlayerPrefab;

    public VolumeSaveData volumeSaveData { get; private set; } = new VolumeSaveData();

    public event Action<VolumeSaveData> OnVolumeChanged;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(EAudioType eAudioType, float volume)
    {
        bool flag = volumeSaveData.ChangeVolume(eAudioType, volume);
        
        if (flag)
        {
            OnVolumeChanged?.Invoke(volumeSaveData);
        }
    }

    public AudioPlayer PlaySound(EAudioName soundEnum, Transform parent)
    {
        AudioPlayer soundPlayer = Instantiate(_soundPlayerPrefab, parent);
        soundPlayer.transform.localPosition = Vector3.zero;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }
    public AudioPlayer PlaySound(EAudioName soundEnum, Vector3 pos)
    {
        AudioPlayer soundPlayer = Instantiate(_soundPlayerPrefab);
        soundPlayer.transform.position = pos;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }
    public AudioPlayer PlaySound(EAudioName soundEnum, Transform parent, Vector3 pos)
    {
        AudioPlayer soundPlayer = Instantiate(_soundPlayerPrefab, parent);
        soundPlayer.transform.localPosition = pos;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }

    public void StopSound(EAudioName soundEnum, Transform target = null)
    {
        AudioPlayer[] soundPlayers;
        if (target == null)
            soundPlayers = FindObjectsByType<AudioPlayer>(FindObjectsSortMode.None);
        else
            soundPlayers = target.GetComponentsInChildren<AudioPlayer>();

        AudioClipDataSO sound = _audios.GetAudioClipData(soundEnum);

        for (int i = 0; i < soundPlayers.Length; i++)
        {
            if (soundPlayers[i].audioName == sound.audioName)
                soundPlayers[i].Die();
        }
    }
    public void StopSound(AudioPlayer soundPlayer)
    {
        soundPlayer.Die();
    }
}
