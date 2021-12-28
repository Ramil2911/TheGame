using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSound : MonoBehaviour
{
    public float timeToDelay;
    public AudioSource audioSource;
    public bool canIPlay = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySoundOnTimer());
    }
    void Update() 
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            canIPlay = true;
        }
        else
        {
            canIPlay = false;
        }
    }

    IEnumerator PlaySoundOnTimer()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            yield return new WaitForSeconds(timeToDelay / 2);
        }
        else
        {
            yield return new WaitForSeconds(timeToDelay);
        }
        
        if(canIPlay == true)
        {
            audioSource.Play();
        }
        StartCoroutine(PlaySoundOnTimer());
    }
}
