using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider musicSlider;
    public AudioSource musicSource;

    private void Awake()
    {
        musicSlider.value = 1;
    }

    private void Update()
    {
        musicSource.volume = musicSlider.value;
    }
}
