using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(PaintedObject))]
public class PlaySoundWhenPainted : MonoBehaviour
{
    PaintedObject _paintedObject;
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _clipsToBePlayed;
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
            _audioSource.PlayOneShot(_clipsToBePlayed[Random.Range(0, _clipsToBePlayed.Length)]);
            _hasPlayedAudio = true;
        }
    }
}
