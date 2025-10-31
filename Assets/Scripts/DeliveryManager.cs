using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawn_recipe_timer;
    private float spawn_recipe_timer_max = 4f;
    private int waiting_recipe_max = 4;
    private int successful_recipes_amount;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawn_recipe_timer -= Time.deltaTime;

        if (spawn_recipe_timer <= 0f)
        {
            spawn_recipe_timer = spawn_recipe_timer_max;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waiting_recipe_max)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plate_contents_match_recipe = true;
                // Has the same number of ingredients
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredient_found = false;
                    // Cycle through all ingredients in the recipe
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycle through all ingredients on the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient matches
                            ingredient_found = true;
                            break; // Break out of the inner loop
                        }
                    }
                    if (!ingredient_found)
                    {
                        // Ingredient not found on the plate
                        plate_contents_match_recipe = false;
                    }
                }

                if (plate_contents_match_recipe)
                {
                    // Player delivered the correct recipe
                    successful_recipes_amount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // No matching recipe found
        // Player delivered an incorrect recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successful_recipes_amount;
    }
}
