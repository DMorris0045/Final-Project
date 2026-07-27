using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    //item placement fields
    public float placementDist = 3f;
    public float placementRayHeight = 5f;
    public float placementRayDist = 10f;
    public LayerMask placementGroundLayers = ~0;
    private ItemData selectedPlacementItem;

    public float InteractionRange = 3f;
    public TextMeshProUGUI promptText;
    public Inventory inventory;

    private InteractableResources currentResource;
    private CauldronCraftingStation currentCauldron;

    private Animator animator;
    private bool isInteracting;
    private CauldronCraftingStation nearbyCauldron;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        FindNearbyInteractable();
    }

    private void FindNearbyInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, InteractionRange);

        InteractableResources closestResource = null;
        CauldronCraftingStation closestCauldron = null;

        float closestResourceDistance = Mathf.Infinity;
        float closestCauldronDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            InteractableResources resource = hit.GetComponentInParent<InteractableResources>();

            if (resource != null && resource.enabled && resource.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, resource.transform.position);

                if (distance < closestResourceDistance)
                {
                    closestResourceDistance = distance;
                    closestResource = resource;
                }
            }

            CauldronCraftingStation cauldron = hit.GetComponentInParent<CauldronCraftingStation>();

            if (cauldron != null)
            {
                float distance = Vector3.Distance(transform.position, cauldron.transform.position);

                if (distance < closestCauldronDistance)
                {
                    closestCauldronDistance = distance;
                    closestCauldron = cauldron;
                }
            }
        }

        if (closestCauldron != null && closestCauldronDistance <= closestResourceDistance)
        {
            currentCauldron = closestCauldron;
            currentResource = null;
        }
        else
        {
            currentResource = closestResource;
            currentCauldron = null;
        }

        UpdatePrompt();
    }

    private void UpdatePrompt()
    {
        if (promptText == null)
        {
            return;
        }

        if (selectedPlacementItem != null)
        {
            promptText.text = "Press E to place " + selectedPlacementItem.itemName;
            promptText.gameObject.SetActive(true);
            return;
        }

        if (currentCauldron != null)
        {
            promptText.text = currentCauldron.IsOpen ? "Press E to close crafting" : "Press E to craft";
            promptText.gameObject.SetActive(true);
            return;
        }

        if (currentResource != null && currentResource.enabled && !isInteracting)
        {
            promptText.text = currentResource.promptText;
            promptText.gameObject.SetActive(true);
            return;
        }

        promptText.gameObject.SetActive(false);
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (selectedPlacementItem != null)
        {
            PlaceSelectedItem();
            return;
        }

        if (currentCauldron != null)
        {
            currentCauldron.ToggleCraftingMenu();
            return;
        }

        if (currentResource == null || !currentResource.enabled || !currentResource.gameObject.activeInHierarchy || isInteracting)
        {
            return;
        }

        StartCoroutine(InteractRoutine());
    }

    public void OnCraft(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (nearbyCauldron == null)
        {
            return;
        }

        nearbyCauldron.ToggleCraftingMenu();
    }

    public void SetNearbyCauldron(CauldronCraftingStation cauldron)
    {
        nearbyCauldron = cauldron;
    }

    public void ClearNearbyCauldron(CauldronCraftingStation cauldron)
    {
        if (nearbyCauldron == cauldron)
        {
            nearbyCauldron = null;
        }
    }

    private IEnumerator InteractRoutine()
    {
        InteractableResources resourceBeingUsed = currentResource;

        if (resourceBeingUsed == null || !resourceBeingUsed.isActiveAndEnabled || !resourceBeingUsed.gameObject.activeInHierarchy)
        {
            yield break;
        }

        isInteracting = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        if (animator != null && !string.IsNullOrEmpty(resourceBeingUsed.animationTrigger))
        {
            animator.SetTrigger(resourceBeingUsed.animationTrigger);
        }

        yield return new WaitForSeconds(0.8f);

        if (resourceBeingUsed != null && resourceBeingUsed.isActiveAndEnabled && resourceBeingUsed.gameObject.activeInHierarchy)
        {
            resourceBeingUsed.Interact(inventory);
        }

        yield return new WaitForSeconds(0.3f);

        isInteracting = false;
    }

    public void SelectItemForPlacement(ItemData item)
    {
        if (item == null)
        {
            return;
        }

        if (item.itemObject == null)
        {
            return;
        }

        if (inventory == null)
        {
            return;
        }

        int itemCount = inventory.GetItemCount(item);

        if (itemCount <= 0)
        {
            return;
        }

        selectedPlacementItem = item;

        UpdatePrompt();
    }

    private void PlaceSelectedItem()
    {
        if (selectedPlacementItem == null)
        {
            CancelPlacement();
            return;
        }

        if (selectedPlacementItem.itemObject == null)
        {
            CancelPlacement();
            return;
        }

        Vector3 targetPosition = transform.position + transform.forward * placementDist;

        Vector3 rayStart = targetPosition + Vector3.up * placementRayHeight;

        if (Physics.Raycast( rayStart, Vector3.down, out RaycastHit hit, placementRayDist, placementGroundLayers, QueryTriggerInteraction.Ignore))
        {
            targetPosition = hit.point;
        }

        targetPosition += Vector3.up * selectedPlacementItem.placementHeightOffset;

        Quaternion placementRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        bool removedItem = inventory.RemoveItem(selectedPlacementItem, 1);

        if (!removedItem)
        {
            CancelPlacement();
            return;
        }

        GameObject placedObject = Instantiate(selectedPlacementItem.itemObject, targetPosition, placementRotation);

        Renderer[] renderers = placedObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }

            float amountBelowGround = targetPosition.y - combinedBounds.min.y;

            placedObject.transform.position += Vector3.up * amountBelowGround;
        }

        selectedPlacementItem = null;
        UpdatePrompt();
    }

    public void CancelPlacement()
    {
        selectedPlacementItem = null;
        UpdatePrompt();
    }
}