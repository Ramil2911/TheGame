using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnClickButton : MonoBehaviour
{
    public AudioSource audioSource;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            audioSource.Play();
        }
    }
}
