using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerInputs.IPlayerActions
{
    public PlayerInputs inputs;
    public Camera playerCamera;
    public CharacterController characterController;

    [Header("Settings")]
    public float movementSpeed = 1.0f;
    public float jumpSpeed = 1.0f;
    public float lookSpeed = 1.0f;

    [Header("Inputs")]
    public float gravity = 9.8f;
    public bool jump;
    public Vector2 movementInput;
    public Vector2 lookInput;
    public Vector2 lookVerticalAngle = new Vector2(30, 60);

    public LayerMask groundLayer;

    public Vector3 movement;
    public Vector3 playerVelocity;
    private Vector2 playerRotation;
    private Vector2 cameraRotation;

    private void Awake()
    {
    }

    private void Update()
    {
        //MOVEMENT
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0.0f;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0.0f;

        movement = cameraForward * movementInput.y + cameraRight * movementInput.x;
        movement *= movementSpeed * Time.deltaTime;

        // JUMP
        bool isGrounded = IsGrounded();       
        if (isGrounded)
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
        characterController.Move(movement);

        //ROTATION
        cameraRotation.x += lookInput.x * lookSpeed;
        cameraRotation.y += -lookInput.y * lookSpeed;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, lookVerticalAngle.x, lookVerticalAngle.y);

        playerRotation.x += transform.eulerAngles.x + (lookInput.x * lookSpeed);
        transform.rotation = Quaternion.Euler(0.0f, playerRotation.x, 0.0f);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0.0f);
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

    #region INPUTS

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;
        jump = true;
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
    }
    #endregion

    private bool IsGrounded()
    {
        Vector3 groundDetectorPosition = transform.position;
        groundDetectorPosition.y += 0.15f;
        return Physics.CheckSphere(groundDetectorPosition, 0.5f, groundLayer, QueryTriggerInteraction.Ignore);
    }
}