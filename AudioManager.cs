using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	public AudioMixer Mixer;

	public AudioClip[] ClickSounds;
	public AudioSource BackgroundMusic;
	public AudioSource Click;

	public static AudioManager Instance = null;

    private void Awake()
	{
		if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
	}

    public void Start()
    {
		BackgroundMusic = GetComponent<AudioSource>();
		Click = GetComponent<AudioSource>();
	}

    public void PlayMusic(AudioClip clip)
	{
		BackgroundMusic.clip = clip;
		BackgroundMusic.Play();
	}

	public void ClickSound()
	{
		Instance.Click.clip = ClickSounds[Random.Range(0, Instance.ClickSounds.Length)];
        if (Instance.Click != null)
        {
            Instance.Click.Play();
		}
	}

	public void BackgroundVolume(float sliderValue)
	{
		Mixer.SetFloat("BackgroundVolume", Mathf.Log10(sliderValue) * 20);

		if (sliderValue >= 0f && sliderValue <= 1f)
		{
			PlayerPrefs.SetFloat("BackgroundVolume", sliderValue);
		}
	}

	public void SoundVolume(float sliderValue)
	{
		Mixer.SetFloat("SoundsVolume", Mathf.Log10(sliderValue) * 20);
    }
}
