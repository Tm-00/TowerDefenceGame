using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource src;
    public AudioClip bgm;

    private void Start()
    {
        StartBGM();
    }

    public void StartBGM()
    {
        src.clip = bgm;
        src.Play(); 
    }
    
    public void StopBGM()
    {
        src.clip = bgm;
        src.Stop(); 
    }
}
