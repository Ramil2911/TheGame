using System;
using UnityEngine;

/// <summary>
/// Base data class for all interactable objects in game
/// </summary>
public abstract class InteractableObject : ScriptableObject
{
    /// <summary>
    /// Name of interactable object that will be seen by player, idk how to internationalize it
    /// </summary>
    [SerializeField] private string interactableName;
    /// <summary>
    /// Description of object, not necessary, leave empty if no description provided
    /// </summary>
    [SerializeField] private string description;
    /// <summary>
    /// A sprite to be rendered in inventory
    /// </summary>
    [SerializeField] private Sprite sprite;

    /// <summary>
    /// Generator of world's representation of interactable
    /// </summary>
    [SerializeField] private InteractableObjectGenerator generator;
    /// <summary>
    /// Prefab of interactable in user's hands
    /// </summary>
    [SerializeField] public GameObject interactableInHands;
    /// <summary>
    /// Can object be stacked in inventory or not
    /// </summary>
    [SerializeField] private bool isStackable;

    /// <summary>
    /// <inheritdoc cref="sprite"/>
    /// </summary>
    public Sprite Sprite => sprite;
    /// <summary>
    /// <inheritdoc cref="interactableName"/>
    /// </summary>
    public string InteractableName => interactableName;
    /// <summary>
    /// <inheritdoc cref="description"/>
    /// </summary>
    public string Description => description;
    /// <summary>
    /// <inheritdoc cref="generator"/>
    /// </summary>
    public InteractableObjectGenerator Generator => generator;
    /// <summary>
    /// <inheritdoc cref="isStackable"/>
    /// </summary>
    public bool IsStackable => isStackable;
}

/// <summary>
/// Maybe in the future we will add somewhat like interactables spawning via console or etc, so i'll just leave it here
/// </summary>
[Serializable]
public class InteractableObjectGenerator
{
    /// <summary>
    /// Generator of world's representation of interactable
    /// </summary>
    /// <param name="position">Position of representation</param>
    /// <param name="rotation">Rotation of representation</param>
    /// <param name="interactableObject">Object to represent</param>
    /// <param name="parent">WIP</param>
    /// <returns>GameObject of representation</returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual GameObject Generate(Vector3 position, Quaternion rotation, InteractableObject interactableObject, Transform parent = null)
    {
        throw new NotImplementedException("Generic interactable generator is not implemented yet");
    }
}
