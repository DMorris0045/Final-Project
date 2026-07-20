using UnityEngine;

public class Inventory : MonoBehaviour
{

    public InventorySlotUI[] slotUIs;
    public InventorySlotData[] slots;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = new InventorySlotData[slotUIs.Length];
        UpdateUI();
    }

    public void AddItem(ItemData item, int amount)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            if (slots[i].item == item && slots[i].amount < item.maxStackSize)
            {
                int spaceLeft = item.maxStackSize - slots[i].amount;
                int amountToAdd = Mathf.Min(spaceLeft, amount);

                slots[i].amount += amountToAdd;
                amount -= amountToAdd;

                if (amount <= 0)
                {
                    UpdateUI();
                    return;
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                continue;
            }

            int amountToAdd = Mathf.Min(item.maxStackSize, amount);
            slots[i] = new InventorySlotData(item, amountToAdd);
            amount -= amountToAdd;

            if (amount <= 0)
            {
                UpdateUI();
                return;
            }
        }

        UpdateUI();
       
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        if (GetItemCount(item) < amount)
        {
            return false;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            if (slots[i].item != item)
            {
                continue; 
            }

            int amountToRemove = Mathf.Min(slots[i].amount, amount);

            slots[i].amount -= amountToRemove;
            amount -= amountToRemove;

            if (slots[i].amount <= 0)
            {
                slots[i] = null;
            }

            if (amount <= 0)
            {
                UpdateUI();
                return true;
            }
        }

        UpdateUI();
        return true;
    }

    public int GetItemCount(ItemData item)
    {
        int totalAmount = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            if (slots[i].item == item)
            {
                totalAmount += slots[i].amount;
            }
        }

        return totalAmount;
    }
    
    private void UpdateUI()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            if (slots[i] == null)
            {
                slotUIs[i].ClearSlot();
            }
            else
            {
                slotUIs[i].SetSlot(slots[i].item, slots[i].amount);
            }
        }
    }

}
