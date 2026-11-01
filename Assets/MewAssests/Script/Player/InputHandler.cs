using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //private ICommand moveCommand; Interesting Idea
    private PlayerController playerController;
    private PlayerPushAndPull pushAndPull;
    private PlayerClimb playerClimb;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        pushAndPull = GetComponentInChildren<PlayerPushAndPull>();
        playerClimb = GetComponentInChildren<PlayerClimb>();
    }

    void Start()
    {
        // moveCommand = new MoveCommand(playerController); Interesting Idea

        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Run");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");
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
            playerController.move.IsGrounded = false;
            if (playerController.climb.IsHold)
            {
                playerController.move.IsWalljump = true;
            }
        }

        if (interactAction.triggered && pushAndPull.currentInteractable != null && playerClimb.currentInteractable == null && !playerController.move.IsJumping)
        {
            playerController.push.IsHold = !playerController.push.IsHold;

            pushAndPull.OnInteract();
        }
        else if(interactAction.triggered && playerClimb.currentInteractable != null && pushAndPull.currentInteractable == null)
        {
            playerController.climb.IsHold = !playerController.climb.IsHold;
            playerController.move.IsClimbing = playerController.climb.IsHold;
            
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
    }
}
