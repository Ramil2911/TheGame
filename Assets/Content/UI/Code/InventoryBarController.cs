
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBarController : MonoBehaviour
{
    [SerializeField] private InventoryComponent inventoryComponent;

    private InventoryUIItem[] bar = new InventoryUIItem[8];
    
    void Start()
    {
        for (var i = 0; i < 1; i++)
        {
            var item = this.transform.GetChild(i);

            bar[i] = new InventoryUIItem()
            {
                panelImage = item.GetComponent<Image>(),
                spriteImage = item.GetChild(0).GetComponent<Image>(),
                text = item.GetChild(1).GetComponent<TextMeshProUGUI>()
            };
        }
        SlotChangedListener(inventoryComponent.ActiveSlotItem, inventoryComponent.activeSlot);
        inventoryComponent.activeSlotChangedEvent.AddListener(SlotChangedListener);
    }

    private void SlotChangedListener(InventoryItem item, int index)
    {
        for (var i = 0; i < 1; i++)
        {
            if (inventoryComponent.inventory[i].interactableObject != null)
            {
                bar[i].panelImage.color = i == index ? Color.grey : Color.red;
                bar[i].spriteImage.sprite = inventoryComponent.inventory[i].interactableObject.Sprite;
                bar[i].text.text = inventoryComponent.inventory[i].interactableObject.IsStackable
                    ? inventoryComponent.inventory[i].amount.ToString()
                    : "";
            }
            else
            {
                bar[i].panelImage.color = Color.red;
                bar[i].spriteImage.sprite = null;
                bar[i].text.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

internal struct InventoryUIItem
{
    public Image panelImage;
    public Image spriteImage;
    public TextMeshProUGUI text;
}
