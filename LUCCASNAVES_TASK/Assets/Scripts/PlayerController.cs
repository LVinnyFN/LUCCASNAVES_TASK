using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerUI playerUI;

    public Interactor playerInteractor;
    public PlayerMovementController movementController;
    public PlayerAnimationController animationController;

    private void Awake()
    {
        playerInteractor.onHover += OnHover;
        playerInteractor.onUnhover += OnUnhover;
    }

    private void OnHover(IInteractable interactable)
    {
        playerUI.SetInteractionTooltip(true);
    }

    private void OnUnhover(IInteractable interactable)
    {
        playerUI.SetInteractionTooltip(false);
    }
}
