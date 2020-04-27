using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("Volume") == false)
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }

        GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume");
        GetComponent<Slider>().onValueChanged.AddListener(OnVolumeChanged);

        ChangeVolumeOffAllAudioSources(GetComponent<Slider>().value);
    }

    private void ChangeVolumeOffAllAudioSources(float value)
    {
        Debug.Log("TEST with value: " + value);
        AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = value;
        }
    }

    void OnVolumeChanged(float sliderValue)
    {
        PlayerPrefs.SetFloat("Volume", sliderValue);
        ChangeVolumeOffAllAudioSources(sliderValue);
    }
}
