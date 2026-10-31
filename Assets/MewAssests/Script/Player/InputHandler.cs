using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //private ICommand moveCommand; Interesting Idea
    private PlayerController playerController;
    [SerializeField]private PlayerPushAndPull pushAndPull;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
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
        }

        if (interactAction.triggered && pushAndPull.currentInteractable != null)
        {
            playerController.push.IsHold = !playerController.push.IsHold;
            pushAndPull.OnInteract();
        }
    }
}
