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

        if (CanCraft(recipe))
        {
            ConsumeIngredients(recipe);
            inventory.AddItem(recipe.result, recipe.resultAmount);
            CreateResult(recipe);
        }
        else
        {
            Debug.Log("Cannot craft " + recipe.recipeName + " due to missing ingredients.");
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
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

    private void CreateResult(CraftingRecipe recipe)
    {
        for (int i = 0; i < recipe.resultAmount; i++)
        {
            inventory.AddItem(recipe.result, recipe.resultAmount);
        }
    }
}