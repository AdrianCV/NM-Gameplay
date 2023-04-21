using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundWhenPainted : MonoBehaviour
{
    PaintedObject _paintedObject;
    AudioSource _audioSource;
    [SerializeField] AudioClip _clipToBePlayed;
    [SerializeField] bool _hasPlayedAudio;

    void Start()
    {
        _paintedObject = GetComponent<PaintedObject>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_paintedObject.IsCorrect && !_hasPlayedAudio)
        {
            _audioSource.PlayOneShot(_clipToBePlayed);
            _hasPlayedAudio = true;
        }
    }
}
