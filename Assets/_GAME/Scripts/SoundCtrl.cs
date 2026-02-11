using H_Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TypeSFX
{
    CLICK,
    BYE,
    COIN,
    WIN,
    LOSE,
    DEAD,
    SWORDHIT,
    HAMMERHIT,
    HITGROUND,
    DOORSTONEOPEN,
    DOORWOODOPEN
}

public class SoundCtrl : MonoBehaviour
{
    public static SoundCtrl I;

    [Header("MUSIC")]
    [SerializeField] AudioSource _musicSource;

    [Header("SOUNDS")]
    [SerializeField] AudioSource[] _soundSources;
    private Queue<AudioSource> _queueSounds;

    [Header("AUDIO CLIPS")]
    [SerializeField] AudioClip _bgMusic;
    [SerializeField]
    AudioClip _click, _bye, _coin, _win, _lose, _dead,
        _swordHit, _hammerHit, _hitGround, _doorStoneOpen, _doorWoodOpen;
    private void Awake()
    {
        I = this;
        _queueSounds = new Queue<AudioSource>(_soundSources);
    }

    public void OnVolumeSoundChange()
    {
        foreach (var sound in _queueSounds)
        {
            sound.mute = !GameDatas.IsSoundOn;
        }
    }

    public void OnVolumeMusicChange()
    {
        _musicSource.mute = !GameDatas.IsMusicOn;
    }

    public void PlayMusic()
    {
        _musicSource.mute = !GameDatas.IsMusicOn;
        _musicSource.clip = _bgMusic;
        _musicSource.Play();
    }

    public void PlaySFXByType(TypeSFX type)
    {
        switch (type)
        {
            case TypeSFX.CLICK:
                PlaySound(_click);
                break;
            case TypeSFX.BYE:
                PlaySound(_bye);
                break;
            case TypeSFX.COIN:
                PlaySound(_coin);
                break;
            case TypeSFX.WIN:
                PlaySound(_win);
                break;
            case TypeSFX.LOSE:
                PlaySound(_lose);
                break;
            case TypeSFX.DEAD:
                PlaySound(_dead);
                break;
            case TypeSFX.SWORDHIT:
                PlaySound(_swordHit);
                break;
            case TypeSFX.HAMMERHIT:
                PlaySound(_hammerHit);
                break;
            case TypeSFX.HITGROUND:
                PlaySound(_hitGround);
                break;
            case TypeSFX.DOORSTONEOPEN:
                PlaySound(_doorStoneOpen);
                break;
            case TypeSFX.DOORWOODOPEN:
                PlaySound(_doorWoodOpen);
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (_queueSounds.Count == 0) return;

        AudioSource source = _queueSounds.Dequeue();
        source.mute = !GameDatas.IsSoundOn;
        source.PlayOneShot(clip);
        StartCoroutine(ReturnToQueueWhenFinished(source));
    }

    private System.Collections.IEnumerator ReturnToQueueWhenFinished(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        _queueSounds.Enqueue(source);
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}

