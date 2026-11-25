using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    //private ICommand moveCommand; Interesting Idea
    public PullObject pullObject;
    private GameManager gameManager;
    private PlayerController playerController;
    private PlayerPush playerPush;
    private PlayerClimb playerClimb;
    private PlayerPick playerPick;
    private PlayerInteract playerInteract;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction crouchAction;
    private InputAction menuAction;
    public Transform crouchRayPosition;
    public float rayRange;
    public int objectLayer;
    public bool isCrouchHit;
    // public bool isWallHit;
    // public float wallRayDistance;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerPush = GetComponentInChildren<PlayerPush>();
        playerClimb = GetComponentInChildren<PlayerClimb>();
        playerPick = GetComponentInChildren<PlayerPick>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        objectLayer = LayerMask.GetMask("Object");
    }

    void Start()
    {
        // moveCommand = new MoveCommand(playerController); Interesting Idea

        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Run");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        menuAction = InputSystem.actions.FindAction("Menu");

        gameManager = GameManager.GetInstace();
    }

    void Update()
    {
        if(gameManager.isOnCheckpoint || gameManager.isGameOver) return;
        Vector3 position1 = transform.position;
        position1.y += playerController.push.RayHeight;
        Debug.DrawRay(position1 , playerController.move.MoveDirection * playerController.climb.RayRange , Color.red);

        if (playerController.push.IsPull)
        {
            WalkTimeWhilePull();
        }

        if (moveAction.IsPressed())
        {
            playerController.Move();
            playerController.move.IsRunning = runAction.IsPressed() && playerController.move.IsWalking && !playerController.move.IsCrouching;
            // moveCommand.Excute() Interesting Idea
        }
        else
        {
            playerController.move.IsWalking = false;
            playerController.move.IsRunning = false;
        }

        if (jumpAction.triggered && playerController.move.IsGrounded)
        {
            if(playerController.move.IsSliding) playerController.move.IsSliding = false;
            
            playerController.move.IsGrounded = false;
            playerController.move.CanJump = true;
            if (playerController.climb.IsHold)
            {
                playerController.move.IsWalljump = true;
            }
        }

        if (interactAction.triggered && playerPush.currentInteractable != null && playerClimb.currentInteractable == null && playerPick.currentInteractable == null && !playerController.move.IsJumping && !playerPush.target.pull.IsEndPull && !playerController.move.IsJumping && playerPush.target.canPush)
        {
            playerController.push.IsHold = !playerController.push.IsHold;
            if (playerController.push.IsHold == true)
            {
                Vector3 position = transform.position;
                position.y += playerController.push.RayHeight;

                Debug.DrawRay(position , playerController.move.MoveDirection * playerController.push.RayRange , Color.red);
                if (Physics.Raycast(position, playerController.move.MoveDirection, out RaycastHit hit, playerController.push.RayRange, objectLayer) && playerController.push.IsHold)
                {
                    Vector3 pushDirection = -hit.normal;
                    pushDirection.y = 0f;

                    float directionX = Mathf.Abs(pushDirection.x);
                    float directionZ = Mathf.Abs(pushDirection.z);

                    if (directionX > directionZ)
                    {
                        pushDirection = new Vector3(Mathf.Sign(pushDirection.x), 0, 0);
                    }
                    else
                    {
                        pushDirection = new Vector3(0, 0, Mathf.Sign(pushDirection.z));
                    }

                    playerController.move.MoveDirection = pushDirection;
                    playerController.push.PushDirection = pushDirection;

                    transform.rotation = Quaternion.LookRotation(pushDirection);

                    
                }
            }
            playerPush.OnInteract();
        }
        else if(interactAction.triggered && playerClimb.currentInteractable != null && playerClimb.climbTarget.canClimb && playerPush.currentInteractable == null && playerPick.currentInteractable == null && !playerController.move.IsGrounded)
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
                if (Physics.Raycast(position, playerController.move.MoveDirection, out RaycastHit hit, playerController.climb.RayRange, objectLayer) && playerController.climb.IsHold)
                {
                    Vector3 wallDirection = -hit.normal;
                    wallDirection.y = 0f;

                    float directionX = Mathf.Abs(wallDirection.x);
                    float directionZ = Mathf.Abs(wallDirection.z);

                    if (directionX > directionZ)
                    {
                        wallDirection = new Vector3(Mathf.Sign(wallDirection.x), 0, 0);
                    }
                    else
                    {
                        wallDirection = new Vector3(0, 0, Mathf.Sign(wallDirection.z));
                    }
                    
                    playerController.move.MoveDirection = wallDirection;
                    playerController.climb.WallDirection = wallDirection;

                    transform.rotation = Quaternion.LookRotation(wallDirection);
                }
            }
            playerClimb.OnInteract();
        }
        
        if (interactAction.triggered && playerPick.currentInteractable != null && CheckAction())
        {
            playerController.pick.IsHold = !playerController.pick.IsHold;

            playerPick.OnInteract();
        }

        if(interactAction.triggered && playerInteract.currentInteractable != null)
        {
            playerInteract.OnInteract();
        }

        if (playerController.move.IsCrouching)
        {
            isCrouchHit = Physics.Raycast(crouchRayPosition.position, Vector3.up, rayRange, objectLayer);
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

        if (menuAction.WasPerformedThisFrame())
        {
            gameManager.MenuUiSet();
        }
    }

    public bool CheckAction()
    {
        if(playerController.push.IsHold || playerController.climb.IsHold) return false;
        return true;
    }

    public void WalkTimeWhilePull()
    {
        if(playerController.move.MoveDirection != playerController.push.PushDirection && playerController.move.IsWalking)
        {
            playerController.push.PullTime += Time.deltaTime;
        }
        else if(!playerController.move.IsWalking || playerController.move.MoveDirection == playerController.push.PushDirection || playerController.push.PullTime < playerController.push.PullInterval)
        {
            playerController.push.PullTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isCrouchHit ? Color.red : Color.green;

        Gizmos.DrawLine(crouchRayPosition.position, crouchRayPosition.position + Vector3.up * rayRange);


    }    
}
