using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AmmoSource : MonoBehaviour
{
    public AmmoType ammoType;
    public int amount;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InventoryComponent>(out var inventory))
        {
            inventory.ammoInventory[ammoType] += (uint)amount;
            Destroy(this.gameObject);
        }
    }
}
