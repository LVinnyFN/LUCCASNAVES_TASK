using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour, PlayerInputs.IPlayerActions
{
    private PlayerInputs inputs;
    public float range = 2.0f;
    public float radius = 1.0f;
    public LayerMask mask;
    public Transform origin;

    [Space]
    public IInteractable currentHover;

    public Action<IInteractable> onHover;
    public Action<IInteractable> onUnhover;
    public Action<IInteractable> onInteract;

    private void Update()
    {
        CheckForInteraction();
    }

    private void CheckForInteraction()
    {
        Ray ray = new Ray(origin.position, origin.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, range, mask))
        {
            if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                if(interactable == currentHover)
                {

                }
                else
                {
                    StopHovering();
                    currentHover = interactable;
                    currentHover.OnHover();
                    onHover?.Invoke(currentHover);
                }
            }
            else
            {
                StopHovering();
            }
        }
        else
        {
            StopHovering();
        }

        void StopHovering()
        {
            if (currentHover != null)
            {
                currentHover.OnUnhover();
                currentHover = null;
                onUnhover?.Invoke(currentHover);
            }
        }
    }

    private void Interact()
    {
        if (currentHover != null)
        {
            currentHover.OnInteract();
            onInteract?.Invoke(currentHover);
        }
    }

    //TODO: Clean the input actions
    #region INPUT
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
    public void OnMove(InputAction.CallbackContext context)
    {
    }
    public void OnLook(InputAction.CallbackContext context)
    {
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
    }
    public void OnJump(InputAction.CallbackContext context)
    {
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
    }
    #endregion
}

public interface IInteractable
{
    public void OnHover();
    public void OnUnhover();
    public void OnInteract();
}