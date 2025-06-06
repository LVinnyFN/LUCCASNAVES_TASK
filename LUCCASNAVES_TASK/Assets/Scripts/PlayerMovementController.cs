using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    public Camera playerCamera;
    public CharacterController characterController;
    public PlayerAnimationController animationController;

    [Header("Settings")]
    public float movementSpeed = 1.0f;
    public float jumpSpeed = 1.0f;
    public float lookSpeed = 1.0f;
    public float gravity = 9.8f;
    public Vector2 lookVerticalAngle = new Vector2(30, 60);
    public LayerMask groundLayer;

    [Header("Inputs")]
    public bool jump;
    public Vector2 normalizedMovementInput;
    public Vector2 movementInput;
    public Vector2 lookInput;

    private Vector3 movement;
    private Vector3 playerVelocity;
    private Vector2 playerRotation;
    private Vector2 cameraRotation;

    private void Update()
    {
        movement = Vector2.zero;

        HandleHorizontalMovement();
        HandleVerticalMovement();
        UpdateMovement();

        UpdateRotation();

        UpdateAnimation(movementInput);
    }

    private void HandleHorizontalMovement()
    {
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0.0f;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0.0f;

        movement = cameraForward * normalizedMovementInput.y + cameraRight * normalizedMovementInput.x;
        movement *= movementSpeed * Time.deltaTime;
    }
    private void HandleVerticalMovement()
    {
        if (IsGrounded())
        {
            playerVelocity.y = Mathf.Max(playerVelocity.y, 0.0f);
            if (jump)
            {
                jump = false;
                playerVelocity.y += Mathf.Sqrt(jumpSpeed * 3.0f * gravity);
            }
        }
        else
        {
            playerVelocity.y -= gravity * Time.deltaTime;
        }
        movement += playerVelocity;
    }
    private void UpdateMovement()
    {
        characterController.Move(movement);
    }
    private void UpdateRotation()
    {
        cameraRotation.x += lookInput.x * lookSpeed;
        cameraRotation.y += -lookInput.y * lookSpeed;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, lookVerticalAngle.x, lookVerticalAngle.y);

        playerRotation.x += transform.eulerAngles.x + (lookInput.x * lookSpeed);
        transform.rotation = Quaternion.Euler(0.0f, playerRotation.x, 0.0f);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0.0f);
    }    
    private void UpdateAnimation(Vector2 direction)
    {
        if (direction.y < 0.0f) direction.x = 0.0f;
        animationController.UpdateDirectionAnimation(direction);
    }

    
    private bool IsGrounded()
    {
        Vector3 groundDetectorPosition = transform.position;
        groundDetectorPosition.y += 0.15f;
        return Physics.CheckSphere(groundDetectorPosition, 0.5f, groundLayer, QueryTriggerInteraction.Ignore);
    }

    public void Move(Vector2 movementInput)
    {
        this.movementInput = movementInput;
        this.normalizedMovementInput = this.movementInput.normalized;
    }
    public void Look(Vector2 lookInput)
    {
        this.lookInput = lookInput;
    }
    public void Jump()
    {
        jump = true;
    }
}