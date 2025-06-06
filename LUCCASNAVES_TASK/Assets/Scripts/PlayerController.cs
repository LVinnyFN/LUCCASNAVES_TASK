using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour, PlayerInputs.IPlayerActions
{
    public PlayerUI playerUI;
    public PlayerInputs inputs;

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
            if(playerInventory.TryAddItem(worldItem.itemIdentifier, worldItem.amount))
            {
                Destroy(worldItem.gameObject);
                animationController.PlayCollectAnimation();
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

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
        inputs.Player.Enable();
        inputs.Player.SetCallbacks(this);
    }
    private void OnDisable()
    {
        inputs.Player.Disable();
        inputs.Player.RemoveCallbacks(this);
    }

    /// <summary>
    /// Checks if the player is in contact with the ground
    /// </summary>
    #region INPUTS
    public void OnMove(InputAction.CallbackContext context)
    {
        movementController.Move(context.ReadValue<Vector2>());
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        movementController.Look(context.ReadValue<Vector2>());
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;
        movementController.Jump();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed) playerInteractor.Interact();
    }
    #endregion
}
