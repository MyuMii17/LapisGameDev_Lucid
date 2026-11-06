using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //private ICommand moveCommand; Interesting Idea
    private PlayerController playerController;
    private PlayerPush playerPush;
    private PlayerClimb playerClimb;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction crouchAction;
    public Transform crouchRayPosition;
    public float rayRange;
    public int edgeLayer;
    public bool isCrouchHit;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerPush = GetComponentInChildren<PlayerPush>();
        playerClimb = GetComponentInChildren<PlayerClimb>();
        edgeLayer = LayerMask.GetMask("Edge");
    }

    void Start()
    {
        // moveCommand = new MoveCommand(playerController); Interesting Idea

        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Run");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");
        crouchAction = InputSystem.actions.FindAction("Crouch");
    }

    void Update()
    {
        if (moveAction.IsPressed())
        {
            playerController.move.IsWalking = true;

            playerController.Move();
            playerController.move.IsRunning = runAction.IsPressed();
            // moveCommand.Excute() Interesting Idea
        }
        else
        {
            playerController.move.IsWalking = false;
            playerController.move.IsRunning = false;
        }

        if (jumpAction.triggered && playerController.move.CanJump)
        {
            if(playerController.move.IsSliding) playerController.move.IsSliding = false;
            
            playerController.move.IsGrounded = false;
            if (playerController.climb.IsHold)
            {
                playerController.move.IsWalljump = true;
            }
        }

        if (interactAction.triggered && playerPush.currentInteractable != null && playerClimb.currentInteractable == null && !playerController.move.IsJumping)
        {
            playerController.push.IsHold = !playerController.push.IsHold;

            playerPush.OnInteract();
        }
        else if(interactAction.triggered && playerClimb.currentInteractable != null && playerPush.currentInteractable == null)
        {
            playerController.climb.IsHold = !playerController.climb.IsHold;
            
            playerController.ResetVelocity();

            if (playerController.climb.IsHold == true)
            {
                playerController.move.IsGrounded = true;
                playerController.move.IsJumping = false;
                playerController.move.CanJump = true;
                playerController.move.IsWalljump = false;

                Vector3 position = transform.position;
                position.y += playerController.climb.RayHeight;

                Debug.DrawRay(position , playerController.move.MoveDirection * playerController.climb.RayRange , Color.red);
                if (Physics.Raycast(position, playerController.move.MoveDirection, out RaycastHit hit, playerController.climb.RayRange, playerController.climb.ClimbLayer) && playerController.climb.IsHold)
                {
                    Vector3 wallDirection = -hit.normal;
                    wallDirection.y = 0f;

                    playerController.move.MoveDirection = wallDirection;
                    playerController.climb.WallDirection = wallDirection;

                    transform.rotation = Quaternion.LookRotation(wallDirection);
                }
            }
            
            playerClimb.OnInteract();
        }

        if (playerController.move.IsCrouching)
        {
            isCrouchHit = Physics.Raycast(crouchRayPosition.position, Vector3.up, rayRange, edgeLayer);
        }

        if (crouchAction.IsPressed())
        {
            playerController.move.IsCrouching = true;
            if(playerController.move.IsRunning && playerController.move.CanSlide) playerController.move.IsSliding = true;
        }
        else if (!crouchAction.IsPressed() && !isCrouchHit && !playerController.move.IsSliding)
        {
            playerController.move.IsCrouching = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isCrouchHit ? Color.red : Color.green; // ถ้าชนจะเป็นสีแดง ถ้าไม่ชนจะเป็นสีเขียว

        Gizmos.DrawLine(crouchRayPosition.position, crouchRayPosition.position + Vector3.up * rayRange);
    }
}
