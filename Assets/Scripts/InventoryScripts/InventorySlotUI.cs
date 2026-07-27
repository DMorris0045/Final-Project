using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerDownHandler
{
    public Image itemIcon;
    public TextMeshProUGUI stackText;

    public PlayerInteraction playerInteraction;
    public InventoryUI inventoryUI;

    private ItemData currentItem;
    private int currentAmount;

    public void SetSlot(ItemData item, int amount)
    {
        currentItem = item;
        currentAmount = amount;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
        }

        if (stackText != null)
        {
            stackText.text = amount.ToString();
            stackText.gameObject.SetActive(true);
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentAmount = 0;

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
        }

        if (stackText != null)
        {
            stackText.text = "";
            stackText.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (currentItem == null)
        {
            return;
        }

        if (currentAmount <= 0)
        {
            return;
        }

        if (currentItem.itemObject == null)
        {
            return;
        }

        if (playerInteraction == null)
        {
            return;
        }

        playerInteraction.SelectItemForPlacement(currentItem);

        if (inventoryUI != null)
        {
            inventoryUI.CloseInventory();
        }
    }
}