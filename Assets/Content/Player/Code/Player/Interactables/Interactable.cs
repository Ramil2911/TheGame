using System;
using UnityEngine;

/// <summary>
/// Basic monobehaviour for interactable objects
/// </summary>
public class Interactable : MonoBehaviour
{
    [SerializeField] private InteractableObject backingObject;
    public InteractableObject BackingObject => backingObject;
    
    private void Awake()
    {
        this.tag = "Interactable";
    }
}
