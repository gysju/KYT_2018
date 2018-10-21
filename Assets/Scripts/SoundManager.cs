using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public struct SoundsSettings
    {
        public string Name;
        public AudioClip Clip;
        public float Volume;
        public float Pitch;
        public bool Loop;
    }

    public List<SoundsSettings> Sounds = new List<SoundsSettings>();
    public AudioSource Source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("TitleMusic");
    }

    public void PlayMusic(string name)
    {
        SoundsSettings soundSettings = Sounds.Find(x => x.Name == name);
        if (soundSettings.Clip != null)
        {
            Source.clip = soundSettings.Clip;
            Source.volume = soundSettings.Volume;
            Source.pitch = soundSettings.Pitch;
            Source.loop = soundSettings.Loop;

            Source.Play();
        }
    }

    public void ResumeSound()
    {
        Source.UnPause();
    }

    public void PauseSound()
    {
        Source.Pause();
    }

    public void Restart()
    {
        Source.time = 0;
        Source.Play();
    }

    public void SetVolume(UnityEngine.UI.Slider slider)
    {
        Source.volume = slider.value *.1f;
    }
}
