using UnityEngine;

public class InteractableResources : MonoBehaviour
{
    public ItemData item;

    public int amountPerCollect = 1;
    public int usesRemaining = 1;

    public string promptText = "Press E to interact";
    public string animationTrigger = "PickFruit";

    public bool destroyWhenEmpty = true;

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
            inventory.AddItem(item, amountPerCollect);
        }

        usesRemaining--;

        if (usesRemaining <= 0 && destroyWhenEmpty)
        {
            gameObject.SetActive(false);
        }
    }
}