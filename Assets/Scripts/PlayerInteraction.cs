using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float InteractionRange = 3f;
    public TextMeshProUGUI promptText;
    public Inventory inventory;

    private InteractableResources currentResource;
    private CauldronCraftingStation currentCauldron;

    private Animator animator;
    private bool isInteracting;

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

            if (resource != null)
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

        if (currentCauldron != null)
        {
            promptText.text = currentCauldron.IsOpen ? "Press F to close crafting" : "Press F to craft";

            promptText.gameObject.SetActive(true);
            return;
        }

        if (currentResource != null && !isInteracting)
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

        if (currentCauldron != null)
        {
            currentCauldron.ToggleCraftingMenu();
            return;
        }

        if (currentResource == null || isInteracting)
        {
            return;
        }

        StartCoroutine(InteractRoutine());
    }

    private IEnumerator InteractRoutine()
    {
        isInteracting = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        if (animator != null && !string.IsNullOrEmpty(currentResource.animationTrigger))
        {
            animator.SetTrigger(currentResource.animationTrigger);
        }

        yield return new WaitForSeconds(0.8f);

        if (currentResource != null)
        {
            currentResource.Interact(inventory);
        }

        yield return new WaitForSeconds(0.3f);

        isInteracting = false;
    }
}