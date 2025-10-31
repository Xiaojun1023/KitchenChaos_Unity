using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        // Logic to add an ingredient to the plate
        // This could involve checking if the ingredient can be added,
        // updating the plate's state, etc.
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false; // Ingredient not valid for this plate
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            Debug.Log("Ingredient already exists on the plate.");
            return false; // Ingredient already exists
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs 
            { 
                KitchenObjectSO = kitchenObjectSO 
            });

            return true; // Ingredient added successfully
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
