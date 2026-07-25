using UnityEngine;

public class CauldronInteraction : MonoBehaviour
{
    private CauldronCraftingStation craftingStation;

    private void Awake()
    {
        if (craftingStation == null)
        {
            craftingStation = GetComponentInParent<CauldronCraftingStation>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();

        if (interaction == null)
        {
            return;
        }

        interaction.SetNearbyCauldron(craftingStation);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();

        if (interaction != null)
        {
            interaction.ClearNearbyCauldron(craftingStation);
        }

        craftingStation.CloseCraftingMenu();
    }
}
