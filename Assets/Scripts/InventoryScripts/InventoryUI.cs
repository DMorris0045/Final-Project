using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{

    public GameObject InventoryPanel;
    private bool isOpen;
    private bool IsOpen => isOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseInventory();
    }

    public void OnInventory(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        ToggleInventory();
    }

    public void ToggleInventory()
    {
        if (isOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        isOpen = true;

        if (InventoryPanel != null)
        {
            InventoryPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseInventory()
    {
        isOpen = false;

        if (InventoryPanel != null)
        {
            InventoryPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}