using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManagement : MonoBehaviour
{
    public AudioClip[] eurobeatClips;

    public AudioClip[] normalClips;

    AudioSource ambientSoundSource;

    float timerSong;
    float songDuration;
    // Start is called before the first frame update
    void Start()
    {       
        ambientSoundSource = GetComponent<AudioSource>();
        ambientSoundSource.loop = false;
        playAmbientSong();
    }

    void ChooseEurobeatSong()
    {
        ambientSoundSource.volume = 0.5f;
        float probability = Random.value;
        if (probability < 0.33f)
        { ambientSoundSource.clip = eurobeatClips[0]; }
        else if (probability < 0.67f)
        { ambientSoundSource.clip = eurobeatClips[1]; }
        else 
        { ambientSoundSource.clip = eurobeatClips[2]; }

    }

    void ChooseNormalSong()
    {
        ambientSoundSource.volume = 0.75f;
        float probability = Random.value;
        if (probability < 0.25f)
        { ambientSoundSource.clip = normalClips[0]; }
        else if (probability < 0.5f)
        { ambientSoundSource.clip = normalClips[1]; }
        else if (probability < 0.75f)
        { ambientSoundSource.clip = normalClips[2]; }
        else
        { ambientSoundSource.clip = normalClips[3]; }
    }

    void playAmbientSong()
    {
        timerSong = 0f;

        /*if (eurobeatClips.Length > 0)
        {
            float probability = Random.value;
            if (probability < 0.3f) { ChooseEurobeatSong(); }
            else { ChooseNormalSong(); }
            songDuration = ambientSoundSource.clip.length;
        }
        else { ChooseNormalSong(); }
        */
        ChooseNormalSong();
        songDuration = ambientSoundSource.clip.length;
        ambientSoundSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timerSong += Time.deltaTime;
        if (timerSong >= songDuration - 0.5f)
        {    playAmbientSong();        }
        if (PauseMenuScript.gameIsPaused)
        { ambientSoundSource.volume = 0.3f; }
        else { ambientSoundSource.volume = 0.5f; }
    }
}
