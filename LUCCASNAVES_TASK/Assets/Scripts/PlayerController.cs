using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerUI playerUI;

    public Transform player;

    public Inventory playerInventory;
    public Interactor playerInteractor;
    public PlayerMovementController movementController;
    public PlayerAnimationController animationController;

    private void Awake()
    {
        playerInteractor.onHover += OnHover;
        playerInteractor.onUnhover += OnUnhover;
        playerInteractor.onInteract += OnInteract;

        playerInventory.onDropOutside += OnDropItem;
    }

    private void OnHover(IInteractable interactable)
    {
        playerUI.SetInteractionTooltip(true);
    }

    private void OnUnhover(IInteractable interactable)
    {
        playerUI.SetInteractionTooltip(false);
    }
    private void OnInteract(IInteractable interactable)
    {
        if(interactable is WorldItem)
        {
            WorldItem worldItem = interactable as WorldItem;
            if(playerInventory.TryAddItem(worldItem.itemIdentifier, 1))
            {
                Destroy(worldItem.gameObject);
            }
        }
    }
    private void OnDropItem(ItemIdentifier identifier, int amount)
    {
        Vector3 position = player.position;

        WorldItem worldItem = Instantiate(identifier.worldItemPrefab, position, Quaternion.identity);
        worldItem.itemIdentifier = identifier;
        worldItem.amount = amount;
    }
}
