using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManagement : MonoBehaviour
{
    public AudioClip jumpSoundClip;
    public AudioClip moveSoundClip;

    private AudioSource jumpSoundSource;
    private AudioSource moveSoundSource;

    // Start is called before the first frame update
    void Start()
    {
        jumpSoundSource = GetComponents<AudioSource>()[0];
        moveSoundSource = GetComponents<AudioSource>()[1];        


        SetAudioSourceProperties();
    }

    // Update is called once per frame
    void Update()
    {
       
        moveSoundSource.volume = (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))) / 2;
        moveSoundSource.volume *= 0.05f;

    }

    void SetAudioSourceProperties()
    {
        jumpSoundSource.clip = jumpSoundClip;
        jumpSoundSource.volume = 0.25f;
        jumpSoundSource.loop = false;

        moveSoundSource.clip = moveSoundClip;
        moveSoundSource.volume = 0.1f;
        moveSoundSource.loop = true;
        moveSoundSource.Play();
    }

    public void JumpSoundPlay()
    { jumpSoundSource.Play(); }

    public void MoveSoundPlay()
    { moveSoundSource.Play(); }

    public void JumpSoundStop()
    { jumpSoundSource.Stop(); }

    public void MoveSoundMute()
    { moveSoundSource.volume = 0f; ; }

}
