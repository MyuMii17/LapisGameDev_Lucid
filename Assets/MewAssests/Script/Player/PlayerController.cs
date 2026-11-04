using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Controller
{
    private Rigidbody rb;
    private InputAction moveAction;
    private Vector3 currentRotation;
    private CapsuleCollider collider;
    [SerializeField]private AnimationClip walkClip;
    private Animator animator;
    private RaycastHit hit;
    public Movement move;
    public PushInteract push;
    public ClimbInteract climb;
    void Awake()
    {
        climb.ClimbLayer = LayerMask.GetMask("Edge");
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        move.CurruntCrouchSizeY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Animation();

        if (move.MoveDirection.sqrMagnitude > 0.01f && !push.IsHold && !climb.IsHold)
        {
            Vector3 movementDirection = new Vector3(move.MoveDirection.x, 0, move.MoveDirection.z);

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, move.RotationSpeed * move.RotationSmoothSpeed * Time.deltaTime);
        }

        if (push.IsHold)
        {
            collider.radius = 1;
        }
        else if (move.IsCrouching)
        {
            if (move.IsWalking)
            {
                collider.height = 2.25f;
                collider.radius = 1.2f;
                collider.center = new Vector3(collider.center.x, 1.2f, collider.center.z);
            }
            else
            {
                collider.height = 3;
                collider.radius = 1;
                collider.center = new Vector3(collider.center.x, 1.5f, collider.center.z);
            }
        }
        else
        {
            collider.radius = 0.5f;
            collider.height = 4.05f;
            collider.center = new Vector3(collider.center.x, 2, collider.center.z);
        }
        
    }
    void FixedUpdate()
    {
        // Vector3 position = transform.position;
        // position.y += climb.RayHeight;

        // Debug.DrawRay(position , move.MoveDirection * climb.RayRange , Color.red);
        // if (Physics.Raycast(position, move.MoveDirection, out RaycastHit hit, climb.RayRange, climb.ClimbLayer) && climb.IsHold)
        // {
        //     Vector3 wallDirection = -hit.normal;
        //     wallDirection.y = 0f;

        //     transform.rotation = Quaternion.LookRotation(wallDirection);
        //     climb.WallDirection = wallDirection;
        // }
        if (move.IsWalking)
        {
            move.MovePower = move.WalkSpeed * (push.IsHold || climb.IsHold || move.IsCrouching? 0.5f : (move.IsRunning? move.RunMultiplier : 1));

            Vector3 velocity = rb.linearVelocity;
            velocity.x = move.MovePower * move.MoveDirection.x;
            velocity.z = move.MovePower * move.MoveDirection.z;
            
            rb.linearVelocity = velocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        if (!move.IsJumping && !move.IsGrounded && move.CanJump && !push.IsHold)
        {
            move.CanJump = false;
            move.IsJumping = true;
            rb.linearVelocity = Vector3.zero;
            move.TotalJumpPower =  move.JumpPower * (move.IsWalljump? move.WallJumpMultiplier : 1);
            rb.AddForce(move.TotalJumpPower * Vector3.up, ForceMode.Impulse);
        }
    }
    public void Move()
    {
        move.MoveDirection = moveAction.ReadValue<Vector3>();

        if(!climb.IsHold) return;

        if(climb.WallDirection.x > climb.WallDirection.z)
        {
            move.MoveDirection.x = 0f;
        }
        else if(climb.WallDirection.z > climb.WallDirection.x)
        {
            move.MoveDirection.z = 0f;
        }
    }

    public void ResetVelocity()
    { 
        rb.linearVelocity = Vector3.zero;
    }
    public void Animation()
    {
        animator.SetBool("IsMoving", move.IsWalking? true : false);
        animator.SetBool("IsWalking", move.IsWalking && !move.IsJumping && !push.IsHold && !climb.IsHold? true : false);
        animator.SetBool("IsRunning",  move.IsRunning && !move.IsJumping && !push.IsHold && !climb.IsHold? true : false);
        animator.SetBool("IsJumping",  move.IsJumping && !push.IsHold? true : false);
        animator.SetBool("IsPush",  push.IsHold? true : false);
        animator.SetBool("IsClimbing",  climb.IsHold? true : false);
        animator.SetBool("IsCrouch",  move.IsCrouching? true : false);
    }
}
