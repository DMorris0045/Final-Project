using UnityEngine;

public class InteractableResources : MonoBehaviour
{
    public ItemData item;

    public bool useRandomAmount;
    public int amountPerCollect = 1;
    public int minAmount = 1;
    public int maxAmount = 3;

    public int usesRemaining = 1;
    public string promptText = "Press E to interact";
    public string animationTrigger = "PickFruit";

    public bool destroyWhenEmpty = true;

    //audio for chopping wood
    public AudioClip woodGatherSound;
    private bool isWood;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isWood = usesRemaining == 5;
    }

    public void Interact(Inventory inventory)
    {
        if (usesRemaining <= 0)
        {
            return;
        }

        if (isWood && woodGatherSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(woodGatherSound);
        }

        if (item != null && inventory != null)
        {
            int collectedAmount = amountPerCollect;

            if (useRandomAmount)
            {
                collectedAmount = Random.Range(minAmount, maxAmount + 1);
            }

            inventory.AddItem(item, collectedAmount);
        }

        usesRemaining--;

        if (usesRemaining <= 0 && destroyWhenEmpty)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetResource(int newUsesRemaining = 1)
    {
        usesRemaining = newUsesRemaining;
        gameObject.SetActive(true);
    }
}