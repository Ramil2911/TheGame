using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _canBeOpened = true;

    public void Lock()
    {
        _canBeOpened = false;
    }
    
    public void Unlock()
    {
        _canBeOpened = true;
    }

    public bool IsAvailable()
    {
        return _canBeOpened;
    }

    public void Close()
    {
        Animator animator = transform.GetComponent<Animator>();
        bool currentState = animator.GetBool("isOpen");
        if (currentState)
        {
            animator.SetBool("isOpen", false);
        }
    }
    
    public void Open()
    {
        Animator animator = transform.GetComponent<Animator>();
        bool currentState = animator.GetBool("isOpen");
        if (!currentState)
        {
            animator.SetBool("isOpen", true);
        }
    }
    
    public void Activate()
    {
        Animator animator = transform.GetComponent<Animator>();
        bool currentState = animator.GetBool("isOpen");
        animator.SetBool("isOpen", !currentState);
    }
}
