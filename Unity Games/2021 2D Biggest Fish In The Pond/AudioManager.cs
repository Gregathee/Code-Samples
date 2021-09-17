using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

//Serialized wrapper of AudioSource. These are declared in the AudioManager inspector and initialized on Start.
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixer; 

    [Range(0f, 1f)] public float volume = 1;
    [Range(0.5f, 1.5f)] public float pitch = 1;

    [Tooltip("On play the sound will have a random volume variance  plus or minus this value. Does not affect music")]
    [Range(0, 0.5f)] public float volumeVarience = 0;
    [Tooltip("On play the sound will have a random volume variance  plus or minus this value. Does not affect music")]
    [Range(0, 0.5f)] public float pitchVarience = 0;

    AudioSource source;

    public void Initialize(AudioSource sourceIn)
    {
        source = sourceIn;
        source.clip = clip;
        source.outputAudioMixerGroup = mixer;
    }

    /// <summary>
    /// Sets the volume of the audio source but still maintains the original volume value. To restore the volume, Call ResetToOriginalVolume()
    /// </summary>
    /// <param name="newVolume"></param>
    public void SetVolume(float newVolume) { source.volume = newVolume; }

    /// <summary>
    /// Use to adjust scale the volume by a specified amount such as, music volume scale, sfx scale etc.
    /// </summary>
    /// <param name="scale"></param>
    public void ScaleVolume(float scale) { source.volume = volume*scale; }

    public void ResetToOriginalVolume() { source.volume = volume; }

    /// <summary>
    /// Plays the Sound's clip. If the Sound has sound volume or pitch variance, the sound will play with a random pitch or volume variance in a set range
    /// </summary>
    public void PlayOnce()
    {
        source.volume = source.volume * (1 + Random.Range(-volumeVarience / 2, volumeVarience / 2));
        source.pitch = pitch * (1 + Random.Range(-pitchVarience / 2, pitchVarience / 2)); ;
        source.Play();
    }

    /// <summary>
    /// Call this for music or ambient sound tracks
    /// </summary>
    public void PlayLoop()
    {
        source.loop = true;
        source.Play();
    }

    public void Pause() { source.Pause(); }
    public void UnPause() { source.UnPause(); }
    public void Stop() { source.Stop(); }
}

    

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] Sound[] musicTracks = null;
    [SerializeField] Sound[] sfxTracks = null;
    [SerializeField] Sound[] ambientTracks = null;
    [SerializeField] List<Sound> currentlyPlayingAmbience = new List<Sound>();
    [Range(0f, 1f)] public float masterVolume = 1;
    [Range(0f, 1f)] public float sfxVolume = 1;
    [Range(0f, 1f)] public float musicVolume = 1;
    [Range(0f, 1f)] public float ambienceVolume = 1;
    [Tooltip("Time it takes for current track to fade out")]
    [SerializeField] float fadeOutTime = 1;
    [Tooltip("Time window of overlap of current track fade out and next track fade in")]
    [SerializeField] float trackOverlapTime = 1;
    [Tooltip("Time it takes for next track to fade in")]
    [SerializeField] float fadeInTime = 1;

    Sound currentlyPlayingMusic = null;

    public bool isMuted = false;

	private void Awake()
	{
        //Singleton pattern
		if (!instance) { instance = this; }
		else { Destroy(gameObject); return; }
        DontDestroyOnLoad(this);

        SanitizeInput();
    }

	private void Start()
	{
        InitializeTracks(musicTracks);
        InitializeTracks(sfxTracks);
        InitializeTracks(ambientTracks);
        
        PlayMusicWithTransition("General Theme");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            masterVolume += 1f * Time.deltaTime;
            if (masterVolume < 0)
                masterVolume = 0;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            masterVolume -= 1f * Time.deltaTime;
            if (masterVolume > 1)
                masterVolume = 1;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            isMuted = !isMuted;
        }
    }

    private void FixedUpdate()
	{
        //Ensures the volume can be adjusted by player dynamically. 
        if (currentlyPlayingMusic != null)
        {
            if (isMuted) 
            {
                currentlyPlayingMusic.ScaleVolume(0); 
                foreach(Sound sound in currentlyPlayingAmbience) { sound.ScaleVolume(0); }
            }
            else 
            {
                currentlyPlayingMusic.ScaleVolume(musicVolume * masterVolume); 
                foreach(Sound sound in currentlyPlayingAmbience) { sound.ScaleVolume(ambienceVolume * masterVolume); }
            }
        }
    }

    /// <summary>
    /// //Initializes GameObjects, give them names, attach an AudioSource, assign the AudioSource to a Sound object.
    /// </summary>
    /// <param name="tracks"></param>
    void InitializeTracks(Sound[] tracks)
	{
        for (int i = 0; i < tracks.Length; i++)
        {
            GameObject obj = new GameObject("Sound_" + i + "_" + tracks[i].name);
            obj.transform.SetParent(transform);
            tracks[i].Initialize(obj.AddComponent<AudioSource>());
        }
    }

    /// <summary>
    /// Plays a given music track and cross fade transitions if music is currently playing
    /// </summary>
    /// <param name="soundName"></param>
	public void PlayMusicWithTransition(string soundName)
	{
        if(currentlyPlayingMusic != null) if(currentlyPlayingMusic.name == soundName) { return; }
        //Search tracks for sound name
        for(int i = 0; i < musicTracks.Length; i++)
		{
            if(musicTracks[i].name == soundName)
			{
                if (currentlyPlayingMusic != null)
                {
                    //If music is playing, cross fade track transition
                    StartCoroutine(Fade(currentlyPlayingMusic, fadeOutTime, false));
                    StartCoroutine(OverLap(i));
                }
                else
                {
                    //If no music, just play it.
                    currentlyPlayingMusic = musicTracks[i];
                    musicTracks[i].ScaleVolume(musicVolume * masterVolume);
                    if (isMuted) musicTracks[i].ScaleVolume(0);
                    musicTracks[i].PlayLoop();
                }
                return;
			}
		}
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
	}


    public void PlayMusicWithoutTransition(string soundName)
	{
        if (currentlyPlayingMusic.name == soundName) { return; }
        //Search tracks for sound name
        for (int i = 0; i < musicTracks.Length; i++)
        {
            if (musicTracks[i].name == soundName)
            {
                if (currentlyPlayingMusic != null) { currentlyPlayingMusic.Stop(); }
                currentlyPlayingMusic = musicTracks[i];
                musicTracks[i].ScaleVolume(musicVolume * masterVolume);
                if (isMuted) musicTracks[i].ScaleVolume(0);
                musicTracks[i].PlayLoop();
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
    }

    /// <summary>
    /// Play ambient sound along side music if applicable
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayAmbience(string soundName)
    {
        foreach (Sound sound in currentlyPlayingAmbience)
        { if (sound.name == soundName) { return; } }
        //Search tracks for sound name
        for (int i = 0; i < ambientTracks.Length; i++)
        {
            if (ambientTracks[i].name == soundName && currentlyPlayingAmbience.Contains(ambientTracks[i]))
            {
                ambientTracks[i].ScaleVolume(ambienceVolume * masterVolume);
                if (isMuted) ambientTracks[i].ScaleVolume(0);
                ambientTracks[i].PlayLoop();
                currentlyPlayingAmbience.Add(ambientTracks[i]);
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
    }

    /// <summary>
    /// Stops a specific ambientTrack
    /// </summary>
    /// <param name="soundName"></param>
    public void StopAmbientTrack(string soundName)
	{
        Sound soundToBeRemoved = null;
        foreach(Sound sound in currentlyPlayingAmbience)
		{
            if(sound.name == soundName) 
            {
                sound.Stop();
                soundToBeRemoved = sound;
            }
		}
        if (soundToBeRemoved != null) { currentlyPlayingAmbience.Remove(soundToBeRemoved); }
	}

    public void StopAllAmbience()
	{
        foreach(Sound sound in currentlyPlayingAmbience) { sound.Stop();  }
        currentlyPlayingAmbience.Clear();
	}

    public void PlaySFX(string soundName)
    {
        //Search tracks for sound name
        for (int i = 0; i < sfxTracks.Length; i++)
        {
            if (sfxTracks[i].name == soundName)
            {
                sfxTracks[i].ScaleVolume(sfxVolume * masterVolume);
                if (isMuted) sfxTracks[i].ScaleVolume(0);
                sfxTracks[i].PlayOnce();
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
    }

    public void PauseMusic() { currentlyPlayingMusic.Pause(); }

    public void ResumeMusic() { currentlyPlayingMusic.UnPause(); }

    public void StopMusic() { currentlyPlayingMusic.Stop(); }

    /// <summary>
    /// Fades a given sound in or out
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="fadeTime"></param>
    /// <param name="fadeIn">False to fade out</param>
    /// <returns></returns>
    IEnumerator Fade(Sound sound, float fadeTime, bool fadeIn)
    {
        //Sanitize Input
        fadeTime = Mathf.Abs(fadeTime);
        //Skip routine if fadeTime is 0
        if (!isMuted) //Jake's attempted solution at getting rid of the "music un-mutes on fade" bug
        {
            if (fadeTime > 0)
            {
                int fadeInOrOut;
                float fadeStart;
                float startVolume = sound.volume;

                //Adjust values to fade in or fade out
                if (fadeIn) { fadeInOrOut = -1; sound.SetVolume(0.1f); fadeStart = 0; }
                else { fadeStart = 1; fadeInOrOut = 1; }

                float timeElapsed = 0;

                while (timeElapsed < fadeTime)
                {
                    //Set volume to the percentage of time that has elapsed over fade time
                    sound.SetVolume((startVolume * (fadeStart - (fadeInOrOut * timeElapsed / fadeTime))) * masterVolume);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                //Stop sound and reset volume to original amount
                if (!fadeIn) sound.Stop();
                sound.volume = startVolume;
            }
        }
    }

    IEnumerator OverLap(int nextSoundIndex )
	{
        //waits until there are trackOverlapTime seconds left in current tracks fadeout
        yield return new WaitForSeconds(fadeOutTime - trackOverlapTime);

        currentlyPlayingMusic = musicTracks[nextSoundIndex];

        //Set up and start next track
        musicTracks[nextSoundIndex].ScaleVolume(musicVolume);
        if (isMuted) musicTracks[nextSoundIndex].ScaleVolume(0);
        musicTracks[nextSoundIndex].PlayLoop();

        StartCoroutine( Fade(currentlyPlayingMusic, fadeInTime, true));
    }

    void SanitizeInput()
	{
        fadeOutTime = Mathf.Abs(fadeOutTime);
        fadeInTime = Mathf.Abs(fadeInTime);
        trackOverlapTime = Mathf.Abs(trackOverlapTime);
        if (trackOverlapTime > fadeOutTime) { trackOverlapTime = fadeOutTime; }
    }
}