using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AddItemToInventory : MonoBehaviour
{
    public WeaponObject weapon;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InventoryComponent>(out var inventory))
        {
            var res = inventory.Add(weapon, 1);
            Destroy(this.gameObject);
        }
    }
}
