using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public Inventory inventory;
    public List<CraftingRecipe> recipes;

    public void Craft(CraftingRecipe recipe)
    {

        if (recipe == null || inventory == null)
        {
            return;
        }

        if (!CanCraft(recipe))
        {
            return;
        }

        ConsumeIngredients(recipe);
        inventory.AddItem(recipe.result, recipe.resultAmount);
        Debug.Log("Crafted " + recipe.recipeName);
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        if (recipe == null || inventory == null)
        {
            return false;
        }

        foreach (ItemAmount ingredient in recipe.ingredients)
        {
            if (ingredient.item == null)
            {
                return false;
            }

            int itemCount = inventory.GetItemCount(ingredient.item);

            if (itemCount < ingredient.amount)
            {
                return false;
            }
        }

        return true;
    }

    private void ConsumeIngredients(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            inventory.RemoveItem(ingredient.item, ingredient.amount);
        }
    }
}