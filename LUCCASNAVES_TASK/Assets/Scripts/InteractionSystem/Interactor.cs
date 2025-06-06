using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
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

    public void Interact()
    {
        if (currentHover != null)
        {
            currentHover.OnInteract();
            onInteract?.Invoke(currentHover);
        }
    }
}

public interface IInteractable
{
    public void OnHover();
    public void OnUnhover();
    public void OnInteract();
}