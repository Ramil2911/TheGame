using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// User's inventory, very primitive version
/// </summary>
public class InventoryComponent : MonoBehaviour
{
    /// <summary>
    /// Invoked when active slot changed
    /// </summary>
    public UnityEvent<InventoryItem, int> activeSlotChangedEvent = new UnityEvent<InventoryItem, int>();
    /// <summary>
    /// Capacity of one slot
    /// </summary>
    private const int SlotCapacity = 1;
    /// <summary>
    /// Current active slot
    /// </summary>
    public ushort activeSlot = 0;

    private const int SIZE = 2;
    /// <summary>
    /// Player's whole inventory
    /// </summary>
    [SerializeField] public InventoryItem[] inventory = new InventoryItem[SIZE];
    public Dictionary<AmmoType, uint> ammoInventory = new Dictionary<AmmoType, uint>();

    /// <summary>
    /// Current active slot
    /// </summary>
    public InventoryItem ActiveSlotItem => inventory[activeSlot];
    
    public TheFirstPerson.FPSController controller;
    private GameObject _currentHands;
    public WeaponObject placeholder;
    public WeaponController weaponController;
    
    private static readonly int State = Animator.StringToHash("State");
    


    /// <summary>
    /// Checks can items be added to inventory or not
    /// </summary>
    /// <param name="interactable">An object to add</param>
    /// <param name="amount">Amount of object to add</param>
    /// <returns>Boolean if can abject be added or not</returns>
    public bool CanAdd(WeaponObject interactable, ushort amount)
    {
        for (var index = 0; index < inventory.Length; index++)
        {
            var item = inventory[index];
            if (item.interactableObject == null ||
                (item.interactableObject.InteractableName == interactable.InteractableName && interactable.IsStackable && item.amount + amount <= SlotCapacity))
            {
                item.interactableObject = interactable;
                item.amount++;
                inventory[index] = item;
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// Adds items to inventory
    /// </summary>
    /// <param name="interactable">An object to add</param>
    /// <param name="amount">Amount of object to add</param>
    /// <returns>Boolean if was object added or not</returns>
    public bool Add(WeaponObject interactable, ushort amount)
    {
        for (var index = 0; index < inventory.Length; index++)
        {
            var item = inventory[index];
            if (item.interactableObject == null)
                //|| (item.interactableObject.InteractableName == interactable.InteractableName && interactable.IsStackable && item.amount + amount <= SlotCapacity))
            {
                item.interactableObject = interactable;
                item.amount += amount;
                inventory[index] = item;
                activeSlotChangedEvent.Invoke(ActiveSlotItem, activeSlot);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if can add item to slot
    /// </summary>
    /// <param name="interactable">An object to add</param>
    /// <param name="amount">Amount of object to add</param>
    /// <param name="slot">Index of slot to add to</param>
    /// <returns>Boolean if was object can be added or not</returns>w
    public bool CanToSlot(WeaponObject interactable, ushort amount, ushort slot)
    {
        var item = inventory[slot];
        if (item.interactableObject == null ||
            (item.interactableObject.InteractableName == interactable.InteractableName && interactable.IsStackable && item.amount + amount <= SlotCapacity))
        {
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Adds item to slot
    /// </summary>
    /// <param name="interactable">An object to add</param>
    /// <param name="amount">Amount of object to add</param>
    /// <param name="slot">Index of slot to add to</param>
    /// <returns>Boolean if was object was added or not</returns>
    public bool AddToSlot(WeaponObject interactable, ushort amount, ushort slot)
    {
        var item = inventory[slot];
        if (item.interactableObject == null ||
            (item.interactableObject.InteractableName == interactable.InteractableName && interactable.IsStackable && item.amount + amount <= SlotCapacity))
        {
            item.interactableObject = interactable;
            item.amount += amount;
            inventory[slot] = item;
            return true;
        }

        return false;
    }

    private void Start()
    {
        activeSlotChangedEvent.AddListener(ActiveItemChangedListener);
        controller = GetComponent<TheFirstPerson.FPSController>();
        activeSlotChangedEvent.Invoke(ActiveSlotItem, activeSlot);
        
        foreach (var suit in (AmmoType[]) Enum.GetValues(typeof(AmmoType)))
        {
            ammoInventory.Add(suit, 0);
        }
    }

    private void ActiveItemChangedListener(InventoryItem item, int index)
    {
        //create view from prefab
        GameObject newHands;
        var transform1 = transform;
        if (item.interactableObject != null)
        {
            newHands = Instantiate(item.interactableObject.interactableInHands, transform1.position + Vector3.up * 2, _currentHands == null ? Quaternion.identity : _currentHands.transform.rotation, transform1);
            controller.cam = newHands.transform;
        }
        else
        {
            newHands = Instantiate(placeholder.interactableInHands, transform1.position + Vector3.up * 2, _currentHands == null ? Quaternion.identity : _currentHands.transform.rotation, transform1);
            controller.cam = newHands.transform;
        }
        Destroy(_currentHands);
        _currentHands = newHands;
        
        //set animator

        weaponController.animator = _currentHands.GetComponent<Animator>();
        weaponController.vfxController = _currentHands.GetComponent<VFXController>();
        weaponController.weaponObject = item.interactableObject;
    }

    public void FixedUpdate()
    {
        var startValue = activeSlot;
        if (Input.GetAxis("Mouse ScrollWheel")*100 > 0f)
        {
            activeSlot--;
        }
        else if (Input.GetAxis("Mouse ScrollWheel")*100 < 0f)
        {
            activeSlot++;
        }
        activeSlot = (ushort) (activeSlot % SIZE);
        if(activeSlot!=startValue) activeSlotChangedEvent.Invoke(ActiveSlotItem, activeSlot);
    }

    private void OnValidate()
    {
        if (inventory.Length != SIZE)
        {
            Debug.LogError("NEVER CHANGE SIZE OF INVENTORY");
            Array.Resize(ref inventory, SIZE);
        }
    }
}

[Serializable]
public class InventoryItem
{
    [SerializeField]
    public WeaponObject interactableObject;
    [SerializeField]
    public ushort amount;
}