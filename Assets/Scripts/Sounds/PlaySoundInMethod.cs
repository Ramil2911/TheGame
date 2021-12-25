using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundInMethod : MonoBehaviour
{
    public AudioSource[] audioSource;

    
    public void PlaySound(int id) //id = 0 - стрельба, id = 1 - перезарядка 
    {
        audioSource[id].Play();
    }
}
