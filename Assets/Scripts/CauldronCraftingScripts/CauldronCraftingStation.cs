using UnityEngine;

public class CauldronCraftingStation : MonoBehaviour
{

    private GameObject craftingMenu;

    public bool IsOpen => craftingMenu != null && craftingMenu.activeSelf;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (craftingMenu == null)
        {
            return;
        }

        craftingMenu.SetActive(false);
        SetCursorState(false);
    }

    public void ToggleCraftingMenu()
    {
        if (craftingMenu == null)
        {
            return;
        }

        bool shouldOpen = !craftingMenu.activeSelf;
        craftingMenu.SetActive(shouldOpen);

        SetCursorState(shouldOpen);
    }

    public void CloseCraftingMenu()
    {
        if (craftingMenu == null)
        {
            return;
        }

        craftingMenu.SetActive(false);
        SetCursorState(false);
    }

    private void SetCursorState(bool menuOpen)
    {
        Cursor.visible = menuOpen;

        Cursor.lockState = menuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
