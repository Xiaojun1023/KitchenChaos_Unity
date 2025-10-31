using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no kitchenobject here
            if (player.HasKitchenObject())
            {
                // Player is carrying the kitchenobject
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player is not carrying a kitchenobject
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                // Player is carring something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject1))
                {
                    // Player is carrying a plate
                    PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is not carrying a plate but something else
                    if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {
                        // Counter has a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}