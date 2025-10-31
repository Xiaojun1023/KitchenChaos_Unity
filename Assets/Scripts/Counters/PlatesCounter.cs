using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawn_plate_timer;
    private float spawn_plate_timer_max = 4f;
    private int plate_spawned_amount;
    private int plate_spawned_amount_max = 4;

    private void Update()
    {
        spawn_plate_timer += Time.deltaTime;

        if (spawn_plate_timer > spawn_plate_timer_max)
        {
            spawn_plate_timer = 0f;

            if (GameManager.Instance.IsGamePlaying() && plate_spawned_amount < plate_spawned_amount_max)
            {
                plate_spawned_amount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player does not have a kitchen object
            if (plate_spawned_amount > 0)
            {
                // There are plates available
                plate_spawned_amount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {

        }
    }
}
