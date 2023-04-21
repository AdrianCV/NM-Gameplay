using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMove : MonoBehaviour
{
    AudioSource _audioSource;

    bool _isFadingIn;
    bool _isFadingOut;

    float _fadeStartTime;
    float _fadeStartVolume;

    [SerializeField] float _fadeDuration;
    [SerializeField] float _maxDistance;

    public float Speed;

    [SerializeField] Transform _player;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Speed / 100, 0, Speed / 100);

        float distance = Vector3.Distance(transform.position, _player.position);
        float volume;

        if (distance > _maxDistance)
        {
            if (!_isFadingOut)
            {
                _isFadingOut = true;
                _isFadingIn = false;
                _fadeStartTime = Time.time;
                _fadeStartVolume = _audioSource.volume;
            }

            float fadeElapsedTime = Time.time - _fadeStartTime;
            float fadePercentage = Mathf.Clamp01(fadeElapsedTime / _fadeDuration);
            volume = Mathf.Lerp(_fadeStartVolume, 0f, fadePercentage);
        }
        else
        {
            if (!_isFadingIn)
            {
                _isFadingIn = true;
                _isFadingOut = false;
                _fadeStartTime = Time.time;
                _fadeStartVolume = _audioSource.volume;
            }

            float fadeElapsedTime = Time.time - _fadeStartTime;
            float fadePercentage = Mathf.Clamp01(fadeElapsedTime / _fadeDuration);
            volume = Mathf.Lerp(0f, 1f, fadePercentage);
        }

        _audioSource.volume = volume;
    }
}
