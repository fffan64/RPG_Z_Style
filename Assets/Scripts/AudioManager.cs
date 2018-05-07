using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public AudioMixer audioMixer;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
        }
    }

    private void Start()
    {
        //
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void UnPause(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.UnPause();
    }
    public void Pause(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Pause();
    }

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll(params string[] names)
    {
        foreach (string name in names)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
            }
            s.source.Stop();
        }
    }

    public void StopGlobal()
    {
        System.Array.ForEach(sounds, s => s.source.Stop());
    }

    public void PlayRandom(params string[] names)
    {
        List<Sound> sToPlayRandom = new List<Sound>();
        foreach (string name in names)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
            }
            else
            {
                sToPlayRandom.Add(s);
            }
        }
        if(sToPlayRandom.Count > 0)
        {
            sToPlayRandom[Random.Range(0, sToPlayRandom.Count)].source.Play();
        }
    }
}
