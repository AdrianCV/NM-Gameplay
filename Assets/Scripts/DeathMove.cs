using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DeathMove : MonoBehaviour
{
    AudioSource _audioSource;

    bool _isFadingIn;
    bool _isFadingOut;

    float _fadeStartTime;
    float _fadeStartVolume;

    [SerializeField] float _fadeDuration;
    [SerializeField] float _maxDistance;

    [SerializeField] Volume _volume;
    Vignette _vignette;
    ColorAdjustments _colorAdjustments;


    public float Speed;

    [SerializeField] Transform _player;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _colorAdjustments);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3((-Speed / 100) * 2, 0, -Speed / 100) * Time.deltaTime;

        _vignette.intensity.value = _audioSource.volume;
        _colorAdjustments.saturation.value = _audioSource.volume * -100;

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
            volume = Mathf.Lerp(_fadeStartVolume, 1f, fadePercentage);
        }

        _audioSource.volume = volume;
    }
}
