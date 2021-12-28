using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource[] audioSource;
    public AudioSource[] audioSourceInButtle;
    public bool isBattleMod = false;
    public bool isMusicPlayNow = false;
    // Start is called before the first frame update
    void Start()
    {
        
        audioSource[Random.Range(0, audioSource.Length)].Play();
        isMusicPlayNow = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in audioSource)
        {
            if(item.isPlaying == true)
            {
                isMusicPlayNow = true;
                break;
            }
            else 
            {
                isMusicPlayNow = false;
                
            }
        }
        if(isBattleMod == false && isMusicPlayNow == false)
        {
            foreach (var item in audioSource)
            {
                if(item.isPlaying == true)
                {
                    isMusicPlayNow = true;
                    break;
                }
                else
                {
                    isMusicPlayNow = false;
                    
                }
            }
            foreach (var item in audioSourceInButtle)
            {
                if(item.isPlaying == true)
                {
                    item.Stop();
                }
            }
            audioSource[Random.Range(0, audioSource.Length)].Play();
            isMusicPlayNow = true;
        }
        else if(isBattleMod == true)
        {
            foreach (var item in audioSourceInButtle)
            {
                if(item.isPlaying == true)
                {
                    isMusicPlayNow = true;
                    break;
                }
                else
                {
                    isMusicPlayNow = false;
                    
                }
            }
            foreach (var item in audioSource)
            {
                if(item.isPlaying == true)
                {
                    item.Stop();
                }
            }
            if(isMusicPlayNow == false)
            {
                audioSourceInButtle[Random.Range(0, audioSourceInButtle.Length)].Play();
                isMusicPlayNow = true;
            }
        }
    }
}
