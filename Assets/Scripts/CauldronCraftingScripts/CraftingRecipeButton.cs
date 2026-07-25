using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CraftingRecipeButton : MonoBehaviour
{

    [SerializeField] private CraftingManager craftingManager;
    [SerializeField] private CraftingRecipe recipe;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(CraftRecipe);
        UpdateButtonState();
    }

    private void Update()
    {
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(CraftRecipe);
        }
    }

    private void CraftRecipe()
    {
        if (craftingManager == null)
        {
            return;
        }

        if (recipe == null)
        {
            return;
        }

        craftingManager.Craft(recipe);
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (button == null)
        {
            return;
        }

        if (craftingManager == null || recipe == null)
        {
            button.interactable = false;
            return;
        }

        button.interactable = craftingManager.CanCraft(recipe);
    }
}
