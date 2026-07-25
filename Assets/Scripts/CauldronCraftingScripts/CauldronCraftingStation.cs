using UnityEngine;

public class CauldronCraftingStation : MonoBehaviour
{
    [SerializeField] private GameObject craftingMenu;

    public bool IsOpen => craftingMenu != null && craftingMenu.activeSelf;

    private void Start()
    {
        if (craftingMenu == null)
        {
            return;
        }

        craftingMenu.SetActive(false);
    }

    public void ToggleCraftingMenu()
    {
        if (craftingMenu == null)
        {
            return;
        }

        bool shouldOpen = !craftingMenu.activeSelf;
        craftingMenu.SetActive(shouldOpen);

        Cursor.visible = shouldOpen;
        Cursor.lockState = shouldOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CloseCraftingMenu()
    {
        if (craftingMenu == null)
        {
            return;
        }

        craftingMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}